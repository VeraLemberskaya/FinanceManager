using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MaterialDesignThemes;
using FinanceManager.ViewModel;

namespace FinanceManager.View
{
    /// <summary>
    /// Логика взаимодействия для CurrencyView.xaml
    /// </summary>
    public partial class CurrencyView : UserControl
    {
        public CurrencyView()
        {
            InitializeComponent();
            this.ListCurrency.SelectedItem = null;
        }

        private void ListCurrency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsInitialized)
            {
                if (this.ListCurrency.SelectedItem != null)
                {
                    this.Dia.IsOpen = true;
                }
            }
        }
    }
}
