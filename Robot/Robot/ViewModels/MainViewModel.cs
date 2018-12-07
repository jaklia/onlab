using Robot.Errors;
using System.Collections.ObjectModel;
using System.Windows;

namespace Robot.ViewModels
{
    public class MainViewModel 
    {
        public ObservableCollection<ErrorLogItem> errorList { get; } = new ObservableCollection<ErrorLogItem>();


        public void asd(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Test");
        }


    }
}
