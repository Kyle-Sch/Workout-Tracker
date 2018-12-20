using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models
{
    public class VisitMetricsModel
    {
        public TimeSpan averageVisitTimeSpan { get; set; }
        public List<Visit> Visits { get; set; }

    }
}
