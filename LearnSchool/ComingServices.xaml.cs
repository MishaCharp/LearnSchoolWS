using System;
using System.Collections.Generic;
using System.Data;
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
    /// Логика взаимодействия для ComingServices.xaml
    /// </summary>
    public partial class ComingServices : Window
    {
        public ComingServices()
        {
            InitializeComponent();
        }

        SqlConnection cn = new SqlConnection("Data Source=stud-mssql.sttec.yar.ru,38325;Database=user2_db;User Id=user2_db;Password=user2");

        DataTable dt = new DataTable();

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {

            UpdateGrid();

            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();

            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 30);
            timer.Start();

            MainHeaderLabel.Background = new SolidColorBrush(Color.FromRgb(231, 250, 191));

        }

        void UpdateGrid()
        {
            cn.Open();
            dt.Clear();
            string query = "SELECT c.Title,b.FirstName+' '+b.LastName+' '+b.Patronymic AS ФИО,b.Email,b.Phone AS Телефон,StartTime AS 'Дата начала', CONVERT(VARCHAR,DATEDIFF(HOUR,GETDATE(),StartTime)-3)+'ч '+CONVERT(VARCHAR,(DATEDIFF(MINUTE,GETDATE(),StartTime)-180)%60)+'м' AS 'До начала' FROM ClientService a JOIN Client b ON a.ClientID = b.ID JOIN Service c ON a.ServiceID = c.ID WHERE YEAR(StartTime)=YEAR(GETDATE()) AND MONTH(StartTime) = MONTH(GETDATE()) AND (DAY(StartTime) = DAY(GETDATE()) AND DATEPART(HOUR, StartTime) >= DATEPART(HOUR, (GETDATE())) OR DAY(StartTime) = DAY(GETDATE()) + 1 AND DATEPART(HOUR, StartTime) >= 0); ";
            SqlCommand cmd = new SqlCommand(query, cn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            MyDataGrid.ItemsSource = dt.DefaultView;
            cn.Close();
        }

        private void ImageMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            MainWindow ifrm = new MainWindow();
            ifrm.Show();
            ifrm.Left = this.Left;
            ifrm.Top = this.Top;
            this.Hide();

        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            Services ifrm = new Services();
            ifrm.Variant = 1;
            ifrm.Show();
            this.Hide();
        }

        private void timerTick(object sender, EventArgs e)
        {
            UpdateGrid();
        }

    }
}
