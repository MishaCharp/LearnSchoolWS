using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    /// Логика взаимодействия для NewService.xaml
    /// </summary>
    public partial class NewService : Window
    {
        public NewService()
        {
            InitializeComponent();
        }

        SqlConnection cn = new SqlConnection("Data Source=stud-mssql.sttec.yar.ru,38325;Database=user2_db;User Id=user2_db;Password=user2");

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Services ifrm = new Services();
            ifrm.Variant = 1;
            ifrm.Show();
            this.Hide();
        }

        void ActionsAfterCheck()
        {

            bool Flag = false;

            if (TitleService.Text != "" && int.TryParse(CostService.Text, out int result) == true && int.TryParse(DurationService.Text, out int result2) == true)
            {

                string query = "";
                if (DiscountService.Text != "")
                {
                    int res;
                    if (int.TryParse(DiscountService.Text, out res) == true)
                    {
                        query = "INSERT INTO Service(Title,Cost,DurationInSeconds,Description,Discount) VALUES('" + TitleService.Text + "','" + CostService.Text + "','" + DurationService.Text + "','" + DescriptionService.Text + "','" + DiscountService.Text + "')";
                        Flag = true;
                    }
                    else
                    {
                        MessageBox.Show("Заполните поле [Скидка] типом [Целые числа]");
                        DiscountService.Text = "";
                    }
                }
                else
                {
                    query = "INSERT INTO Service(Title,Cost,DurationInSeconds,Description,Discount) VALUES('" + TitleService.Text + "','" + CostService.Text + "','" + DurationService.Text + "','" + DescriptionService.Text + "','" + DBNull.Value + "')";
                    Flag = true;
                }

                if(Flag==true)
                {
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.ExecuteScalar();

                string queryTwo = "UPDATE Service SET Discount=NULL WHERE Discount = 0";
                SqlCommand cmdTwo = new SqlCommand(queryTwo, cn);
                cmdTwo.ExecuteScalar();

                MessageBox.Show("Услуга успешно добавлена!");

                Services ifrm = new Services();
                ifrm.Variant = 1;
                ifrm.Show();
                this.Hide();
                }
                else
                {

                }
                }
            else
            {
                MessageBox.Show("Обязательные поля с типом их заполнения \n[Описание] - [Текст]\n[Стоимость] - [Целые числа]\n[Длительность] - [Целые числа]\n");
            }
        }

        private void AddServiceButtonClick(object sender, RoutedEventArgs e)
        {
            cn.Open();
            ActionsAfterCheck();
            cn.Close();
        }
    }
}
