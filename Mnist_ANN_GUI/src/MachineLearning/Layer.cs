using System;

namespace MachineLearning
{
    public class Layer
    {
        public Perceptron[] Perceptrons;
        public ActivationFunctions ActivationFunction;

        private uint InputCount;
        private bool OutputLayer;

        public Layer(uint numPerceptrons, uint numInputs, ActivationFunctions activationFunction, uint batches = 1, bool outputLayer = false)
        {
            ActivationFunction = activationFunction;
            Perceptrons = new Perceptron[numPerceptrons];
            InputCount = numInputs;
            OutputLayer = outputLayer;

            if (batches == 0)
            {
                batches = 1;
            }
            TotalBatches = batches;

            for (uint i = 0; i < Perceptrons.Length; i++)
            {
                Perceptrons[i] = new Perceptron(InputCount, Batches, OutputLayer);
            }
        }

        public virtual void ResetWeights()
        {
            for (uint i = 0; i < Perceptrons.Length; i++)
            {
                Perceptrons[i].ResetWeights();
            }
        }

        #region Layer Property Updaters
        public void UpdateNumberPerceptrons(uint numPerceptrons)
        {
           if(numPerceptrons != 0 && numPerceptrons != Perceptrons.Length)
            {
                Perceptron[] PerceptronPlaceHolder = Perceptrons;
                Perceptrons = new Perceptron[numPerceptrons];

                for(uint i = 0; i < PerceptronPlaceHolder.Length && i < numPerceptrons; i++)
                {
                    Perceptrons[i] = PerceptronPlaceHolder[i];
                }

                for(uint i = (uint)PerceptronPlaceHolder.Length; i < numPerceptrons; i++)
                {
                    Perceptrons[i] = new Perceptron(InputCount, Batches, OutputLayer);
                }
            }
        }

        public void UpdateNumberInputs(uint numInputs)
        {
            if(numInputs != 0)
            {
                InputCount = numInputs;
                for (uint i = 0; i < Perceptrons.Length; i++)
                {
                    Perceptrons[i].UpdateNumberInputs(numInputs);
                }
            }
        }

        public virtual void UploadInputs(float[] inputs, int completedBatches)
        {
            for(uint i = 0; i < Perceptrons.Length; i++)
            {
                Perceptrons[i].Outputs[completedBatches] = inputs[i];
            }
        }

        public virtual void UploadExpectedOutputs(float[] outputs, int completedBatches)
        {
            for (uint i = 0; i < Perceptrons.Length; i++)
            {
                Perceptrons[i].ExpectedOutputs[completedBatches] = outputs[i];
            }
        }

        public bool ChangeActivationFunction(ActivationFunctions.FunctionTypes funcType)
        {
            return ActivationFunction.ChangeFunction(funcType);
        }

        public virtual void ForwardPropagate(Layer prevLayer, int completedBatches)
        {
            for (uint p = 0; p < Perceptrons.Length; p++)
            {
                Perceptron perceptron = Perceptrons[p];
                perceptron.LinearSums[completedBatches] = perceptron.Weights[perceptron.Weights.Length - 1];

                for (uint w = 0; w < perceptron.Weights.Length - 1; w++)
                {
                    perceptron.LinearSums[completedBatches] += perceptron.Weights[w] * prevLayer.Perceptrons[w].Outputs[completedBatches];
                }

                perceptron.Outputs[completedBatches] = ActivationFunction.evaluate(perceptron.LinearSums[completedBatches], false);
            }
        }

