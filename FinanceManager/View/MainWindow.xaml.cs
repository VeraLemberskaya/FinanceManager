using System.Windows;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;

namespace FinanceManager.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            ITheme theme = paletteHelper.GetTheme();
            if(!Setting.Default.ThemeToggle)
            {
                theme.SetBaseTheme(Theme.Light);
                ThemeToggle.IsChecked = false;
            }
            else
            {
                theme.SetBaseTheme(Theme.Dark);
                ThemeToggle.IsChecked = true;
            }
            paletteHelper.SetTheme(theme);
        } 
        public bool IsDarkTheme { get; set; }
        private readonly PaletteHelper paletteHelper = new();

        private void ThemeToggle_Click(object sender, RoutedEventArgs e)
        {
            ITheme theme = paletteHelper.GetTheme();
            if(IsDarkTheme = theme.GetBaseTheme()==BaseTheme.Dark)
            {
                IsDarkTheme = false;
                theme.SetBaseTheme(Theme.Light);
                Setting.Default.ThemeToggle = false;
                Setting.Default.Save();
            }
            else
            {
                IsDarkTheme = true;
                theme.SetBaseTheme(Theme.Dark);
                Setting.Default.ThemeToggle = true;
                Setting.Default.Save();
            }
            paletteHelper.SetTheme(theme);
        }

        private void exit_Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuOpenToggleButton_Click(object sender, RoutedEventArgs e)
        {
            MenuOpenToggleButton.Visibility = Visibility.Collapsed;
            MenuCloseToggleButton.Visibility = Visibility.Visible;
        }

        private void MenuCloseToggleButton_Click(object sender, RoutedEventArgs e)
        {
            MenuOpenToggleButton.Visibility = Visibility.Visible;
            MenuCloseToggleButton.Visibility = Visibility.Collapsed;
        }

        private void Card_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }

    }
}
