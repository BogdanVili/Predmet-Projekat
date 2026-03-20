using PhishingApp.ViewModel;
using System.Windows.Controls;

namespace PhishingApp.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class EmailSendingView : UserControl
    {
        public EmailSendingView()
        {
            InitializeComponent();
            DataContext = new EmailSendingViewModel();
        }
    }
}


