using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WT.Services
{
    public class Measurements
    {
        public DateTime timeStamp { get; set; }
        
        public decimal temperature { get; set; }
        public decimal dewPoint { get; set; }
        public decimal precipitation { get; set; }
    }
}
