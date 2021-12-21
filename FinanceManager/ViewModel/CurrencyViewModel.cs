using System;
using System.Collections.ObjectModel;
using FinanceManager.Model;
using FinanceManager.Core;
using System.Windows.Data;
using FinanceManager.Services;

namespace FinanceManager.ViewModel
{
    class CurrencyViewModel:BaseVM
    {
        Service service = Service.GetInstance();
        public ObservableCollection<Currency> Currency { get; set; }
        public CollectionViewSource FilteredCurrency { get; set; }
        public RelayCommand SaveCurrency { get; set; }

        private Currency _selectedCurrency;

        private string _searchFilter;
        private Currency _defaultCurrency;

        #region Properties
        public Currency SelectedCurrency
        {
            get => _selectedCurrency;
            set
            {
                _selectedCurrency = value;
                OnPropertyChanged();
            }
        }
        public string SearchFilter
        {
            get => _searchFilter;
            set
            {
                    _searchFilter = value;
                    SelectedCurrency = null;
                    AddFilter();
                    FilteredCurrency.View.Refresh();              
            }
        }

        public Currency DefaultCurrency
        {
            get => _defaultCurrency;
            set
            {
                _defaultCurrency = value;
                service.DefaultCurrency = value;
                OnPropertyChanged();
            }
        }
        #endregion
        public CurrencyViewModel()
        {
            _defaultCurrency = service.DefaultCurrency;
            Currency = new ObservableCollection<Currency>(service.Currency);
            FilteredCurrency = new CollectionViewSource();
            FilteredCurrency.Source = Currency;
            SaveCurrency = new RelayCommand(o =>
            {
                DefaultCurrency = SelectedCurrency;
            });
        }

        private void AddFilter()
        {
            FilteredCurrency.Filter += (object sender, FilterEventArgs e) =>
            {
                Currency search = e.Item as Currency;
                if (search != null)
                {
                    if (SearchFilter != null)
                    {
                        if (search.Name.Contains(SearchFilter, StringComparison.InvariantCultureIgnoreCase)) e.Accepted = true;
                        else e.Accepted = false;
                    }
                }
            };
        }
    }
}
