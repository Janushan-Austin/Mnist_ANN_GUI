using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning
{
    public class Perceptron
    {
        public float[] Weights;
        public float[] OldWeights;

        public float[] LinearSums;
        public float[] Outputs;
        public float[] ExpectedOutputs;
        public float[] BackPropigationDerivatives;

        public uint InputCount;
        public bool OutputLayerPerceptron;

        #region Mini Batch Properties
        private uint TotalBatches;
        public uint Batches { get { return TotalBatches; } set { if (value != 0) { UpdateBatches(value); } } }
        private void UpdateBatches(uint batches)
        {
            TotalBatches = batches;

            LinearSums = new float[batches];
            Outputs = new float[batches];
            BackPropigationDerivatives = new float[batches];

            ExpectedOutputs = OutputLayerPerceptron ? new float[batches] : null;

            for (uint b = 0; b < batches; b++)
            {
                LinearSums[b] = Outputs[b] = BackPropigationDerivatives[b] = 0;
            }
        }
        #endregion

        public Perceptron(uint numInputs, uint batches = 1, bool outputLayerPerceptron = false)
        {
            Weights = new float[numInputs + 1];
            OldWeights = new float[numInputs + 1];

            OutputLayerPerceptron = outputLayerPerceptron;

            if (batches < 1)
            {
                batches = 1;
            }
            UpdateBatches(batches);
        }

        public void ResetWeights()
        {
            for(uint i = 0; i < Weights.Length; i++)
            {
                Weights[i] = Rand.NextFloat() - 0.5f;
            }
        }

        public void UpdateNumberInputs(uint numInputs)
        {
            if(numInputs != Weights.Length - 1)
            {
                InputCount = numInputs;
                Weights = new float[numInputs + 1];
                OldWeights = new float[numInputs + 1];
            }
        }
    }
}
