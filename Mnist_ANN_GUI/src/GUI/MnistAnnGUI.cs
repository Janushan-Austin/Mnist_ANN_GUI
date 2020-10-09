using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MachineLearning;

namespace Mnist_ANN_GUI
{
    public partial class MnistAnnGUI : Form
    {
        MNIST_NeuralNetwork mnistNetwork;
        MnistImage[] trainingImages;
        MnistImage[] testImages;
        MnistImage[] selectedImageSet;
        int selectedImageIndex;

        BackgroundWorker NetworkWorker;

        public MnistAnnGUI()
        {
            InitializeComponent();

            string trainImgPath = "res/train-images.idx3-ubyte"
                , trainLblPath = "res/train-labels.idx1-ubyte"
                , testImgPath = "res/t10k-images.idx3-ubyte"
                , testLblPath = "res/t10k-labels.idx1-ubyte";

            trainingImages = MnistImage.ProcessMNISTFile(trainImgPath, trainLblPath);
            testImages = MnistImage.ProcessMNISTFile(testImgPath, testLblPath);

            mnistNetwork = new MNIST_NeuralNetwork();
            mnistNetwork.UploadTrainingSet(trainingImages);
            mnistNetwork.UploadTestSet(testImages);
            mnistNetwork.NumEpochs = 1;

            //mnistNetwork.TestTrainingSet();
            mnistNetwork.Test();
            UpdateSensitivity();

            selectedImageSet = testImages;

            LoadImage(0);
            UseTrainingSetCheckBox_CheckedChanged(null, null);

            NetworkWorker = new BackgroundWorker();
            NetworkWorker.WorkerReportsProgress = true;
            NetworkWorker.DoWork += TrainNetwork;
            NetworkWorker.ProgressChanged += (object sender, ProgressChangedEventArgs e) =>
            {
                int percent = e.ProgressPercentage;
                TrainingProgressLabel.Text = $"Network Progress: {percent}%";
                TrainingProgressBar.Value = percent;
            };
            NetworkWorker.RunWorkerCompleted += (object sender, RunWorkerCompletedEventArgs e) =>
            {
                UpdateSensitivity();
                SetNetworkActive(true);
                TrainingProgressLabel.Text = e.Cancelled == false ? $"Network Progress: {0}% (finished)" : "Network Progress: {0}% (cancelled)";
                TrainingProgressBar.Value = 0;
                TotalEpochLabel.Text = $"Total Trained Epochs: {mnistNetwork.TotalEpochs.ToString("n2")}";
            };
            NetworkWorker.WorkerSupportsCancellation = true;

            NumEpochsComboBox.SelectedIndex = 0;
        }



        private void ImageSelectionTextBox_TextChanged(object sender, EventArgs e)
        {
            int index = 0;
            if (int.TryParse(ImageSelectionTextBox.Text, out index) == true)
            {
                InvalidImageLabel.Visible = false;
                if (index >= 0 && index < selectedImageSet.Length)
                {
                    LoadImage(index);
                }
                else
                {
                    InvalidImageLabel.Visible = true;
                }
            }
            else if (ImageSelectionTextBox.Text == "")
            {
                InvalidImageLabel.Visible = false;
            }
            else
            {
                InvalidImageLabel.Visible = true;
            }
        }

        private void LoadImage(int index)
        {
            if (index < 0 || index >= selectedImageSet.Length)
            {
                return;
            }

            selectedImageIndex = index;

            int width = selectedImageSet[index].Width;
            int height = selectedImageSet[index].Height;
            Bitmap bm = new Bitmap(width, height);
            int pixelIndex = 0;

            MnistImageLabel.Text = $"Image Label: {selectedImageSet[index].Label}";

            for (int py = 0; py < height; py++)
            {
                for (int px = 0; px < width; px++, pixelIndex++)
                {
                    int pixelColor = (int)(selectedImageSet[index].Pixels[pixelIndex] * 255.0f);
                    bm.SetPixel(px, py, Color.FromArgb(pixelColor, pixelColor, pixelColor));
                }
            }
            MnistPictureBox.Image = bm;

            mnistNetwork.Propagate(selectedImageSet[selectedImageIndex], true);

            float[] testOutputs = mnistNetwork.Outputs();
            int predictedOutputIndex = 0;
            float predictedOutputValue = testOutputs[0];
            for (int i = 1; i < testOutputs.Length; i++)
            {
                if (testOutputs[i] > predictedOutputValue)
                {
                    predictedOutputValue = testOutputs[i];
                    predictedOutputIndex = i;
                }
            }

            PredictionLabel.Text = $"Network Label Prediction: {predictedOutputIndex}";
        }

