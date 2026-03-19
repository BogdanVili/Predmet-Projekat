using PhishingApp.Model;
using System.Windows;

namespace PhishingApp.Views
{
    /// <summary>
    /// Interaction logic for VictimsWindow.xaml
    /// </summary>
    public partial class VictimsWindow : Window
    {
        public VictimsWindow(StatisticsModel statisticsModel)
        {
            InitializeComponent();

            VictimsDataGrid.ItemsSource = statisticsModel.Victims;
        }
    }
}
