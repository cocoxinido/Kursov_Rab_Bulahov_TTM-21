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
using System.Windows.Shapes;

namespace Kursov_Rab_Bulahov_TTM_21
{
    /// <summary>
    /// Логика взаимодействия для Add_URL.xaml
    /// </summary>
    public partial class Add_URL : Window
    {
        public Add_URL()
        {
            InitializeComponent();
        }

        private void Okey_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Cancel_But_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