        private void UseTrainingSetCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (UseTrainingSetCheckBox.Checked == true)
            {
                selectedImageSet = trainingImages;
            }
            else
            {
                selectedImageSet = testImages;
            }

            ImageSelectionlabel.Text = $"Choose image to test (0-{selectedImageSet.Length - 1})";

            LoadImage(selectedImageIndex);
        }

        private void TrainButton_Click(object sender, EventArgs e)
        {
            SetNetworkActive(false);
            NetworkWorker.RunWorkerAsync();
        }

        private void TrainNetwork(object sender, DoWorkEventArgs e)
        {
            mnistNetwork.Train((int percent, bool epochComplete) =>
            {
                if (epochComplete == true)
                {
                    ProgressChangedEventHandler func = null;
                    func = (object sender2, ProgressChangedEventArgs e2) =>
                   {

                       UpdateSensitivity();
                       TotalEpochLabel.Text = $"Total Trained Epochs: {mnistNetwork.CompletedEpochs}";
                       NetworkWorker.ProgressChanged -= func;
                   };
                    NetworkWorker.ProgressChanged += func;
                }                
                NetworkWorker.ReportProgress(percent);
                if (NetworkWorker.CancellationPending == true)
                {
                    e.Cancel = true;
                    return false;
                }
                return true;
            }
            , true);            
        }

        private void UpdateSensitivity()
        {
            TrainingSensLabel.Text = $"Training Sensitivity: {mnistNetwork.TrainingSensitivity}";
            TestingSensLabel.Text = $"Testing Sensitivity: {mnistNetwork.TestingSensitivity}";
        }

        private void SetNetworkActive(bool state)
        {
            TrainButton.Enabled = state;
            TestImageButton.Enabled = state;
            NumEpochsComboBox.Enabled = state;
            ImageSelectionTextBox.Enabled = state;
            ResetNetworkButton.Enabled = state;
        }

        private void TestImageButton_Click(object sender, EventArgs e)
        {
            mnistNetwork.Propagate(selectedImageSet[selectedImageIndex], true);

            float[] testOutputs = mnistNetwork.Outputs();
            int predictedOutputIndex = 0;
            float predictedOutputValue = testOutputs[0];
            for (int i = 1; i < testOutputs.Length; i++)
            {
                if (testOutputs[i] > predictedOutputValue)
                {
                    predictedOutputValue = testOutputs[i];
                    predictedOutputIndex = i;
                }
            }
            PredictionLabel.Text = $"Network Label Prediction: {predictedOutputIndex}";
        }

        private void NumEpochsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            uint numEpochs = uint.Parse(NumEpochsComboBox.SelectedItem.ToString());
            TrainButton.Text = $"Train {numEpochs} Epoch" + (numEpochs > 1 ? "s" : "");
            mnistNetwork.NumEpochs = numEpochs;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            NetworkWorker.CancelAsync();
        }

        private void ResetNetworkButton_Click(object sender, EventArgs e)
        {
            mnistNetwork.Reset();
            //mnistNetwork.TestTrainingSet();
            mnistNetwork.Test();

            UpdateSensitivity();

            TrainingProgressLabel.Text = $"Network Progress: {0}% (Reset)";
            TrainingProgressBar.Value = 0;
            TotalEpochLabel.Text = $"Total Trained Epochs: {mnistNetwork.TotalEpochs.ToString("n2")}";
        }
    }
}
