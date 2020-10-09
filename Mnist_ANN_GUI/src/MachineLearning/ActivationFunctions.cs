using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning
{
	public class ActivationFunctions
	{
		#region Types of Activation Functions Supported
		public enum FunctionTypes
		{
			Tanh = 0,
			Sigmoid,
			Double_Sigmoid,
			RELU,
		}
		#endregion

		public static readonly FunctionTypes DefaultFunctionType = Enum.GetValues(typeof(FunctionTypes)).Cast<FunctionTypes>().ToArray()[0];

		public static readonly uint ActivationFunctionCount = (uint)Enum.GetValues(typeof(FunctionTypes)).Length;

		private LearningRate[] LearningRates;

        public FunctionTypes Function { get; private set; }
		
		public float FunctionLearningRate { get { return LearningRates[(int)Function].learningRate; } set { LearningRates[(int)Function].learningRate = value;  } }
		
		public ActivationFunctions()
		{
			Initialize(DefaultFunctionType);
		}

        public ActivationFunctions(FunctionTypes type)
		{
			Initialize(type);
		}

		private void Initialize(FunctionTypes type)
        {
			Function = type;
			LearningRates = new LearningRate[ActivationFunctionCount];
			for(uint i=0; i < LearningRates.Length; i++)
            {
				LearningRates[i].learningRate = DefaultLearningRate((FunctionTypes)i);
            }
		}

		public bool ChangeFunction(int funcType)
		{
			bool validFunction = false;

			if (funcType >= 0 && funcType < ActivationFunctionCount)
			{
				Function = (FunctionTypes)funcType;
				validFunction = true;
			}

			return validFunction;
		}

		public bool ChangeFunction(FunctionTypes funcType)
        {
			Function = funcType;
			return true;
        }

		public static float DefaultLearningRate(FunctionTypes funcType)
        {
			switch(funcType)
            {
				case FunctionTypes.Tanh:
					return 0.32f;
				case FunctionTypes.Sigmoid:
					return .75f;
				case FunctionTypes.Double_Sigmoid:
					return 0.75f;
				case FunctionTypes.RELU:
					return 0.15f;
				default:
					return 0.1f;
            }
        }

		#region Implementation of Activation Functions
		public float evaluate(float x, bool derivative = false, bool xIsOutput = false)
		{
			return evaluate(Function, x, derivative, xIsOutput);
		}

		public static float evaluate(FunctionTypes funcType, float x, bool derivative = false, bool xIsOutput = false)
		{
			switch (funcType)
			{
				case FunctionTypes.Tanh:
					return tanh(x, derivative, xIsOutput);
				case FunctionTypes.Sigmoid:
					return sigmoid(x, derivative, xIsOutput);
				case FunctionTypes.RELU:
					return RELU(x, derivative, xIsOutput);
				case FunctionTypes.Double_Sigmoid:
					return DoubleSigmoid(x, derivative);
				default:
					return 0.0f;
			}
		}

		public float evaluateDerivative(float x, bool xIsOutput = false)
		{
			return evaluateDerivative(Function, x, xIsOutput);
		}

		public static float evaluateDerivative(FunctionTypes funcType, float x, bool xIsOutput = false)
		{
			switch (funcType)
			{
				case FunctionTypes.Tanh:
					return tanh(x, true, xIsOutput);
				case FunctionTypes.Sigmoid:
					return sigmoid(x, true, xIsOutput);
				case FunctionTypes.RELU:
					return RELU(x, true, xIsOutput);
				case FunctionTypes.Double_Sigmoid:
					return DoubleSigmoid(x, true);
				default:
					return 0.0f;
			}
		}

		static float tanh(float x, bool derivative = false, bool xIsOutput = false)
		{
			float output = 0;
			if (derivative == true)
			{
				if (xIsOutput == false)
				{
					float tanhX = tanh(x, false);
					output =  1 - tanhX * tanhX;
				}
				else
                {
					output = 1 - x * x;
                }
			}
			else
			{
				float eX = (float)Math.Pow(Math.E, x);
				float eNegX = 1 / eX;

				output = (eX - eNegX) / (eX + eNegX);
			}

			return output;
		}

		static float sigmoid(float x, bool derivative = false, bool xIsOutput = false)
		{
			float output = 0;
			if (derivative == true)
			{
				if (xIsOutput == false)
				{
					float sigmoidX = sigmoid(x, false);
					output =  sigmoidX * (1 - sigmoidX);
				}
				else
                {
					output = x * (1 - x);
                }
			}
			else
			{
				output = 1 / (1 + (float)Math.Pow(Math.E, -x));
			}

			return output;
		}

		//TODO: derive derivative in terms of output of double sigmoid
		static float DoubleSigmoid(float x, bool derivative = false)
		{
			if (derivative == true)
			{
				return 2 * sigmoid(x, true);
			}
			else
			{
				return 2 * sigmoid(x, false) - 1;
			}
		}

		static float RELU(float x, bool derivative = false, bool xIsOutput = false)
		{
			if (derivative == true)
			{
				if (x > 0)
				{
					return 1;
				}
				else
				{
					//return 0;
					return 0.001f;
				}
			}
			else
			{
				if (x > 0)
				{
					return x;
				}
				else
				{
					return 0;
				}
			}
		}

        #endregion

        #region ToString and ToList functions
        public override string ToString()
		{
			return ToString(Function);
		}

		public static string ToString(FunctionTypes type)
		{
			switch (type)
			{
				case FunctionTypes.Tanh:
					return "Tanh";
				case FunctionTypes.Sigmoid:
					return "Sigmoid";
				case FunctionTypes.RELU:
					return "RELU";
				case FunctionTypes.Double_Sigmoid:
					return "Double Sigmoid";
				default:
					return "INVALID ACTIVATION FUNCTION TYPE";
			}
		}

		public static string[] ToOrderedList()
		{
			FunctionTypes[] functionTypes = Enum.GetValues(typeof(FunctionTypes)).Cast<FunctionTypes>().ToArray();
			string[] list = new string[functionTypes.Length];

			int index = 0;
			foreach (FunctionTypes functionType in functionTypes)
			{
				list[index] = $"{ToString(functionType)}";
				index++;
			}

			return list;
		}

        #endregion
    }
}
