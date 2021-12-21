using FinanceManager.Core;

namespace FinanceManager.ViewModel
{
    class MainViewModel : BaseVM
    {
        public RelayCommand IncomeViewCommand { get; set; }
        public RelayCommand ExpensesViewCommand { get; set; }
        public RelayCommand AccountsViewCommand { get; set; }
        public RelayCommand CategoriesViewCommand { get; set; }
        public RelayCommand CurrencyViewCommand { get; set; }
        private BaseTransactionsViewModel IncomeTransactionsVM{get;set;}
        private BaseTransactionsViewModel ExpensesTransactionsVM { get; set; }
        private AccountsViewModel AccountsVM { get; set; }
        private CategoriesViewModel CategoriesVM { get; set; }
        private CurrencyViewModel CurrencyVM { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set 
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        public MainViewModel()
        {
            IncomeTransactionsVM = new BaseTransactionsViewModel(OperationType.Income);
            ExpensesTransactionsVM = new BaseTransactionsViewModel(OperationType.Expenses);

            AccountsVM = new AccountsViewModel();
            CategoriesVM = new CategoriesViewModel();
            CurrencyVM = new CurrencyViewModel();

            CurrentView = IncomeTransactionsVM;
            IncomeViewCommand = new RelayCommand(o =>
              {
                  IncomeTransactionsVM.UpDate();
                  CurrentView = IncomeTransactionsVM;
              });
            ExpensesViewCommand = new RelayCommand(o =>
            {
                ExpensesTransactionsVM.UpDate();
                CurrentView = ExpensesTransactionsVM;
            });
            AccountsViewCommand = new RelayCommand(o =>
             {
                 AccountsVM.UpDate();
                 CurrentView = AccountsVM;
             });
            CategoriesViewCommand = new RelayCommand(o =>
              {
                  CategoriesVM.UpDate();
                  CurrentView = CategoriesVM;
              });
            CurrencyViewCommand = new RelayCommand(o =>
              {
                  CurrentView = CurrencyVM;
              });
        }
    }
}
