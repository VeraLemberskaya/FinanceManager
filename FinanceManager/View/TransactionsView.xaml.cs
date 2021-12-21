using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;

namespace FinanceManager.View
{
    /// <summary>
    /// Логика взаимодействия для TransactionsView.xaml
    /// </summary>
    public partial class TransactionsView : UserControl
    {
        public TransactionsView()
        {
            InitializeComponent();
        }
        //private void Chart_OnDataClick(object sender, ChartPoint chartpoint)
        //{
        //    var chart = (LiveCharts.Wpf.PieChart)chartpoint.ChartView;
        //    foreach (PieSeries series in chart.Series)
        //        series.PushOut = 0;

        //    var selectedSeries = (PieSeries)chartpoint.SeriesView;
        //    selectedSeries.PushOut = 8;
        //}
    }
}
