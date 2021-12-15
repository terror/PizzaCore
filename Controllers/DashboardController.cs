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
      GetDailyChartData();
      return View();
    }

    [HttpPost]
    public ActionResult Index(string timeUnit)
    {
      switch(timeUnit)
      {
        case "daily":
          GetDailyChartData();
          break;
        case "weekly":
          GetWeeklyChartData();
          break;
        case "monthly":
          GetMonthlyChartData();
          break;
        case "yearly":
          GetYearlyChartData();
          break;
      }

      return View();
    }

    private void GetDailyChartData() {
      var ordersByDate = repository.GetAllOrders().GroupBy(o => o.Date.Date);
      Dictionary<string, int> productOrderFrequency = repository.GetDailyProductOrderFrequency();
      List<double> dailySales = new List<double>();
      List<int> dailyOrders = new List<int>();

      foreach (var date in ordersByDate) {
        dailySales.Add(date.Sum(o => o.SubTotal));
        dailyOrders.Add(date.Count());
      }

      ViewBag.timeFrame = "Daily";
      ViewBag.timeUnits = JsonSerializer.Serialize(ordersByDate.Select(o => o.Key.ToString("MM-dd")).ToList());
      ViewBag.productNames = JsonSerializer.Serialize(productOrderFrequency.Select(p => p.Key));
      ViewBag.productOrderFrequencies = string.Join(",", productOrderFrequency.Select(p => p.Value));
      ViewBag.timeFrameSales = repository.GetTotalDailySales().ToString("C2");
      ViewBag.averagetimeFrameSales = (dailySales.Sum() / dailySales.Count).ToString("C2");
      ViewBag.salesPerTimeUnit = string.Join(",", dailySales);
      ViewBag.timeFrameOrders = repository.GetTotalDailyOrders();
      ViewBag.averageTimeFrameOrders = Math.Round(Convert.ToDouble(dailyOrders.Sum())/dailyOrders.Count, 2);
      ViewBag.ordersPerTimeUnit = string.Join(",", dailyOrders);
    }

    private void GetWeeklyChartData() {
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

      ViewBag.timeFrame = "Weekly";
      ViewBag.timeUnits = JsonSerializer.Serialize(ordersByWeek.Select(o => $"Week {o.Key}").ToList());
      ViewBag.productNames = JsonSerializer.Serialize(productOrderFrequency.Select(p => p.Key));
      ViewBag.productOrderFrequencies = string.Join(",", productOrderFrequency.Select(p => p.Value));
      ViewBag.timeFrameSales = repository.GetTotalWeeklySales().ToString("C2");
      ViewBag.averageTimeFrameSales = (weeklySales.Sum() / weeklySales.Count).ToString("C2");
      ViewBag.salesPerTimeUnit = string.Join(",", weeklySales);
      ViewBag.timeFrameOrders = repository.GetTotalWeeklyOrders();
      ViewBag.averageTimeFrameOrders = Math.Round(Convert.ToDouble(weeklyOrders.Sum()) / weeklyOrders.Count, 2);
      ViewBag.ordersPerTimeUnit = string.Join(",", weeklyOrders);
    }

    private void GetMonthlyChartData() {
      var ordersByMonth = repository.GetAllOrders().GroupBy(o => o.Date.ToString("MM-yy")).ToList();
      Dictionary<string, int> productOrderFrequency = repository.GetMonthlyProductOrderFrequency();
      List<double> monthlySales = new List<double>();
      List<int> monthlyOrders = new List<int>();

      foreach (var month in ordersByMonth) {
        monthlySales.Add(month.Sum(o => o.SubTotal));
        monthlyOrders.Add(month.Count());
      }

      ViewBag.timeFrame = "Monthly";
      ViewBag.timeUnits = JsonSerializer.Serialize(ordersByMonth.Select(o => o.Key).ToList());
      ViewBag.productNames = JsonSerializer.Serialize(productOrderFrequency.Select(p => p.Key));
      ViewBag.productOrderFrequencies = string.Join(",", productOrderFrequency.Select(p => p.Value));
      ViewBag.timeFrameSales = repository.GetTotalMonthlySales().ToString("C2");
      ViewBag.averageTimeFrameSales = (monthlySales.Sum() / monthlySales.Count).ToString("C2");
      ViewBag.salesPerTimeUnit = string.Join(",", monthlySales);
      ViewBag.timeFrameOrders = repository.GetTotalMonthlyOrders();
      ViewBag.averageTimeFrameOrders = Math.Round(Convert.ToDouble(monthlyOrders.Sum()) / monthlyOrders.Count, 2);
      ViewBag.ordersPerTimeUnit = string.Join(",", monthlyOrders);
    }

    private void GetYearlyChartData() {
      var ordersByYear = repository.GetAllOrders().GroupBy(o => o.Date.Year).ToList();
      Dictionary<string, int> productOrderFrequency = repository.GetYearlyProductOrderFrequency();
      List<double> yearlySales = new List<double>();
      List<int> yearlyOrders = new List<int>();

      foreach (var year in ordersByYear) {
        yearlySales.Add(year.Sum(o => o.SubTotal));
        yearlyOrders.Add(year.Count());
      }

      ViewBag.timeFrame = "Yearly";
      ViewBag.timeUnits = JsonSerializer.Serialize(ordersByYear.Select(o => o.Key).ToList());
      ViewBag.productNames = JsonSerializer.Serialize(productOrderFrequency.Select(p => p.Key));
      ViewBag.productOrderFrequencies = string.Join(",", productOrderFrequency.Select(p => p.Value));
      ViewBag.timeFrameSales = repository.GetTotalYearlySales().ToString("C2");
      ViewBag.averageTimeFrameSales = (yearlySales.Sum() / yearlySales.Count).ToString("C2");
      ViewBag.salesPerTimeUnit = string.Join(",", yearlySales);
      ViewBag.timeFrameOrders = repository.GetTotalYearlyOrders();
      ViewBag.averageTimeFrameOrders = Math.Round(Convert.ToDouble(yearlyOrders.Sum()) / yearlyOrders.Count, 2);
      ViewBag.ordersPerTimeUnit = string.Join(",", yearlyOrders);
    }
  }
}
