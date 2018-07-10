using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Grid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GridViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new GridViewModel();
            this.DataContext = viewModel;
        }

        private void OnUpdateButtonClick(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("OnUpdateButtonClick");
            viewModel.UpdateDataGrid(numberGrid);
            
        }
    }
}
