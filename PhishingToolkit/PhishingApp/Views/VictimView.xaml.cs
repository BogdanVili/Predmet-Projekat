using PhishingApp.ViewModel;
using System.Windows.Controls;

namespace PhishingApp.Views
{
    /// <summary>
    /// Interaction logic for VictimView.xaml
    /// </summary>
    public partial class VictimView : UserControl
    {
        public VictimView()
        {
            InitializeComponent();

            DataContext = new VictimViewModel();
        }
    }
}
