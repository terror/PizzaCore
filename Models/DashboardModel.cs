using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaCore.Models {
  public class DashboardModel {
    public string TimeFrame { get; set; }
    public string TimeUnits { get; set; }
    public string ProductNames { get; set; }
    public string ProductOrderFrequencies { get; set; }
    public string TimeFrameSales { get; set; }
    public string AverageTimeFrameSales { get; set; }
    public string SalesPerTimeUnit { get; set; }
    public string TimeFrameOrders { get; set; }
    public string AverageTimeFrameOrders { get; set; }
    public string OrdersPerTimeUnit { get; set; }
  }
}
