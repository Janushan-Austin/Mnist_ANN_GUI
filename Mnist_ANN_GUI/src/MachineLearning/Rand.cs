using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public static class Rand
    {
        private static Random Instance = new Random((int)System.DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
        
        public static int Next()
        {
            return Instance.Next();
        }

        public static int Next(int maxValue)
        {
            return Instance.Next(maxValue);
        }

        public static int Next(int minValue, int maxValue)
        {
            return Instance.Next(minValue, maxValue);
        }

        public static double NextDouble()
        {
            return Instance.NextDouble();
        }

        public static float NextFloat()
        {
            return (float)NextDouble();
        }
    }
