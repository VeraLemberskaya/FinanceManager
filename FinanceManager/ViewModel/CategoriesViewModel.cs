using FinanceManager.Core;
using FinanceManager.Model;
using System.Collections.ObjectModel;
using FinanceManager.Services;
using System.Windows.Data;

namespace FinanceManager.ViewModel
{
    class CategoriesViewModel:BaseVM
    {
        Service service = Service.GetInstance();
        public RelayCommand ChangeSelection { get; set; }
        public RelayCommand AddExpensesCategory { get; set; }
        public RelayCommand AddIncomeCategory { get; set; }
        public RelayCommand EditCategory { get; set; }
        public ObservableCollection<CategoryViewModel> IncomeCategories
        {
            get
            {
                return new ObservableCollection<CategoryViewModel>(ServiceConverter.ConvertToCategoryVM(service.IncomeCategories));
            }
        }
        public ObservableCollection<CategoryViewModel> ExpensesCategories
        {
            get
            {
                return new ObservableCollection<CategoryViewModel>(ServiceConverter.ConvertToCategoryVM(service.ExpensesCategories));
            }
        }
        private object _currentVM;
        private CategoryViewModel _selectedCategory;

        public CategoriesViewModel()
        {
            _selectedTab = 0;
            Service service = Service.GetInstance();

            #region Commands
            ChangeSelection = new RelayCommand(obj =>
            {
                CurrentVM = null;
                SelectedCategory = null;
            });
            AddExpensesCategory = new RelayCommand(obj =>
              {
                  SelectedCategory = null;
                  Category category = new Category(OperationType.Expenses);
                  CurrentVM = new CategoryViewModel(category);
                  (CurrentVM as CategoryViewModel).SaveObject += AddCategory;
              });
            AddIncomeCategory = new RelayCommand(obj =>
            {
                SelectedCategory = null;
                Category category = new Category(OperationType.Income);
                CurrentVM = new CategoryViewModel(category);
                (CurrentVM as CategoryViewModel).SaveObject += AddCategory;
            });
            EditCategory = new RelayCommand(obj =>
              {
                  CurrentVM = new CategoryViewModel(new Category(SelectedCategory.Name, SelectedCategory.DefaultSum, SelectedCategory.Image, SelectedCategory.Type));
                  (CurrentVM as CategoryViewModel).SaveObject += (object sender, SaveObjectChangesEventArgs e) =>
                  {
                      Category CategoryModel = e.Object as Category;
                      service.EditCategory(SelectedCategory.Category, CategoryModel);
                      OnPropertyChanged(nameof(IncomeCategories));
                      OnPropertyChanged(nameof(ExpensesCategories));
                      CurrentVM = null;
                  };
              });
            #endregion
        }
        private void AddCategory(object sender, SaveObjectChangesEventArgs e)
        {
            SelectedCategory = null;
            Category category = e.Object as Category;
            service.AddCategory(category);
            if(category.Type == OperationType.Income) OnPropertyChanged(nameof(IncomeCategories));
            else OnPropertyChanged(nameof(ExpensesCategories));
            CurrentVM = null;
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
        public CategoryViewModel SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged();
                CurrentVM = null;
            }
        }
        private int _selectedTab;
        public int SelectedTab
        {
            get => _selectedTab;
            set
            {
                _selectedTab = value;
                CurrentVM = null;
                SelectedCategory = null;
                OnPropertyChanged();
            }
        }
        #endregion
        public void UpDate()
        {
            CurrentVM = null;
        }
    }

}
