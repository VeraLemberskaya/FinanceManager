using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;
using FinanceManager.Core;
using FinanceManager.Model;
using FinanceManager.Services;

namespace FinanceManager.ViewModel
{
    class AccountsViewModel:BaseVM
    {
        Service service = Service.GetInstance();
        public RelayCommand DeleteAccount { set; get; }
        public RelayCommand AddAccount { set; get; }
        public RelayCommand EditAccount { set; get; }

        public ObservableCollection<AccountViewModel> Accounts
        {
            get
            {
                return new ObservableCollection<AccountViewModel>(ServiceConverter.ConvertToAccountVM(service.Accounts));
            }
        }

        private AccountViewModel _selectedAccount;
        private object _currentVM;
        public AccountsViewModel()
        {
            #region Commands
            DeleteAccount = new RelayCommand(obj =>
            {
                AccountViewModel newAccount = obj as AccountViewModel;
                if (newAccount != null)
                {
                    Service service = Service.GetInstance();
                    service.DeleteAccount(newAccount.Account);
                    OnPropertyChanged(nameof(Accounts));
                    OnPropertyChanged(nameof(TotalBalance));
                }
                CurrentVM = null;
            });

            AddAccount = new RelayCommand(obj =>
             {
                 Service service = Service.GetInstance();
                 AccountViewModel _newAccount = new AccountViewModel(new Account(service.DefaultCurrency),true);
                 CurrentVM = _newAccount;
                 _newAccount.SaveObject += (object sender, SaveObjectChangesEventArgs e) =>
                   {
                       AccountViewModel newAccount = e.Object as AccountViewModel;
                       service.AddAccount(newAccount.Account);
                       OnPropertyChanged(nameof(Accounts));
                       OnPropertyChanged(nameof(TotalBalance));
                       CurrentVM = null;
                   };
             });

             EditAccount = new RelayCommand(obj =>
            {
                CurrentVM = null;
                if (SelectedAccount!=null)
                {
                     CurrentVM = new AccountViewModel(new Account(SelectedAccount.Name, SelectedAccount.Balance, SelectedAccount.Currency, SelectedAccount.Image, SelectedAccount.TakeIntoBalance),false);
                    (CurrentVM as AccountViewModel).SaveObject += (object sender, SaveObjectChangesEventArgs e) =>
                      {
                          Account newAccount = (e.Object as AccountViewModel).Account;
                          Service service = Service.GetInstance();
                          service.EditAccount(SelectedAccount.Account, newAccount);
                          OnPropertyChanged(nameof(Accounts));
                          CurrentVM = null;
                          OnPropertyChanged(nameof(TotalBalance));
                      };
                }
            });
            #endregion
        }
        #region Properties
        public object CurrentVM
        {
            get => _currentVM;
            set
            {
                _currentVM = value;
                OnPropertyChanged();
            }
        }
        public AccountViewModel SelectedAccount
        {
            get => _selectedAccount;
            set
            {
                _selectedAccount = value;
                OnPropertyChanged();
            }
        }

        public float TotalBalance
        {
            get
            {
                float balance = 0;
                foreach (var account in Accounts)
                {
                    if (account.TakeIntoBalance)
                        if (account.Currency.Equals(DefaultCurrency))
                            balance += account.Balance;
                }
                return balance;
            }
        }

        public Currency DefaultCurrency
        {
            get
            {
                return service.DefaultCurrency;
            }
        }
        #endregion
        public void UpDate()
        {
            CurrentVM = null;
        }
    }
}
