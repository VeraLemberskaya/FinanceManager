using System.ComponentModel;
using System.Runtime.CompilerServices;
using FinanceManager.Model;

namespace FinanceManager.Core
{
    public enum OperationType
    {
        Income,
        Expenses
    }
    public enum Period
    {
        Day,
        Week,
        Month,
        Year
    }
    abstract class BaseVM : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
