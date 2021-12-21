using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FinanceManager.ViewModel;

namespace FinanceManager.View
{
    /// <summary>
    /// Логика взаимодействия для Accounts.xaml
    /// </summary>
    public partial class AccountsView: UserControl
    {
        public AccountsView()
        {
            InitializeComponent();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
            {
                DeleteButton.Visibility = Visibility.Collapsed;
                AddButton.Visibility = Visibility.Visible;
            }
            else
            {
                DeleteButton.Visibility = Visibility.Visible;
                AddButton.Visibility = Visibility.Collapsed;
            }
        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.AccountsList.SelectedItem = null;
        }
    }
}
