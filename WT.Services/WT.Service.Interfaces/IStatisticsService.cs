using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WT.Models;

namespace WT.Services
{
    public interface IStatisticsService
    {
        /// <summary>
        /// Return the List of statistic data 
        /// </summary>
        /// <param name="metrics"></param>
        /// <param name="stats"></param>
        /// <param name="fromDateTime"></param>
        /// <param name="toDateTime"></param>
        /// <returns></returns>
        IList<Statistic> CalculateStaticts(IList<string> metrics, IList<string> stats, string fromDateTime, string toDateTime);
    }
}
