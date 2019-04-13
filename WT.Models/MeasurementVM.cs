using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WT.Models
{
    public class MeasurementVM
    {
        public string TimeStampStr { get; set; }

        public IList<MetricData> MeasurementValues { get; set; }
       
    }
}
