using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using FinanceManager.Core;
using FinanceManager.Model;
using FinanceManager.Services;

namespace FinanceManager.ViewModel
{
    class TransactionViewModel:BaseVM, IDataErrorInfo,ISaveObjectChanges
    {
        public RelayCommand SaveTransaction { get; set; }
        public RelayCommand DeleteTransaction { get; set; }
        private Transaction _transaction;
        public ObservableCollection<Account> Accounts { get; set; }
        public ObservableCollection<Category> Categories { get; set; }

        public event ISaveObjectChanges.SaveObjectChangesHandler SaveObject;
        public event ISaveObjectChanges.SaveObjectChangesHandler DeleteObject;
        public TransactionViewModel(OperationType Type,Currency currency)
        {
            Service service = Service.GetInstance();
            Accounts = new ObservableCollection<Account>(service.GetAccounts(currency));
            if (Type == OperationType.Income) Categories = new ObservableCollection<Category>(service.IncomeCategories);
            else Categories = new ObservableCollection<Category>(service.ExpensesCategories);

            _transaction = new Transaction(Accounts.First(), Type);

            #region Commands
            SaveTransaction = new RelayCommand(obj =>
            {
                SaveObjectExecute(this);
            });
            DeleteTransaction = new RelayCommand(obj =>
            {
                DeleteObject?.Invoke(this, new SaveObjectChangesEventArgs(this));
            });
            #endregion
        }

        public TransactionViewModel(Transaction transaction):this(transaction.Type, transaction.Currency)
        {
            this._transaction = transaction;  
        }
        #region Properties
        public Transaction Transaction
        {
            get => _transaction;
        }
        public bool IsValid
        {
            get => _transaction.IsValid;
        }
        public Currency Currency
        {
            get => _transaction.Currency;
        }
        public Category Category
        {
            get => _transaction.Category;
            set
            {
                _transaction.Category = value;
                OnPropertyChanged(nameof(IsValid));
                OnPropertyChanged();
            }
        }
        public DateTime Date
        {
            get => _transaction.Date;
            set
            {
                _transaction.Date = value;
                OnPropertyChanged(nameof(IsValid));
                OnPropertyChanged();
            }
        }
        public string Comment
        {
            get => _transaction.Comment;
            set
            {
                _transaction.Comment = value;
                OnPropertyChanged();
            }
        }
        public Account Account
        {
            get => _transaction.Account;
            set
            {
                _transaction.Account = value;
                OnPropertyChanged(nameof(Money));
                OnPropertyChanged();
            }
        }
        public float Money
        {
            get => _transaction.Money;
            set
            {
                _transaction.Money = value;
                OnPropertyChanged(nameof(IsValid));
                OnPropertyChanged();
            }
        }
        public OperationType Type
        {
            get => _transaction.Type;
            set
            {
                _transaction.Type = value;
                OnPropertyChanged();
            }
        }
        public bool PossibleToDelete
        {
            get
            {
                if (DeleteObject == null) return false;
                else return true;
            }
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
                    case "Money":
                        _transaction.Validate("SumIsNotNegative", ref error);
                        _transaction.Validate("SumIsValid", ref error);
                        _transaction.Validate("SumIsNotNull", ref error);
                        break;
                    case "Date":
                        _transaction.Validate("DateIsValid", ref error);
                        break;
                    case "Category":
                        _transaction.Validate("CategoryIsNotNull", ref error);
                        break;
                }
                return error;
            }
        }
        public string Error => throw new NotImplementedException();
        #endregion
        public void SaveObjectExecute(object obj)
        {
            SaveObject?.Invoke(this, new SaveObjectChangesEventArgs(obj as TransactionViewModel));
        }
    }
}