        public virtual void BackPropagate(Layer prevLayer, Layer forwardLayer, int completedBatches)
        {
            if(forwardLayer == null)
            {
                if(OutputLayer == true)
                {
                    BackPropagateOutputLayer(prevLayer, completedBatches);
                }
            }
            else
            {
                for (uint perceptron = 0; perceptron < Perceptrons.Length; perceptron++)
                {
                    Perceptron currPerceptron = Perceptrons[perceptron];

                    for (uint batch = 0; batch < completedBatches; batch++)
                    {
                        currPerceptron.BackPropigationDerivatives[batch] = 0;
                        for (uint connectingPerceptron = 0; connectingPerceptron < forwardLayer.Perceptrons.Length; connectingPerceptron++)
                        {
                            currPerceptron.BackPropigationDerivatives[batch] +=
                                      forwardLayer.Perceptrons[connectingPerceptron].BackPropigationDerivatives[batch] *
                                      forwardLayer.Perceptrons[connectingPerceptron].OldWeights[perceptron];
                        }

                        //TODO make evaluate function to evaluate derivative in terms of output from the original evaluation
                        currPerceptron.BackPropigationDerivatives[batch] *= 
                            ActivationFunction.evaluateDerivative(Perceptrons[perceptron].Outputs[batch], true);
                    }

                    float SummationBackPropDerivative = 0; //currPerceptron.BackPropigationDerivatives * currLayer.ActivationFunction.FunctionLearningRate;
                    int weight = 0;
                    for (; weight < currPerceptron.Weights.Length - 1; weight++)
                    {
                        currPerceptron.OldWeights[weight] = currPerceptron.Weights[weight];

                        SummationBackPropDerivative = 0;
                        for (uint batch = 0; batch < completedBatches; batch++)
                        {
                            SummationBackPropDerivative += currPerceptron.BackPropigationDerivatives[batch] * prevLayer.Perceptrons[weight].Outputs[batch];
                        }

                        currPerceptron.Weights[weight] -= ActivationFunction.FunctionLearningRate * SummationBackPropDerivative / completedBatches;
                    }

                    //this is adjusting the biases throughout the network since they do not have perceptrons
                    //they connect with from a previous layer
                    currPerceptron.OldWeights[weight] = currPerceptron.Weights[weight];
                    SummationBackPropDerivative = 0;
                    for (uint batch = 0; batch < completedBatches; batch++)
                    {
                        SummationBackPropDerivative += currPerceptron.BackPropigationDerivatives[batch];
                    }
                    currPerceptron.Weights[weight] -= ActivationFunction.FunctionLearningRate * SummationBackPropDerivative / completedBatches;
                }
            }
        }

        public void ChangeLearningRate(float learningRate)
        {
            ActivationFunction.FunctionLearningRate = learningRate;
        }

        protected virtual void BackPropagateOutputLayer(Layer prevLayer, int completedBatches)
        {
            for (uint outputPerceptron = 0; outputPerceptron < Perceptrons.Length; outputPerceptron++)
            {
                Perceptron currPerceptron = Perceptrons[outputPerceptron];

                for (uint batch = 0; batch < completedBatches; batch++)
                {
                    //could divide by batches and multiply by learning rate here possibly
                    currPerceptron.BackPropigationDerivatives[batch] = (currPerceptron.Outputs[batch] - currPerceptron.ExpectedOutputs[batch]) *
                                                               ActivationFunction.evaluateDerivative(currPerceptron.Outputs[batch], true);
                }

                float SummationBackPropDerivative = 0;
                int weight = 0;
                for (; weight < currPerceptron.Weights.Length - 1; weight++)
                {
                    currPerceptron.OldWeights[weight] = currPerceptron.Weights[weight];

                    SummationBackPropDerivative = 0;
                    for (uint batch = 0; batch < completedBatches; batch++)
                    {
                        SummationBackPropDerivative += currPerceptron.BackPropigationDerivatives[batch] * prevLayer.Perceptrons[weight].Outputs[batch];
                    }

                    currPerceptron.Weights[weight] -= ActivationFunction.FunctionLearningRate * SummationBackPropDerivative / completedBatches;
                }

                currPerceptron.OldWeights[weight] = currPerceptron.Weights[weight];

                //this is adjusting the biases throughout the network since they do not have perceptrons
                //they connect with from a previous layer
                SummationBackPropDerivative = 0;
                for (uint batch = 0; batch < completedBatches; batch++)
                {
                    SummationBackPropDerivative += currPerceptron.BackPropigationDerivatives[batch];
                }
                currPerceptron.Weights[weight] -= ActivationFunction.FunctionLearningRate * SummationBackPropDerivative / completedBatches;
            }
        }
        #endregion

        #region Mini Batch Properties
        private uint TotalBatches;
        public uint Batches { get { return TotalBatches; } set { if (value != 0) { UpdateBatches(value); } } }
        private void UpdateBatches(uint batches)
        {
            TotalBatches = batches;

            for (uint i = 0; i < Perceptrons.Length; i++)
            {
                Perceptrons[i].Batches = batches;
            }
        }
        #endregion
    }
}
