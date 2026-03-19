using PhishingApp.Views;
using System.Windows;

namespace PhishingApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainContent.Content = new MainView();
        }
    }
}
