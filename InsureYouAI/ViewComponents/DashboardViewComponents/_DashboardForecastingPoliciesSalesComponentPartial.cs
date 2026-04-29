using InsureYouAI.Context;
using InsureYouAI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;

namespace InsureYouAI.ViewComponents.DashboardViewComponents
{
    public class _DashboardForecastingPoliciesSalesComponentPartial : ViewComponent
    {
        private readonly InsureContext _context;
        public _DashboardForecastingPoliciesSalesComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {

            // 1) Veri Hazırlığı – Son 6 Ay
            var rawData = _context.Policies
                .Where(x => x.CreatedDate >= new DateTime(2025, 5, 1) &&
                            x.CreatedDate <= new DateTime(2025, 11, 30))
                .GroupBy(x => x.PolicyType)
                .Select(g => new
                {
                    PolicyType = g.Key,
                    MonthlyCounts = g
                        .GroupBy(z => new { z.CreatedDate.Year, z.CreatedDate.Month })
                        .Select(s => new { Month = s.Key.Month, Count = s.Count() })
                        .OrderBy(s => s.Month)
                        .ToList()
                })
                .ToList();

            // 2) ML.NET Tahmin İçin Setup
            var ml = new MLContext();

            List<PolicyForecastViewModel> result = new();

            foreach (var item in rawData)
            {
                // ML.NET için input formatı
                var mlData = item.MonthlyCounts.Select(m => new PolicyMonthlyData
                {
                    MonthIndex = m.Month,
                    Value = m.Count
                });

                var dataView = ml.Data.LoadFromEnumerable(mlData);

                var pipeline = ml.Forecasting.ForecastBySsa(
                    outputColumnName: "Forecast",
                    inputColumnName: "Value",
                    windowSize: 2,
                    seriesLength: 6,
                    trainSize: 6,
                    horizon: 1);

                var model = pipeline.Fit(dataView);

                var forecastEngine = model.CreateTimeSeriesEngine<PolicyMonthlyData, PolicyForecastOutput>(ml);

                var prediction = forecastEngine.Predict();

                int predicted = (int)prediction.Forecast[0];

                result.Add(new PolicyForecastViewModel
                {
                    PolicyType = item.PolicyType,
                    ForecastCount = predicted
                });
            }

            // 3) Yüzde Hesabı
            int total = result.Sum(x => x.ForecastCount);

            foreach (var item in result)
                item.Percentage = total > 0 ? (item.ForecastCount * 100 / total) : 0;

            return View(result);
        }
    }

    public class PolicyMonthlyData
    {
        public float MonthIndex { get; set; }
        public float Value { get; set; }
    }

    public class PolicyForecastOutput
    {
        public float[] Forecast { get; set; }
    }
}
