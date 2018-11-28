using Antlr4.Runtime;
using Microsoft.Win32;
using Robot.Errors;
using Robot.Grammar;
using Robot.Visitors;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Robot.ViewModels
{
    public class MainViewModel //: INotifyCollectionChanged
    {
        public ObservableCollection<ErrorLogItem> errorList { get; } = new ObservableCollection<ErrorLogItem>();

       // public event NotifyCollectionChangedEventHandler CollectionChanged;

        public void asd(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Test");
        }


    }
}
