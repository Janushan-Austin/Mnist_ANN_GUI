using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MachineLearning
{
    public class NeuralNetwork
    {
        protected Layer[] Layers;
        protected Layer InputLayer;
        protected Layer HiddenLayer;
        protected Layer OutputLayer;
        protected uint NumInputs, NumHiddenNodes, NumOutputs;

        public uint NumEpochs = 2;
        public float TotalEpochs { get; protected set; }
        public uint CompletedEpochs { get; protected set; }

        protected bool LastPropagateWasATest;
        protected bool InvalidatedNetwork;

        public ActivationFunctions m_ActivationFunction;
        protected ConfusionMatrix ConfusionMatrix;

        #region Mini Batch properties
        private uint TotalBatches;
        public uint Batches { get { return TotalBatches; } private set { if (value != 0) { UpdateBatches(value); } } }
        public int CompletedBatches { get; private set; }
        protected bool IncrementBatch()
        {
            if (CompletedBatches < Batches - 1)
            {
                CompletedBatches++;
                return true;
            }
            return false;
        }
        protected void ResetBatch()
        {
            CompletedBatches = 0;
        }
        public bool ChangeBatches(uint batches)
        {
            if (batches == 0 || batches == Batches)
            {
                return false;
            }

            UpdateBatches(batches);
            return true;
        }

        protected void UpdateBatches(uint batches)
        {
            if (CompletedBatches > 0)
            {
                BackPropagate();
            }
            ResetBatch();
            TotalBatches = batches;

            for (uint i = 0; i < Layers.Length; i++)
            {
                Layers[i].Batches = batches;
            }
        }
        #endregion

        public NeuralNetwork(uint numInputs, uint numHiddenNodes, uint numOutputs, uint batches = 1)
        {
            NumInputs = numInputs;
            NumHiddenNodes = numHiddenNodes;
            NumOutputs = numOutputs;

            m_ActivationFunction = new ActivationFunctions();
            Layers = new Layer[3];

            if (batches < 1)
            {
                batches = 1;
            }
            TotalBatches = batches;
            ResetBatch();

            InputLayer = Layers[0] = new Layer(numInputs, 0, m_ActivationFunction, batches);
            HiddenLayer = Layers[1] = new Layer(numHiddenNodes, numInputs, m_ActivationFunction, batches);
            OutputLayer = Layers[Layers.Length - 1] = new Layer(numOutputs, numHiddenNodes, m_ActivationFunction, batches, true);

            ConfusionMatrix = new ConfusionMatrix(numOutputs);

            Reset();
        }

        #region Network Initializations

        public virtual void Reset()
        {
            InvalidatedNetwork = false;
            ConfusionMatrix.ResetMatrix();
            ResetWeights();
            ResetBatch();
            CompletedEpochs = 0;
            TotalEpochs = 0;
        }

        public virtual void ResetWeights()
        {
            for (uint i = 0; i < Layers.Length; i++)
            {
                Layers[i].ResetWeights();
            }
        }

        public void UpdateLayerNumberPerceptrons(uint layer, uint numberPerceptrons)
        {
            if (layer > Layers.Length)
            {
                return;
            }

            InvalidatedNetwork = true;
            Layers[layer].UpdateNumberPerceptrons(numberPerceptrons);
            if (layer + 1 < Layers.Length)
            {
                if (layer == 0)
                {
                    NumInputs = numberPerceptrons;
                    TrainingSet = null;
                    TrainingSetSize = 0;
                    TestSet = null;
                    TestSetSize = 0;
                }
                Layers[layer + 1].UpdateNumberInputs(numberPerceptrons);
            }
            else
            {
                NumOutputs = numberPerceptrons;
                ConfusionMatrix = new ConfusionMatrix(NumOutputs);
                TrainingSet = null;
                TrainingSetSize = 0;
                TestSet = null;
                TestSetSize = 0;
            }

        }

        public bool ChangeLayerActivationFunction(int layer, ActivationFunctions.FunctionTypes funcType)
        {
            if (layer < 0 || layer >= Layers.Length)
            {
                return false;
            }
            return Layers[layer].ChangeActivationFunction(funcType);
        }

        public bool ChangeLayerActivationFunction(ActivationFunctions.FunctionTypes funcType)
        {
            bool allValid = true;
            for (int i = 0; i < Layers.Length; i++)
            {
                if (Layers[i].ChangeActivationFunction(funcType) == false)
                {
                    allValid = false;
                }
            }
            return allValid;
        }

        public void ChangeLayerLearningRate(int layer, float learningRate)
        {
            Layers[layer].ChangeLearningRate(learningRate);
        }
        #endregion

        #region Training and Testing

        protected IDataSet[] TrainingSet;
        protected IDataSet[] TestSet;
        public uint TrainingSetSize;
        public uint TestSetSize;

        public float TrainingSensitivity { get; protected set; }
        public float TestingSensitivity { get; protected set; }

        public virtual bool UploadTrainingSet(IDataSet[] trainingSet)
        {
            if (trainingSet == null || trainingSet[0].ExpectedOutputSet == null || trainingSet[0].InputSet == null ||
               trainingSet[0].InputSet.Length != NumInputs || trainingSet[0].ExpectedOutputSet.Length != NumOutputs)
            {
                return false;
            }

            TrainingSet = trainingSet;
            TrainingSetSize = (uint)trainingSet.Length;

            return true;
        }

        public virtual bool UploadTestSet(IDataSet[] testSet)
        {
            if (testSet == null || testSet[0].InputSet == null || testSet[0].ExpectedOutputSet == null ||
               testSet[0].InputSet.Length != NumInputs || testSet[0].ExpectedOutputSet.Length != NumOutputs)
            {
                return false;
            }

            TestSet = testSet;
            TestSetSize = (uint)testSet.Length;

            return true;
        }

        public virtual void Train(bool continueLastTrainingSet = false)
        {
            if (TrainingSet == null)
            {
                return;
            }

            float prevTrainingSensitivity = TrainingSensitivity;
            float epochCompletionPerPropagation = 1.0f / TrainingSetSize;
            uint startSet = continueLastTrainingSet == true ? (uint)((TotalEpochs - (uint)TotalEpochs) * TrainingSetSize) : 0;
            ConfusionMatrix.ResetMatrix();
            for (uint epoch = 0; epoch < NumEpochs; epoch++)
            {
                for (uint set = startSet; set < TrainingSetSize; set++)
                {
                    Propagate(TrainingSet[set], testing: false);
                    EvaluateOutput();
                    TotalEpochs += epochCompletionPerPropagation;
                }
                startSet = 0;
                CompletedEpochs++;

                if (CompletedBatches > 0)
                {
                    BackPropagate();
                }

                TrainingSensitivity = Sensitivity();

                if(TrainingSensitivity - prevTrainingSensitivity < 0.005f)
                {
                    ChangeLayerLearningRate(0, Layers[0].ActivationFunction.FunctionLearningRate / 2.0f);
                }
            }
        }

        public virtual void TestTrainingSet()
        {
            if (TrainingSet == null)
            {
                return;
            }

            ConfusionMatrix.ResetMatrix();

            for (uint set = 0; set < TrainingSetSize; set++)
            {
                Propagate(TrainingSet[set], testing: true);
                EvaluateOutput();
            }

            TrainingSensitivity = Sensitivity();
        }

        public virtual void Test()
        {
            if (TestSet == null)
            {
                return;
            }

            float prevTestSensitivity = TestingSensitivity;
            ConfusionMatrix.ResetMatrix();

            for (uint set = 0; set < TestSetSize; set++)
            {
                Propagate(TestSet[set], testing: true);
                EvaluateOutput();
            }

            TestingSensitivity = Sensitivity();
        }



        #endregion

        #region Propagation
        public bool Propagate(IDataSet data, bool testing = false)
        {
            if (data.InputSet.Length != NumInputs || data.ExpectedOutputSet.Length != NumOutputs)
            {
                return false;
            }

            if (InvalidatedNetwork == true)
            {
                Reset();
            }

            if (testing == true && CompletedBatches > 0)
            {
                BackPropagate();
                CompletedBatches = 0;
            }

            //for (uint i = 0; i < NumInputs; i++)
            //{
            //    InputLayer.Perceptrons[i].Outputs[CompletedBatches] = inputs[i];
            //}
            //for (uint i = 0; i < OutputLayer.Perceptrons.Length; i++)
            //{
            //    OutputLayer.Perceptrons[i].ExpectedOutputs[CompletedBatches] = expectedOutputs[i];
            //}

            InputLayer.UploadInputs(data.InputSet, CompletedBatches);
            OutputLayer.UploadExpectedOutputs(data.ExpectedOutputSet, CompletedBatches);

            ForwardPropagate();
            if (testing == false)
            {
                if (IncrementBatch() == false)
                {
                    CompletedBatches = (int)Batches;
                    BackPropagate();
                }
            }
            LastPropagateWasATest = testing;

            return true;
        }

        protected virtual void ForwardPropagate()
        {
            for (uint L = 1; L < Layers.Length; L++)
            {
                Layer currLayer = Layers[L];
                Layer prevLayer = Layers[L - 1];
                currLayer.ForwardPropagate(prevLayer, CompletedBatches);

                //for (uint p = 0; p < currLayer.Perceptrons.Length; p++)
                //{
                //    Perceptron perceptron = currLayer.Perceptrons[p];
                //    perceptron.LinearSums[CompletedBatches] = perceptron.Weights[perceptron.Weights.Length - 1];

                //    for (uint w = 0; w < perceptron.Weights.Length - 1; w++)
                //    {
                //        perceptron.LinearSums[CompletedBatches] += perceptron.Weights[w] * prevLayer.Perceptrons[w].Outputs[CompletedBatches];
                //    }

                //    perceptron.Outputs[CompletedBatches] = currLayer.ActivationFunction.evaluate(perceptron.LinearSums[CompletedBatches], false);
                //}
            }
        }

        protected virtual void BackPropagate()
        {
            OutputLayer.BackPropagate(Layers[Layers.Length - 2], null, CompletedBatches);
            for (int layer = Layers.Length - 2; layer > 0; layer--)
            {
                Layers[layer].BackPropagate(Layers[layer - 1], Layers[layer + 1], CompletedBatches);
            }
            ResetBatch();
        }
        #endregion


        #region Network Results and Statistics
        public float[] Outputs()
        {
            int currentBatch = (CompletedBatches != 0 ? CompletedBatches : (int)Batches) - 1;
            if (LastPropagateWasATest == true)
            {
                currentBatch = 0;
            }

            float[] outputs = new float[OutputLayer.Perceptrons.Length];

            for (uint i = 0; i < OutputLayer.Perceptrons.Length; i++)
            {
                outputs[i] = OutputLayer.Perceptrons[i].Outputs[currentBatch];
            }

            return outputs;
        }

        public float[] ExpectedOutputs()
        {
            int currentBatch = (CompletedBatches != 0 ? CompletedBatches : (int)Batches) - 1;
            if (LastPropagateWasATest == true)
            {
                currentBatch = 0;
            }

            float[] outputs = new float[OutputLayer.Perceptrons.Length];

            for (uint i = 0; i < OutputLayer.Perceptrons.Length; i++)
            {
                outputs[i] = OutputLayer.Perceptrons[i].ExpectedOutputs[currentBatch];
            }

            return outputs;
        }

        public float[] Errors()
        {
            int currentBatch = (CompletedBatches != 0 ? CompletedBatches : (int)Batches) - 1;
            if (LastPropagateWasATest == true)
            {
                currentBatch = 0;
            }
            float[] errors = new float[OutputLayer.Perceptrons.Length];

            for (uint i = 0; i < OutputLayer.Perceptrons.Length; i++)
            {
                errors[i] = OutputLayer.Perceptrons[i].Outputs[currentBatch] - OutputLayer.Perceptrons[i].ExpectedOutputs[currentBatch];
            }

            return errors;
        }
        public virtual void EvaluateOutput()
        {
            float[] expectedOutputs = ExpectedOutputs();
            uint actualIndex = 0;
            float maxActualValue = expectedOutputs[0];

            float[] outputs = Outputs();
            uint PredictedIndex = 0;
            float maxPredictedValue = outputs[0];
            for (uint i = 1; i < outputs.Length; i++)
            {
                if (outputs[i] > maxPredictedValue)
                {
                    maxPredictedValue = outputs[i];
                    PredictedIndex = i;
                }

                if (expectedOutputs[i] > maxActualValue)
                {
                    maxActualValue = expectedOutputs[i];
                    actualIndex = i;
                }
            }

            ConfusionMatrix.AddToIndex(actualIndex, PredictedIndex);
        }

        #region Confusion Matrix Results
        public virtual uint TruePositives(int classType = -1)
        {
            return ConfusionMatrix.GetTruePositives(classType);
        }

        public virtual uint TrueNegatives(int classType = -1)
        {
            return ConfusionMatrix.GetTrueNegatives(classType);
        }

        public virtual uint FalsePositives(int classType = -1)
        {
            return ConfusionMatrix.GetFalsePositives(classType);
        }

        public virtual uint FalseNegatives(int classType = -1)
        {
            return ConfusionMatrix.GetFalseNegatives(classType);
        }

        public virtual float ClassificationAccuracy(int classType = -1)
        {
            return ConfusionMatrix.ClassificationAccuracy(classType) *100;
        }

        public virtual float ClassificationError(int classType = -1)
        {
            return (1.0f - ConfusionMatrix.ClassificationAccuracy(classType)) *100;
        }

        public virtual float Precision(int classType = -1)
        {
            return ConfusionMatrix.Precision(classType) * 100;
        }

        public virtual float Specificity(int classType = -1)
        {
            return ConfusionMatrix.Specificity(classType) * 100;
        }

        public virtual float Sensitivity(int classType = -1)
        {
            return ConfusionMatrix.Sensitivity(classType) * 100;
        }
        #endregion

        #endregion
    }
}
