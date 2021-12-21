using FinanceManager.Model;
using System.Collections.ObjectModel;
using FinanceManager.Core;
using System.ComponentModel;
using System;
using FinanceManager.Services;

namespace FinanceManager.ViewModel
{
    class CategoryViewModel : BaseVM, IDataErrorInfo, ISaveObjectChanges
    {
        Service service = Service.GetInstance();

        public event ISaveObjectChanges.SaveObjectChangesHandler SaveObject;
        public RelayCommand SaveCategory { get; set; }
        public static ObservableCollection<string> Icons { get; set; } = new ObservableCollection<string>()
        {
           "BabyBottleOutline", "BabyFaceOutline", "Badminton", "BacteriaOutline", "BasketFill",
           "BankOutline", "BathtubOutline", "BedSingle","Billboard","BottleTonicPlus","BottleWineOutline",
           "BowlMixOutline","BusDoubleDecker","CakeVariantOutline","CandelabraFire","Campfire",
           "CarConvertible","CardsHeart","CardsPlayingSpadeMultiple","Cart","Cat","Charity","SackPercent",
           "ChessQueen","Church","Cigar","HomeHeart","PaletteOutline","FormatPaint","GiftOpenOutline",
           "Basketball","Crowd","SchoolOutline","CoffeeOutline","BottleTonicPlusOutline","Lipstick","HandCoinOutline"
        };

        private Category _category;

        private CategoryViewModel()
        {
            SaveCategory = new RelayCommand(obj =>
              {
                  SaveObjectExecute(_category);
              });
        }

        public CategoryViewModel(Category category) : this()
        {
            _category = category;
        }
        #region Properties
        public bool IsValid
        {
            get => _category.IsValid;
        }

        public float DefaultSum
        {
            get => _category.DefaultSum;
            set
            {
                _category.DefaultSum = value;
                OnPropertyChanged(nameof(IsValid));
                OnPropertyChanged();
            }
        }
        public string Name
        {
            get => _category.Name;
            set
            {
                _category.Name = value;
                OnPropertyChanged(nameof(IsValid));
                OnPropertyChanged();
            }
        }
        public string Image
        {
            get => _category.Image;
            set
            {
                _category.Image = value;
                OnPropertyChanged();
            }
        }

        public OperationType Type
        {
            get => _category.Type;
            set
            {
                _category.Type = value;
                OnPropertyChanged();
            }
        }

        public Category Category
        {
            get => _category;
        }

        public Currency DefaultCurrency
        {
            get
            {
                return service.DefaultCurrency;
            }
        }
        public bool SuitsDefaultSum
        {
            get => _category.SuitsDefaultSum;
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
                    case "Name":
                        _category.Validate("NameIsNotNull", ref error);
                        _category.Validate("NameValidLength", ref error);
                        break;
                    case "DefaultSum":
                        _category.Validate("SumIsValid", ref error);
                        _category.Validate("SumIsNotNegative", ref error);
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
            SaveObject?.Invoke(this, new SaveObjectChangesEventArgs(obj as Category));
        }
    }
}
