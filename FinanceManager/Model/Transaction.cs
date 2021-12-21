using System;
using FinanceManager.Core;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceManager.Model
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        private DateTime _date;
        private Category _category;
        private string _comment;
        private Account _account;
        private float _money;
        private OperationType _type;
        private float _startMoney;

        #region Properties
        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
            }
        }
        public Category Category
        {
            get => _category;
            set
            {
                _category = value;
            }
        }
        public string Comment
        {
            get => _comment;
            set
            {
                _comment = value;
            }
        }
        public Account Account
        {
            get => _account;
            set
            {
                _account = value;
            }
        }
        public float Money
        {
            get => _money;
            set
            {
                _money = value;
            }
        }
        public Currency Currency
        {
            get => Account.Currency;
        }
        public OperationType Type
        {
            get => _type;
            set
            {
                _type = value;
            }
        }
        [NotMapped]
        public float StartMoney
        {
            get => _startMoney;
            set
            {
                _startMoney = value;
            }
        }
        #endregion
        public Transaction()
        {
            this._date = DateTime.Today;
            this._category = null;
            this._comment = string.Empty;
            this._account = null;
            this._money = 0;
            this._type = OperationType.Expenses;
            this._startMoney = _money;
        }

        public Transaction(Account Account, OperationType Type)
        {
            this.Date = DateTime.Today;
            this.Category = null;
            this.Comment = string.Empty;
            this.Account = Account;
            this.Money = 0;
            this._startMoney = Money;
            this.Type = Type;
        }
        public Transaction(DateTime Date, Account Account, Category Category, float Money, OperationType Type) : this(Account, Type)
        {
            this.Date = Date;
            this.Category = Category;
            this.Money = Money;
            _startMoney = Money;
        }
        public Transaction(DateTime Date, Account Account, Category Category, string Comment, float Money, OperationType Type) : this(Date, Account, Category, Money, Type)
        {
            this.Comment = Comment;
        }
        public bool Validate(string Property, ref string Error)
        {
            ValidationRules vr = ValidationRules.GetInstance();
            return vr.Validate(this, Property, ref Error);
        }
        public bool IsValid
        {
            get
            {
                ValidationRules vr = ValidationRules.GetInstance();
                if (vr.Validate(this)) return true;
                return false;
            }
        }
    }
}
