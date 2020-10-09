using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning
{
    public class ConfusionMatrix
    {
        private uint[,] confusionMatrix;
        public ConfusionMatrix(uint n)
        {
            uint r = n, c = n;
            confusionMatrix = new uint[r, c];
            for (r = 0; r < confusionMatrix.GetLength(0); r++)
            {
                for (c = 0; c < confusionMatrix.GetLength(1); c++)
                {
                    confusionMatrix[r, c] = 0;
                }
            }
        }

        public ConfusionMatrix(uint[,] matrix)
        {
            confusionMatrix = new uint[matrix.GetLength(0), matrix.GetLength(1)];
            for (uint r = 0; r < confusionMatrix.GetLength(0); r++)
            {
                for (uint c = 0; c < confusionMatrix.GetLength(1); c++)
                {
                    confusionMatrix[r, c] = matrix[r, c];
                }
            }
        }

        public void ResetMatrix()
        {
            for (uint r = 0; r < confusionMatrix.GetLength(0); r++)
            {
                for (uint c = 0; c < confusionMatrix.GetLength(1); c++)
                {
                    confusionMatrix[r, c] = 0;
                }
            }
        }

        public void AddToIndex(uint actual, uint predicted, uint incAmount = 1)
        {
            if (actual < 0 || actual >= confusionMatrix.GetLength(0) || predicted < 0 || predicted >= confusionMatrix.GetLength(1))
            {
                return;
            }

            confusionMatrix[actual, predicted] += incAmount;
        }

        public uint GetTruePositives(int elementIndex = -1)
        {
            uint truePositives = 0;
            if (elementIndex <= -1)
            {
                for (int i = 0; i < confusionMatrix.GetLength(0); i++)
                {
                    truePositives += GetTruePositives(i);
                }
            }
            else if (elementIndex < confusionMatrix.GetLength(0))
            {
                truePositives = confusionMatrix[elementIndex, elementIndex];
            }

            return truePositives;
        }

        public uint GetTrueNegatives(int elementIndex = -1)
        {
            uint trueNegatives = 0;

            if (elementIndex == -1)
            {
                for (int i = 0; i < confusionMatrix.GetLength(0); i++)
                {
                    trueNegatives += GetTrueNegatives(i) - 2 * GetTruePositives(i);
                }
            }
            else if (elementIndex < confusionMatrix.GetLength(0))
            {
                for (uint r = 0; r < confusionMatrix.GetLength(0); r++)
                {
                    if (r != elementIndex)
                    {
                        for (uint c = 0; c < confusionMatrix.GetLength(1); c++)
                        {
                            if (c != elementIndex)
                            {
                                trueNegatives += confusionMatrix[r, c];
                            }
                        }
                    }
                }
            }

            return trueNegatives;
        }

        public uint GetFalsePositives(int elementIndex = -1)
        {
            uint falsePositives = 0;

            if (elementIndex == -1)
            {
                for (int i = 0; i < confusionMatrix.GetLength(0); i++)
                {
                    falsePositives += GetFalsePositives(i);
                }
            }
            else if (elementIndex < confusionMatrix.GetLength(0))
            {
                for (uint r = 0; r < confusionMatrix.GetLength(0); r++)
                {
                    if (r != elementIndex)
                    {
                        falsePositives += confusionMatrix[r, elementIndex];
                    }
                }
            }

            return falsePositives;
        }

        public uint GetFalseNegatives(int elementIndex = -1)
        {
            uint falseNegatives = 0;

            if (elementIndex == -1)
            {
                for (int i = 0; i < confusionMatrix.GetLength(0); i++)
                {
                    falseNegatives += GetFalseNegatives(i);
                }
            }
            else if (elementIndex < confusionMatrix.GetLength(0))
            {
                for (uint c = 0; c < confusionMatrix.GetLength(1); c++)
                {
                    if (c != elementIndex)
                    {
                        falseNegatives += confusionMatrix[elementIndex, c];
                    }
                }
            }
            return falseNegatives;
        }

        public float ClassificationAccuracy(int classType = -1)
        {
            float numerator = 0;
            float denominator = 0;
            uint classTP, classTN, classFP, classFN;

            classTP = GetTruePositives(classType); classFP = GetFalsePositives(classType);
            classTN = GetTrueNegatives(classType); classFN = GetFalseNegatives(classType);
            numerator += classTP + classTN;
            denominator = classTP + classTN + classFP + classFN;

            return denominator != 0 ? numerator / denominator : 0.0f;
        }

        public float Precision(int classType = -1)
        {
            float numerator = 0;
            float denominator = 0;
            uint classTP, classFP;

            classTP = GetTruePositives(classType); classFP = GetFalsePositives(classType);
            numerator += classTP;
            denominator = classTP + classFP;

            return denominator != 0 ? numerator / denominator : 0.0f;
        }

        public float Specificity(int classType = -1)
        {
            float numerator = 0;
            float denominator = 0;
            uint classTN, classFP;

            classTN = GetTrueNegatives(classType); classFP = GetFalsePositives(classType);
            numerator += classTN;
            denominator = classTN + classFP;

            return denominator != 0 ? numerator / denominator : 0.0f;
        }

        public float Sensitivity(int classType = -1)
        {
            float numerator = 0;
            float denominator = 0;
            uint classTP, classFN;

            classTP = GetTruePositives(classType); classFN = GetFalseNegatives(classType);
            numerator += classTP;
            denominator = classTP + classFN;

            return denominator != 0 ? numerator / denominator : 0.0f;
        }
    }
}
