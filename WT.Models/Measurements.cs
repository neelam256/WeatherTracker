using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WT.Models
{
    public class Measurement
    {
        
        public IList<MetricData> MeasurementValues { get; set; }

        public DateTime TimeStamp { get; set; }
       
       
    }
}
