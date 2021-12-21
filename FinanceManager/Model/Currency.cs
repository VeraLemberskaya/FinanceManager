using FinanceManager.Core;
using System;

namespace FinanceManager.Model
{
    public class Currency : IComparable<Currency>
    {
        public int CurrencyId { get; set; }

        private string _name;
        private string _designation;

        #region Properties
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
            }
        }
        public string Designation
        {
            get => _designation;
            set
            {
                _designation = value;
            }
        }
        #endregion
        private Currency()
        {
            this._name = string.Empty;
            this._designation = string.Empty;
        }
        public Currency(string Name, string Designation)
        {
            this.Name = Name;
            this.Designation = Designation;
        }
        public override string ToString()
        {
            return $"{Designation}";
        }

        public int CompareTo(Currency obj)
        {
            return this.Name.CompareTo(obj.Name);
        }
    }
}
