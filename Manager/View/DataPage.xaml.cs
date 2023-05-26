using Manager.View;
using System;
using System.Collections.Generic;
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

namespace Manager
{
    /// <summary>
    /// Логика взаимодействия для DataPage.xaml
    /// </summary>
    public partial class DataPage : Page
    {
        public DataPage()
        {
            InitializeComponent();
        }

        private void addCategory_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddCategory());
        }

        private void addWord_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddWord());
        }
    }
}
