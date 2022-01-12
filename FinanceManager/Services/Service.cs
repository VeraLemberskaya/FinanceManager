using System;
using System.Collections.Generic;
using System.Linq;
using FinanceManager.Core;
using FinanceManager.Model;
using FinanceManager.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Services
{
     class Service
    {
        private static Service instance;
        private static ApplicationContext _db { get; set; }

        #region Properties
        public List<Account> Accounts
        {
            get
            {
                return _db.Accounts.Include(acc=>acc.Currency).ToList();
            }
        }
        public List<Transaction> IncomeTransactions
        {
            get
            {
                List<Transaction> incomeTr = _db.Transactions.Include(tr=>tr.Account).Include(tr=>tr.Category).ToList().FindAll(t => t.Type == OperationType.Income);
                incomeTr.Sort((tr1, tr2) => { return tr1.Category.Name.CompareTo(tr2.Category.Name); });
                return incomeTr;
            }
        }
        public List<Transaction> ExpensesTransactions
        {
            get
            {
                List<Transaction> expensesTr = _db.Transactions.Include(tr => tr.Account).Include(tr => tr.Category).ToList().FindAll(t => t.Type == OperationType.Expenses);
                expensesTr.Sort((tr1, tr2) => { return tr1.Category.Name.CompareTo(tr2.Category.Name); });
                return expensesTr;
            }
        }
        public List<Currency> Currency
        {
            get
            {
                return _db.Currency.ToList();
            }
        }
        public List<Category> IncomeCategories
        {
            get
            {
                return _db.Categories.ToList().FindAll(c => c.Type == OperationType.Income);
            }
        }
        public List<Category> ExpensesCategories
        {
            get
            {
                return _db.Categories.ToList().FindAll(c => c.Type == OperationType.Expenses);
            }
        }
        #endregion


        private Currency _defaultCurrency;

        public Currency DefaultCurrency
        {
            get => _defaultCurrency;
            set
            {
                 Setting.Default.index = Currency.IndexOf(value);
                Setting.Default.Save();
                _defaultCurrency = value;
            }
        }

        private Service()
        {
            _db = new ApplicationContext();
            _db.Database.EnsureCreated();
            _defaultCurrency = Currency[Setting.Default.index];            
        }

        #region Getters
        public List<Transaction> GetTransactions(OperationType Type)
        {
            if(Type == OperationType.Income) return IncomeTransactions;
            return ExpensesTransactions;
        }
        public List<Category> GetCategories(OperationType Type)
        {
            if (Type == OperationType.Income) return IncomeCategories;
            return ExpensesCategories;
        }

        public static Dictionary<Category, float> GetTransactionsCategories(ICollection<TransactionViewModel> transactions)
        {
            Dictionary<Category, float> categories = new();
            foreach (var transaction in transactions)
            {
                if (!categories.ContainsKey(transaction.Category)) categories.Add(transaction.Category, transaction.Money);
                else categories[transaction.Category] += transaction.Money;
            }
            return categories;
        }
        public List<Account> GetAccounts(Currency currency)
        {
            List<Account> accCur = new List<Account>();
            foreach (var account in Accounts)
            {
                if (account.Currency.Equals(currency)) accCur.Add(account);
            }
            return accCur;
        }

        public float GetTotalBalance(Currency currency)
        {
            float balance = 0;
            foreach (var account in Accounts)
            {
                if (account.TakeIntoBalance == true)
                    if (account.Currency.Equals(currency)) balance += account.Balance;
            }
            return balance;
        }
        public List<Currency> GetAccountsCurrency()
        {
            List<Currency> AccountsCurrency = new List<Currency>();
            foreach (var account in Accounts)
            {
                if (!AccountsCurrency.Contains(account.Currency))
                {
                    AccountsCurrency.Add(account.Currency);
                }
            }
            return AccountsCurrency;
        }

        public Account GetAccount(Account account)
        {
            int index = Accounts.IndexOf(account);
            return Accounts.ElementAt(index);
        }

        public bool GetCategorySum(Category category)
        {
            float Sum = 0;
            List<Transaction> transactions = MonthTransactions(_db.Transactions.ToList(), DefaultCurrency);
            foreach(var transaction in transactions)
            {
                if (transaction.Category.Equals(category)) Sum += transaction.Money;
            }
            if (category.DefaultSum != 0)
            {
                if (category.Type == OperationType.Income)
                {
                    if (Sum < category.DefaultSum) return false;
                }
                if (category.Type == OperationType.Expenses)
                {
                    if (Sum > category.DefaultSum) return false;
                }
            }
            return true;
        }
        #endregion

        #region ChangeCollection
        #region Account
        public void AddAccount(Account account)
        {
            _db.Accounts.Add(account);
            _db.SaveChanges();
        }
        public void EditAccount(Account _prev, Account _new)
        {
            _prev.Balance = _new.Balance;
            _prev.Currency = _new.Currency;
            _prev.Image = _new.Image;
            _prev.Name = _new.Name;
            _prev.TakeIntoBalance = _new.TakeIntoBalance;
            _db.SaveChanges();
        }

        public void DeleteAccount(Account account)
        {
            foreach(var transaction in _db.Transactions)
            {
                if(transaction.Account.Equals(account))
                {
                    _db.Transactions.Remove(transaction);
                    _db.SaveChanges();
                }
            }
            _db.Accounts.Remove(account);
            _db.SaveChanges();
        }
        #endregion
        #region Category

        public void AddCategory(Category category)
        {
            _db.Categories.Add(category);
            _db.SaveChanges();
        }
        public void EditCategory(Category _prev, Category _new)
        {
            _prev.DefaultSum = _new.DefaultSum;
            _prev.Image = _new.Image;
            _prev.Name = _new.Name;
            _prev.Type = _new.Type;
            _db.SaveChanges();
        }
        #endregion
        #region Transaction
        public void AddTransaction(Transaction transaction)
        {
            _db.Transactions.Add(transaction);
            _db.SaveChanges();
        }
        public void EditTransaction(Transaction _prev, Transaction _new)
        {
            _prev.Account = _new.Account;
            _prev.Category = _new.Category;
            _prev.Comment = _new.Comment;
            _prev.Money = _new.Money;
            _prev.Date = _new.Date;
            _db.SaveChanges();
        }
        public void DeleteTransaction(Transaction transaction)
        {
            _db.Transactions.Remove(transaction);
            _db.SaveChanges();
        }
        #endregion
        #endregion

        #region PeriodTransactions
        public static List<Transaction> DayTransactions(List<Transaction> transactions, Currency currency)
        {
           return DayTransactions(transactions, currency, DateTime.Today);
        }
        public static List<Transaction> DayTransactions(List<Transaction> transactions, Currency currency, DateTime date)
        {
            DateTime day = date;
            List<Transaction> TodayTransactions = new();
            foreach (var transaction in transactions)
            {
                if (transaction.Date.Equals(day) && transaction.Currency.Equals(currency)) TodayTransactions.Add(transaction);
            }
            return TodayTransactions;
        }
        public static List<Transaction> WeekTransactions(List<Transaction> transactions, Currency currency, List<DateTime> dateList)
        {
            List<Transaction> WeekTransactions = new();
            foreach (var transaction in transactions)
            {
                foreach (var date in dateList)
                {
                    if (transaction.Date.Equals(date) && transaction.Currency.Equals(currency)) WeekTransactions.Add(transaction);
                }
            }
            return WeekTransactions;
        }

        public static List<Transaction> WeekTransactions(List<Transaction> transactions, Currency currency)
        {
            return WeekTransactions(transactions, currency, DateTimeService.Week());
        }

        public static List<Transaction> MonthTransactions(List<Transaction> transactions, Currency currency)
        {
            return MonthTransactions(transactions, currency, DateTime.Today.Month);
        }

        public static List<Transaction> MonthTransactions(List<Transaction> transactions, Currency currency,int month)
        {
            DateTime date = new DateTime(DateTime.Today.Year, month, 1);
            List<DateTime> Month = DateTimeService.Month(date);
            List<Transaction> MonthTransactions = new();
            foreach (var transaction in transactions)
            {
                foreach (var day in Month)
                {
                    if (transaction.Date.Equals(day) && transaction.Currency.Equals(currency)) MonthTransactions.Add(transaction);
                }
            }
            return MonthTransactions;
        }

        public static List<Transaction> YearTransactions(List<Transaction> transactions, Currency currency)
        {
            return YearTransactions(transactions, currency, DateTime.Today.Year);
        }
        public static List<Transaction> YearTransactions(List<Transaction> transactions, Currency currency, int year)
        {
            DateTime date = new DateTime(DateTime.Today.Year, 1, 1);
            List<DateTime> Year = DateTimeService.Year(date);
            List<Transaction> YearTransactions = new();
            foreach(var transaction in transactions)
            {
                foreach(var day in Year)
                {
                    if (transaction.Date.Equals(day) && transaction.Currency.Equals(currency)) YearTransactions.Add(transaction);
                }
            }
            return YearTransactions;
        }
        #endregion

        #region BalanceChanges

        public void ChangeAccountBalance(Account accountStart, Account accountEnd, float sumStart, float sumEnd)
        {
            if (accountStart.Equals(accountEnd))
            {
                int index = Accounts.IndexOf(accountStart);
                Accounts.ElementAt(index).Balance += (sumStart - sumEnd);
            }
            else
            {
                int index = Accounts.IndexOf(accountStart);
                Accounts.ElementAt(index).Balance +=sumStart;
                index = Accounts.IndexOf(accountEnd);
                Accounts.ElementAt(index).Balance -= sumEnd;
            }
        }


        public void ReduceAccountBalance(Account account, float sum)
        {
            int index = Accounts.IndexOf(account);
            Accounts.ElementAt(index).Balance -= sum;
        }
        public void AppendAccountBalance(Account account, float sum)
        {
            int index = Accounts.IndexOf(account);
            Accounts.ElementAt(index).Balance += sum;
        }
       

        public void AddTransactionChangeBalance(Transaction newTransaction)
        {
            OperationType Type = newTransaction.Type;
            if (Type == OperationType.Expenses) ReduceAccountBalance(newTransaction.Account, newTransaction.Money);
            else AppendAccountBalance(newTransaction.Account, newTransaction.Money);
        }
        public void DeleteTransactionChangeBalance(Transaction SelectedTransaction)
        {
            OperationType Type = SelectedTransaction.Type;
            if (Type == OperationType.Expenses)
                AppendAccountBalance(SelectedTransaction.Account, SelectedTransaction.Money);
            else ReduceAccountBalance(SelectedTransaction.Account, SelectedTransaction.Money);
        }

        public void EditTransactionChangeBalance(Transaction SelectedTransaction, Transaction newTransaction)
        {
            OperationType Type = SelectedTransaction.Type;
            if (Type == OperationType.Expenses) ChangeAccountBalance(SelectedTransaction.Account, newTransaction.Account, SelectedTransaction.Money, newTransaction.Money);
            else ChangeAccountBalance(newTransaction.Account, SelectedTransaction.Account, newTransaction.Money, SelectedTransaction.Money);
        }
        #endregion
        public static Service GetInstance()
        {
            if (instance == null) instance = new Service();
            return instance;
        }
    }
}
