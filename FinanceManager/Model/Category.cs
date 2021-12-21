using System.Collections.Generic;
using FinanceManager.Core;
using FinanceManager.Services;

namespace FinanceManager.Model
{
    public class Category
    {
        public int CategoryId { get; set; }

        private string _name;
        private string _image;
        private float _defaultSum;
        private OperationType _type;

        #region Properties

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
            }
        }
        public string Image
        {
            get => _image;
            set
            {
                _image = value;
            }
        }
        public float DefaultSum
        {
            get => _defaultSum;
            set
            {
                _defaultSum = value;
            }
        }
        public OperationType Type
        {
            get => _type;
            set
            {
                _type = value;
            }
        }
        #endregion

        private Category()
        {
            this._name = string.Empty;
            this._image = "DnsOutline";
            this._defaultSum = 0;
            this._type = OperationType.Expenses;
        }
        public Category(OperationType Type) : this()
        {
            this.Type = Type;
        }
        public Category(string Name, string Image, OperationType Type) : this(Type)
        {
            this.Name = Name;
            this.Image = Image;
        }
        public Category(string Name, float DefaultSum, string Image, OperationType Type) : this(Name, Image, Type)
        {
            this.DefaultSum = DefaultSum;
        }
        public override bool Equals(object obj)
        {
            if (obj is Category)
                return Name.Equals((obj as Category).Name) || CategoryId.Equals((obj as Category).CategoryId);
            return false;
        }
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
        public override string ToString()
        {
            return Name.ToString();
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
        public bool SuitsDefaultSum
        {
            get
            {
                Service service = Service.GetInstance();
                return service.GetCategorySum(this);
            }
        }
    }
}
