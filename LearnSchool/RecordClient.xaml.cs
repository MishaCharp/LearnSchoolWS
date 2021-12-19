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
    /// Логика взаимодействия для RecordClient.xaml
    /// </summary>
    public partial class RecordClient : Window
    {
        public RecordClient()
        {
            InitializeComponent();
        }

        SqlConnection cn = new SqlConnection("Data Source=stud-mssql.sttec.yar.ru,38325;Database=user2_db;User Id=user2_db;Password=user2");

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            MainHeaderLabel.Background = new SolidColorBrush(Color.FromRgb(231, 250, 191));

            cn.Open();

            string query = "SELECT ID, FirstName+' '+LastName+' '+Patronymic AS ФИО FROM Client";
            SqlCommand cmd = new SqlCommand(query, cn);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                ClientComboBox.Items.Add(dr.GetString(1));
            }

            dr.Close();

            string query2 = "SELECT Title FROM Service";
            SqlCommand cmd2 = new SqlCommand(query2, cn);
            dr = cmd2.ExecuteReader();


            while (dr.Read())
            {
                ServiceComboBox.Items.Add(dr.GetString(0));
            }

            dr.Close();

            cn.Close();

        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Services ifrm = new Services();
            ifrm.Variant = 1;
            ifrm.Show();
            this.Hide();
        }

        private void AddRecordButtonClick(object sender, RoutedEventArgs e)
        {
            if(ClientComboBox.SelectedIndex>=0 && ServiceComboBox.SelectedIndex >= 0 && MyDateTime.SelectedDate.ToString()!="" && int.TryParse(HoursTextBox.Text, out int res)==true && int.Parse(HoursTextBox.Text)>=0 && int.Parse(HoursTextBox.Text) <= 24 && int.TryParse(MinuteTextBox.Text, out int res2) == true && int.Parse(MinuteTextBox.Text) >= 0 && int.Parse(MinuteTextBox.Text) <= 60)
            {
                cn.Open();

                string[] fio = new string[2];
                fio = ClientComboBox.SelectedItem.ToString().Split(' ');

                string QueryGetClientId = "SELECT ID FROM Client WHERE FirstName='"+fio[0]+ "' AND LastName='" + fio[1] + "' AND Patronymic='" + fio[2] + "'";
                SqlCommand CmdGetIdClient = new SqlCommand(QueryGetClientId, cn);
                int IdClient = int.Parse(CmdGetIdClient.ExecuteScalar().ToString());

                string QueryGetServiceId = "SELECT ID FROM Service WHERE Title='"+ServiceComboBox.SelectedItem.ToString()+"'";
                SqlCommand CmdGetIdService = new SqlCommand(QueryGetServiceId, cn);
                int IdService = int.Parse(CmdGetIdService.ExecuteScalar().ToString());

                string DateTime = MyDateTime.Text + " " + HoursTextBox.Text + ":" + MinuteTextBox.Text;

                string MainQuery;

                if (CommentTextBox.Text=="")
                {
                    MainQuery = "INSERT INTO ClientService(ClientID,ServiceID,StartTime) VALUES('" + IdClient + "','" + IdService + "','" + DateTime + "')";
                }
                else
                {
                    MainQuery = "INSERT INTO ClientService(ClientID,ServiceID,StartTime,Comment) VALUES('" + IdClient + "','" + IdService + "','" + DateTime + "','"+CommentTextBox.Text+"')";
                }

                SqlCommand MainCmd = new SqlCommand(MainQuery, cn);
                MainCmd.ExecuteScalar();
                MessageBox.Show("Клиент [" + ClientComboBox.Text + "] успешно записан на [" + ServiceComboBox.Text + "] !");

                Services ifrm = new Services();
                ifrm.Variant = 1;
                ifrm.Show();
                this.Hide();

                cn.Close();
            }
            else
            {
                MessageBox.Show("Проверьте заполненность всех обязательных полей и их тип данных:\n" +
                    "[Услуга] - [Выбранный элемент]\n" +
                    "[Клиент] - [Выбранный элемент]\n" +
                    "[Комментарий] - [Не обязательно к заполнению] - [Текст]\n" +
                    "[Дата] - [Выбранный или введённый вручную тип данных]\n" +
                    "[Часы:Минуты] - [Целые числа с диапазоном 0-24 и 0-60 соответственно]");
            }
        }
    }
}
