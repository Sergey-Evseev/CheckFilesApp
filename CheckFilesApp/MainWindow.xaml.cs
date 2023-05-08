using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using System.Xml.Linq;

namespace CheckFilesApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();
            //create the instance of the view model
            _viewModel = new MainWindowViewModel();
            //When the DataContext property is set to an instance of a class, the properties of that class
            //become available to the XAML elements in the user interface. This enables the XAML elements
            //to bind to the properties of the class and receive updates automatically when the values of
            //those properties change:
            this.DataContext = _viewModel;
        }

        //event handler method of button "Выбрать"
        private void SelectDirectory_Click(object sender, RoutedEventArgs e)
        {
            //using statement ensures Dispose() method is called automatically when the scope
            //of the using block is exited.
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                //if OK set the SelectedDirectory property of the _viewModel object
                //to the selected folder's path.
                //!string.IsNullOrWhiteSpace(fbd.SelectedPath) condition checks
                //whether the selected folder path is not null or empty
                if (result == System.Windows.Forms.DialogResult.OK 
                    && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    _viewModel.SelectedDirectory = fbd.SelectedPath;
                }
            }
        }

        private void ScanButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.ScanDirectory().Wait();
        }
    }
}
