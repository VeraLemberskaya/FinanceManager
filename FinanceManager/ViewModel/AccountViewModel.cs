using System;
using System.Collections.Generic;
using FinanceManager.Core;
using FinanceManager.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using FinanceManager.Services;

namespace FinanceManager.ViewModel
{
    class AccountViewModel :BaseVM, IDataErrorInfo,ISaveObjectChanges
    {
        Service service = Service.GetInstance();
        public RelayCommand SaveAccount { get; set; }
        public static ObservableCollection<string> Icons { get; set; } = new ObservableCollection<string>()
        {
             "Cash","Wallet","Bank","PiggyBankOutline","CreditCardOutline","CurrencyEur","WalletGiftcard","CurrencyUsd","CurrencyGbp"
        };
        public List<Currency> CurrencyCollection { get; set; }
        private Account _account;

        public event ISaveObjectChanges.SaveObjectChangesHandler SaveObject;

        private AccountViewModel() 
        {
            CurrencyCollection = service.Currency;
            
            SaveAccount = new RelayCommand(o =>
              {
                  SaveObjectExecute(this);
              });
        }
        public AccountViewModel(Account account):this()
        {
            _account = account;
        }
        public AccountViewModel(Account account, bool CurrencyIsEditable) : this(account)
        {
            this.CurrencyIsEditable = CurrencyIsEditable;
        }
        #region Properties

        public bool CurrencyIsEditable
        {
            get;
            private set;
        }

        public bool IsValid
        {
            get => _account.IsValid;
        }
        public string Name
        {
            get => _account.Name;
            set
            {
                _account.Name = value;
                OnPropertyChanged(nameof(IsValid));
                OnPropertyChanged();
            }
        }
        public float Balance
        {
            get => _account.Balance;

            set
            {
                _account.Balance = value;
                OnPropertyChanged(nameof(IsValid));
                OnPropertyChanged();        
            }
        }
        public string Image
        {
            get => _account.Image;
            set
            {
                _account.Image = value;
                OnPropertyChanged();
            }
        }
        public Currency Currency
        {
            get => _account.Currency;
            set
            {
                _account.Currency = value;
                OnPropertyChanged();
            }
        }
        public bool TakeIntoBalance
        {
            get => _account.TakeIntoBalance;
            set
            {
                _account.TakeIntoBalance = value;
                OnPropertyChanged();
            }
        }
        public Account Account
        {
            get => _account;
        }
        #endregion

        #region DataValidation
        public string this[string propertyName]
        {
            get
            {
                string error = string.Empty;
                switch (propertyName)
                {
                    case "Balance":
                        _account.Validate("BalanceIsNotNegative", ref error);
                        _account.Validate("BalanceIsValid", ref error);
                        break;
                    case "Name":
                        _account.Validate("NameIsNotNull", ref error);
                        _account.Validate("NameValidLength", ref error);
                        break;
                }
                return error;
            }
        }
        public string Error
        {
            get { throw new NotImplementedException(); }
        }
        #endregion
        public void SaveObjectExecute(object obj)
        {
            SaveObject?.Invoke(this, new SaveObjectChangesEventArgs(obj));
        }
    }
}
