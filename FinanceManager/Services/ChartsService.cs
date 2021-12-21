using System;
using System.Collections.Generic;
using LiveCharts.Wpf;
using LiveCharts.Defaults;
using LiveCharts;
using FinanceManager.Model;

namespace FinanceManager.Services
{
    class ChartsService
    {
        public static SeriesCollection CreateSeriesCollection(Dictionary<Category,float> categories, Currency currency)
        {
            SeriesCollection collection = new SeriesCollection();
            foreach(var category in categories)
            {
                collection.Add(new PieSeries
                {
                    Title = category.Key.Name,
                    Values = new ChartValues<ObservableValue> { new ObservableValue(category.Value) },
                    LabelPoint=chartPoint=>String.Format("{0} {1}", category.Value, currency),
                    DataLabels = true
                }) ;
            }
            return collection;
        }
    }
}
