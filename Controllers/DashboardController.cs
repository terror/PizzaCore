using Microsoft.AspNetCore.Mvc;
using PizzaCore.Data;
using PizzaCore.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PizzaCore.Controllers {
  public class DashboardController : Controller {
    private readonly IPizzaCoreRepository repository;

    public DashboardController(IPizzaCoreRepository repository) {
      this.repository = repository;
    }

    public IActionResult Index() {
      return View(GetChartData());
    }

    [HttpPost]
    public ActionResult Index(string timeUnit) {
      return View(GetChartData(timeUnit));
    }

    private DashboardModel GetChartData(string timeUnit = "daily") {
      if (repository.GetAllOrders() == null) {
        timeUnit = string.Empty;
      }

      switch (timeUnit) {
        case "daily":
          return GetDailyChartData();
        case "weekly":
          return GetWeeklyChartData();
        case "monthly":
          return GetMonthlyChartData();
        case "yearly":
          return GetYearlyChartData();
        default:
          return GetEmptyChartData();
      }
    }

    private DashboardModel GetDailyChartData() {
      var ordersByDate = repository.GetAllOrders().GroupBy(o => o.Date.Date);
      Dictionary<string, int> productOrderFrequency = repository.GetDailyProductOrderFrequency();
      List<double> dailySales = new List<double>();
      List<int> dailyOrders = new List<int>();

      foreach (var date in ordersByDate) {
        dailySales.Add(date.Sum(o => o.SubTotal));
        dailyOrders.Add(date.Count());
      }

      return new DashboardModel {
        TimeFrame = "Daily",
        TimeUnits = JsonSerializer.Serialize(ordersByDate.Select(o => o.Key.ToString("MM-dd")).ToList()),
        ProductNames = JsonSerializer.Serialize(productOrderFrequency.Select(p => p.Key)),
        ProductOrderFrequencies = string.Join(",", productOrderFrequency.Select(p => p.Value)),
        TimeFrameSales = repository.GetTotalDailySales().ToString("C2"),
        AverageTimeFrameSales = (dailySales.Sum() / dailySales.Count).ToString("C2"),
        SalesPerTimeUnit = string.Join(",", dailySales),
        TimeFrameOrders = repository.GetTotalDailyOrders().ToString(),
        AverageTimeFrameOrders = Math.Round(Convert.ToDouble(dailyOrders.Sum()) / dailyOrders.Count, 2).ToString(),
        OrdersPerTimeUnit = string.Join(",", dailyOrders)
      };
    }

    private DashboardModel GetWeeklyChartData() {
      CultureInfo cultureInfo = new CultureInfo("en-US");
      CalendarWeekRule rule = cultureInfo.DateTimeFormat.CalendarWeekRule;
      DayOfWeek firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;

      var ordersByWeek = repository.GetAllOrders().GroupBy(o => cultureInfo.Calendar.GetWeekOfYear(o.Date, rule, firstDayOfWeek)).ToList();
      Dictionary<string, int> productOrderFrequency = repository.GetWeeklyProductOrderFrequency();
      List<double> weeklySales = new List<double>();
      List<int> weeklyOrders = new List<int>();

      foreach (var week in ordersByWeek) {
        weeklySales.Add(week.Sum(o => o.SubTotal));
        weeklyOrders.Add(week.Count());
      }

      return new DashboardModel {
        TimeFrame = "Weekly",
        TimeUnits = JsonSerializer.Serialize(ordersByWeek.Select(o => $"Week {o.Key}").ToList()),
        ProductNames = JsonSerializer.Serialize(productOrderFrequency.Select(p => p.Key)),
        ProductOrderFrequencies = string.Join(",", productOrderFrequency.Select(p => p.Value)),
        TimeFrameSales = repository.GetTotalWeeklySales().ToString("C2"),
        AverageTimeFrameSales = (weeklySales.Sum() / weeklySales.Count).ToString("C2"),
        SalesPerTimeUnit = string.Join(",", weeklySales),
        TimeFrameOrders = repository.GetTotalWeeklyOrders().ToString(),
        AverageTimeFrameOrders = Math.Round(Convert.ToDouble(weeklyOrders.Sum()) / weeklyOrders.Count, 2).ToString(),
        OrdersPerTimeUnit = string.Join(",", weeklyOrders)
      };
    }

    private DashboardModel GetMonthlyChartData() {
      var ordersByMonth = repository.GetAllOrders().GroupBy(o => o.Date.ToString("MM-yy")).ToList();
      Dictionary<string, int> productOrderFrequency = repository.GetMonthlyProductOrderFrequency();
      List<double> monthlySales = new List<double>();
      List<int> monthlyOrders = new List<int>();

      foreach (var month in ordersByMonth) {
        monthlySales.Add(month.Sum(o => o.SubTotal));
        monthlyOrders.Add(month.Count());
      }

      return new DashboardModel {
        TimeFrame = "Monthly",
        TimeUnits = JsonSerializer.Serialize(ordersByMonth.Select(o => o.Key).ToList()),
        ProductNames = JsonSerializer.Serialize(productOrderFrequency.Select(p => p.Key)),
        ProductOrderFrequencies = string.Join(",", productOrderFrequency.Select(p => p.Value)),
        TimeFrameSales = repository.GetTotalMonthlySales().ToString("C2"),
        AverageTimeFrameSales = (monthlySales.Sum() / monthlySales.Count).ToString("C2"),
        SalesPerTimeUnit = string.Join(",", monthlySales),
        TimeFrameOrders = repository.GetTotalMonthlyOrders().ToString(),
        AverageTimeFrameOrders = Math.Round(Convert.ToDouble(monthlyOrders.Sum()) / monthlyOrders.Count, 2).ToString(),
        OrdersPerTimeUnit = string.Join(",", monthlyOrders)
      };
    }

    private DashboardModel GetYearlyChartData() {
      var ordersByYear = repository.GetAllOrders().GroupBy(o => o.Date.Year).ToList();
      Dictionary<string, int> productOrderFrequency = repository.GetYearlyProductOrderFrequency();
      List<double> yearlySales = new List<double>();
      List<int> yearlyOrders = new List<int>();

      foreach (var year in ordersByYear) {
        yearlySales.Add(year.Sum(o => o.SubTotal));
        yearlyOrders.Add(year.Count());
      }

      return new DashboardModel {
        TimeFrame = "Yearly",
        TimeUnits = JsonSerializer.Serialize(ordersByYear.Select(o => o.Key).ToList()),
        ProductNames = JsonSerializer.Serialize(productOrderFrequency.Select(p => p.Key)),
        ProductOrderFrequencies = string.Join(",", productOrderFrequency.Select(p => p.Value)),
        TimeFrameSales = repository.GetTotalYearlySales().ToString("C2"),
        AverageTimeFrameSales = (yearlySales.Sum() / yearlySales.Count).ToString("C2"),
        SalesPerTimeUnit = string.Join(",", yearlySales),
        TimeFrameOrders = repository.GetTotalYearlyOrders().ToString(),
        AverageTimeFrameOrders = Math.Round(Convert.ToDouble(yearlyOrders.Sum()) / yearlyOrders.Count, 2).ToString(),
        OrdersPerTimeUnit = string.Join(",", yearlyOrders)
      };
    }

    private DashboardModel GetEmptyChartData() {
      return new DashboardModel {
        TimeFrame = "",
        TimeFrameSales = "No sales have been made",
        AverageTimeFrameSales = "0",
        TimeFrameOrders = "No orders have been made",
        AverageTimeFrameOrders = "o"
      };
    }
  }
}
