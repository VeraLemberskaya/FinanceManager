using System.Collections.Generic;
using FinanceManager.Core;

namespace FinanceManager.Model
{
    public class Account
    {
        public int AccountId { get; set; }

        private string _name;
        private float _balance;
        private Currency _currency;
        private string _image;
        private bool _takeIntoBalance;

        #region Properties
        public string Name
        {
            get => _name;
            set
            {
                this._name = value;
            }
        }
        public float Balance
        {
            get => _balance;
            set
            {
                this._balance = value;
            }
        }
        public Currency Currency
        {
            get => _currency;
            set
            {
                _currency = value;
            }
        }
        public string Image
        {
            get => _image;
            set
            {
                this._image = value;
            }
        }
        public bool TakeIntoBalance
        {
            get => _takeIntoBalance;
            set
            {
                _takeIntoBalance = value;
            }
        }
        #endregion

        public Account()
        {
            this._name = "New Account";
            this._balance = 0;
            this._currency = null;
            this._image = "WalletOutline";
            this._takeIntoBalance = true;
        }
        public Account(Currency Currency) : this()
        {
            this.Currency = Currency;
        }
        public Account(string Name, float Balance, Currency Currency, bool TakeIntoBalance) : this(Currency)
        {
            this.Name = Name;
            this.Balance = Balance;
            this.TakeIntoBalance = TakeIntoBalance;
        }
        public Account(string Name, float Balance, Currency Currency, string Image, bool TakeIntoBalance)
        {
            this.Name = Name;
            this.Balance = Balance;
            this.Currency = Currency;
            this.Image = Image;
            this.TakeIntoBalance = TakeIntoBalance;
        }

        public override string ToString()
        {
            return $"{Name}  {Balance}{Currency.Designation}";
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
