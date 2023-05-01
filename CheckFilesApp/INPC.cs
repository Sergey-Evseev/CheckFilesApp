using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CheckFilesApp
{
    // INotifyPropertyChanged interface used in WPF to notify the UI when a property of a data-bound
    // object has changed, so that the UI can update itself accordingly.
    public class INPC : INotifyPropertyChanged
    {
        //event raised whenever a property value changes
        public event PropertyChangedEventHandler PropertyChanged;

        //method OnPropertyChanged called when a property is changed, which then raises the PropertyChanged event
        //method uses the CallerMemberName attribute to automatically retrieve the name of the calling property,
        //which is then passed as an argument to the PropertyChanged event
        public void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        //If the OnPropertyChanged method is called without any arguments, it sets the property string
        //argument to an empty string, which means that no specific property was changed.
    }
}
