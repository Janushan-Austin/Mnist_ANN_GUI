using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class MnistImage : MachineLearning.IDataSet
{
    public float[] Pixels;
    public int Width, Height;

    private int Label_;
    public int Label { get { return Label_; } set { ProcessLabel(value); } }

    public MnistImage()
    {
        Initialize();
    }

    public MnistImage(int width, int height)
    {
        Initialize(width, height);
    }

    public MnistImage(int width, int height, int label)
    {
        Initialize(width, height, label);
    }

    private void Initialize(int width = 0, int height = 0, int label = -1)
    {
        Width = width;
        Height = height;
        Pixels = Width > 0 && Height > 0 ? new float[Width * Height] : null;

        Label_ = label;
        ExpectedOutputSet = new float[10];
        if (label >= 0 && label <= 9)
        {
            ExpectedOutputSet[label] = 1;
        }
    }

    private void ProcessLabel(int label)
    {
        if (label < 0 || label > 9)
        {
            return;
        }
        Label_ = label;
        ExpectedOutputSet[label] = 1;
    }

    #region IDataSet Implementation
    public float[] InputSet { get { return Pixels; } set { Pixels = value; } }
    public float[] ExpectedOutputSet { get; set; }
    #endregion

    public enum MessageSection
    {
        HEAD,
        BODY,
        TAIL
    }

    public static MnistImage[] ProcessMNISTFile(string imgPath, string labelPath, Action<string, MessageSection> updaterCallback = null)
    {
        MnistImage[] images;
        int numImages, rows, cols;

        StreamReader imageReaderStream = new StreamReader(imgPath);
        BinaryReader imageReader = new BinaryReader(imageReaderStream.BaseStream);
        StreamReader labelReaderStream = new StreamReader(labelPath);
        BinaryReader labelReader = new BinaryReader(labelReaderStream.BaseStream);

        //reading hear information
        {
            //discard magic number
            imageReader.ReadBytes(4);
            labelReader.ReadBytes(4);
            //number of images
            byte[] ReadBytes = imageReader.ReadBytes(4);
            labelReader.ReadBytes(4);
            numImages = (ReadBytes[0] << 24) + (ReadBytes[1] << 16) + (ReadBytes[2] << 8) + ReadBytes[3];

            ReadBytes = imageReader.ReadBytes(4);
            rows = (ReadBytes[0] << 24) + (ReadBytes[1] << 16) + (ReadBytes[2] << 8) + ReadBytes[3];

            ReadBytes = imageReader.ReadBytes(4);
            cols = (ReadBytes[0] << 24) + (ReadBytes[1] << 16) + (ReadBytes[2] << 8) + ReadBytes[3];
        }

        updaterCallback?.Invoke($"Processing Training Set Images from path: {imgPath}", MessageSection.HEAD);
        images = new MnistImage[numImages];
        for (int i = 0; i < numImages; i++)
        {
            images[i] = new MnistImage(cols, rows);
            images[i].Pixels = new float[rows * cols];
        }


        byte[] labels = labelReader.ReadBytes(numImages);
        byte[] imagesPixels = imageReader.ReadBytes(numImages * rows * cols);

        int imagesPixelIndex = 0;
        int pixelIndex = 0;
        for (int image = 0; image < numImages; image++)
        {
            images[image].Label = labels[image];
            updaterCallback?.Invoke($"Processing Image {image + 1} out of {numImages}", MessageSection.BODY);

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++, imagesPixelIndex++, pixelIndex++)
                {
                    images[image].Pixels[pixelIndex] = imagesPixels[imagesPixelIndex] / 255.0f;
                }
            }
            pixelIndex = 0;
        }

        labels = imagesPixels = null;

        return images;
    }
}

