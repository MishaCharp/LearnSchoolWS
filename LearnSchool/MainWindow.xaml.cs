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

namespace LearnSchool
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            OurServicesLabel.Background = new SolidColorBrush(Color.FromRgb(231,250,191));
            MainHeaderLabel.Background = new SolidColorBrush(Color.FromRgb(231, 250, 191));
        }

        private void OurServicesLabelClick(object sender, MouseButtonEventArgs e)
        {
            Services ifrm = new Services();
            ifrm.Left = this.Left;
            ifrm.Top  = this.Top;
            ifrm.Show();
            this.Hide();
        }
    }
}
