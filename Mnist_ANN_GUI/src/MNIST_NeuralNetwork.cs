using System;
using MachineLearning;


public class MNIST_NeuralNetwork : NeuralNetwork
{
    public MNIST_NeuralNetwork() : base(numInputs: 784, numHiddenNodes: 90, numOutputs: 10, batches: 1)
    {
        m_ActivationFunction.ChangeFunction(ActivationFunctions.FunctionTypes.Sigmoid);
        m_ActivationFunction.FunctionLearningRate = .5f;
    }

    public void Train(Func<int, bool, bool> progressFunc, bool continueLastTrainingSet = false)
    {
        if (TrainingSet == null)
        {
            return;
        }
        float epochCompletionPerPropagation = 1.0f / TrainingSetSize;
        uint startSet = continueLastTrainingSet == true ? (uint)((TotalEpochs - (uint)TotalEpochs) * TrainingSetSize) : 0;

        float deltaSetPercent = 1.0f / (NumEpochs * TrainingSetSize) * 100.0f;
        float percent = startSet * deltaSetPercent;
        float prevTrainingSensitivity = TrainingSensitivity;
        bool earlyExit = false;

        for (uint epoch = 0; epoch < NumEpochs; epoch++)
        {
            ConfusionMatrix.ResetMatrix();
            for (uint set = startSet; set < TrainingSetSize; set++, percent += deltaSetPercent)
            {
                if(progressFunc((int)percent, false) == false)
                {
                    earlyExit = true;
                    break;
                }
                Propagate(TrainingSet[set], testing: false);
                EvaluateOutput();
                TotalEpochs += epochCompletionPerPropagation;
            }
            startSet = 0;
            if (CompletedBatches > 0)
            {
                BackPropagate();
            }
            TrainingSensitivity = Sensitivity();
            Test();

            while(TotalEpochs - CompletedEpochs > 1.0f)
            {
                CompletedEpochs++;
            }
            if(earlyExit == true)
            {
                return;
            }



            if (TrainingSensitivity - prevTrainingSensitivity < 0.5f)
            {
                ChangeLayerLearningRate(0, Layers[0].ActivationFunction.FunctionLearningRate / 2.0f);
            }

            if(progressFunc((int)percent, true) == false)
            {
                return;
            }
        }
        
    }

    public void Test(Action<int, bool> progressFunc)
    {
        if (TestSet == null)
        {
            return;
        }
        float deltaSetPercent = 1.0f / (TestSetSize) * 100.0f;
        float percent = 0.0f;
        float prevTestSensitivity = TestingSensitivity;
        ConfusionMatrix.ResetMatrix();

        for (uint set = 0; set < TestSetSize; set++, percent += deltaSetPercent)
        {
            progressFunc((int)percent, false);
            Propagate(TestSet[set], testing: true);
            EvaluateOutput();
        }
        TestingSensitivity = Sensitivity();
        progressFunc((int)percent, false);
    }
}