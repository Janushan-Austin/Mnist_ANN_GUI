using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning
{
    public interface IDataSet
    {
        float[] InputSet { get; set; }
        float[] ExpectedOutputSet { get; set; }
    }
}
