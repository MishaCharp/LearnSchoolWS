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

namespace LearnSchool
{
    /// <summary>
    /// Логика взаимодействия для Check.xaml
    /// </summary>
    public partial class Check : Window
    {
        public Check()
        {
            InitializeComponent();
        }

        public bool Result=false;

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Services ifrm = new Services();
            ifrm.Variant = 0;
            ifrm.Show();
            this.Hide();
        }

        private void EnterButtonClick(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Password == "000000")
            {
                Result=true;
                Services ifrm = new Services();
                ifrm.Variant = 1;
                ifrm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Вы ввели неверный код!\nПопробуйте еще раз!");
                Result = false;
            }
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
