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
    /// Логика взаимодействия для Services.xaml
    /// </summary>
    public partial class Services : Window
    {
        public Services()
        {
            InitializeComponent();
        }

        SqlConnection cn = new SqlConnection("Data Source=stud-mssql.sttec.yar.ru,38325;Database=user2_db;User Id=user2_db;Password=user2");

        public int Variant = 0;

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            if (Variant == 0)
            {
                LoadUserVariant();
            }
            else
            {
                LoadAdminVariant();
                MySlider.Value=1;
            }
        }

        private void BtnChangeClick(object sender, RoutedEventArgs e)
        {

            var button = (Button)sender;
            var sp = (Grid)button.Parent;


            sp.Children.Remove(button);
        }

        private void BtnDeleteClick(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var sp = (Grid)button.Parent;
            sp.Children.Remove(button);
            MySecondGrid.Children.Remove(sp);
        }

        public void LoadAdminVariant()
        {

            UserOrAdmin.Content = "Режим администратора";

            AddNewButton.Visibility = Visibility.Visible;

            MainHeaderLabel.Background = new SolidColorBrush(Color.FromRgb(231, 250, 191));

            cn.Open();
            string QueryCountFields = "SELECT COUNT(*) FROM Service";
            SqlCommand CmdQueryCount = new SqlCommand(QueryCountFields, cn);
            int Count = int.Parse(CmdQueryCount.ExecuteScalar().ToString());
            string query = "SELECT Title, Cost, DurationInSeconds,Discount, MainImagePath FROM Service";
            SqlCommand CmdQuery = new SqlCommand(query, cn);
            SqlDataReader dr = CmdQuery.ExecuteReader();

            int i = 0;

            while (dr.Read())
            {


                Grid sp = new Grid();

                sp.Width = 600;
                sp.Height = 180;
                sp.Background = new SolidColorBrush(Color.FromRgb(231, 250, 191));

                Grid.SetRow(sp, 1);

                if (i == 0)
                {
                    sp.Margin = new Thickness(0, 10, 0, 10);
                }
                else
                {
                    sp.Margin = new Thickness(0, 203 * i, 0, 10);
                }


                sp.VerticalAlignment = VerticalAlignment.Top;
                sp.HorizontalAlignment = HorizontalAlignment.Center;
                MySecondGrid.Children.Add(sp);


                Button BtnChange = new Button();
                BtnChange.Width = 200;
                BtnChange.Height = 35;
                BtnChange.FontSize = 17;
                BtnChange.VerticalAlignment = VerticalAlignment.Bottom;
                BtnChange.Margin = new Thickness(160, 0, 0, 0);
                BtnChange.Content = $"Изменить";
                BtnChange.Click += BtnChangeClick;
                BtnChange.HorizontalAlignment = HorizontalAlignment.Left;

                Button BtnDelete = new Button();
                BtnDelete.Width = 200;
                BtnDelete.Height = 35;
                BtnDelete.FontSize = 17;
                BtnDelete.VerticalAlignment = VerticalAlignment.Bottom;
                BtnDelete.HorizontalAlignment = HorizontalAlignment.Right;
                BtnDelete.Margin = new Thickness(0, 0, 30, 0);
                BtnDelete.Content = $"Удалить";
                BtnDelete.Click += BtnDeleteClick;

                TextBlock HeaderTextBlock = new TextBlock();
                HeaderTextBlock.Width = 450;
                HeaderTextBlock.Height = 75;
                HeaderTextBlock.FontSize = 19;
                HeaderTextBlock.FontWeight = FontWeights.Bold;
                HeaderTextBlock.TextWrapping = TextWrapping.Wrap;
                HeaderTextBlock.VerticalAlignment = VerticalAlignment.Top;
                HeaderTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
                HeaderTextBlock.Margin = new Thickness(150, 0, 0, 0);
                HeaderTextBlock.Text = dr.GetString(0);

                Image img = new Image();
                BitmapImage source = new BitmapImage();

                if (!dr.IsDBNull(4))
                {
                    source.BeginInit();
                    string PhotoUri = dr.GetString(4).Substring(13);
                    source.UriSource = new Uri("/LearnSchool;component/Услуги школы/" + PhotoUri, UriKind.Relative);
                    source.EndInit();
                    img.Source = source;
                }

                img.Height = 180;
                img.Width = 150;
                img.HorizontalAlignment = HorizontalAlignment.Left;
                img.Clip = new EllipseGeometry() { RadiusX = 90, RadiusY = 90, Center = new Point(85, 74) };


                TextBlock DiscountTextBlock = new TextBlock();
                DiscountTextBlock.Width = 110;
                DiscountTextBlock.Height = 75;
                DiscountTextBlock.FontSize = 26;
                DiscountTextBlock.VerticalAlignment = VerticalAlignment.Top;
                DiscountTextBlock.HorizontalAlignment = HorizontalAlignment.Left;
                DiscountTextBlock.Margin = new Thickness(150, 55, 0, 0);

                TextBlock NotDiscountTextBlock = new TextBlock();
                NotDiscountTextBlock.Width = 310;
                NotDiscountTextBlock.Height = 75;
                NotDiscountTextBlock.FontSize = 26;
                NotDiscountTextBlock.VerticalAlignment = VerticalAlignment.Top;
                NotDiscountTextBlock.HorizontalAlignment = HorizontalAlignment.Left;
                NotDiscountTextBlock.Margin = new Thickness(210, 55, 0, 0);

                TextBlock DiscountBlock = new TextBlock();
                DiscountBlock.Width = 310;
                DiscountBlock.Height = 75;
                DiscountBlock.FontSize = 19;
                DiscountBlock.VerticalAlignment = VerticalAlignment.Top;
                DiscountBlock.HorizontalAlignment = HorizontalAlignment.Left;
                DiscountBlock.Margin = new Thickness(150, 85, 0, 0);

                if (!dr.IsDBNull(3))
                {
                    int Discount = dr.GetInt32(3);
                    int PriceWithDis = Convert.ToInt32(dr.GetDecimal(1));
                    int PriceWithoutDis = PriceWithDis + (PriceWithDis * Discount) / 100;
                    DiscountTextBlock.Text = PriceWithoutDis.ToString();
                    DiscountTextBlock.TextDecorations = TextDecorations.Strikethrough;
                    NotDiscountTextBlock.Text = PriceWithDis.ToString() + " рублей";
                    NotDiscountTextBlock.FontWeight = FontWeights.Bold;
                    DiscountBlock.Text = "*" + Discount.ToString() + "% скидка";
                }
                else
                {
                    int PriceWithDis = Convert.ToInt32(dr.GetDecimal(1));
                    NotDiscountTextBlock.Text = PriceWithDis.ToString() + " рублей";
                    NotDiscountTextBlock.FontWeight = FontWeights.Bold;
                }


                sp.Children.Add(img);
                sp.Children.Add(BtnDelete);
                sp.Children.Add(BtnChange);
                sp.Children.Add(HeaderTextBlock);
                sp.Children.Add(DiscountTextBlock);
                sp.Children.Add(NotDiscountTextBlock);
                sp.Children.Add(DiscountBlock);

                i++;

            }
            cn.Close();
        }

        public void LoadUserVariant()
        {

            UserOrAdmin.Content = "Режим пользователя";
            AddNewButton.Visibility = Visibility.Hidden;
            AddNewServiceClient.Visibility = Visibility.Hidden;
            ComingRecords.Visibility = Visibility.Hidden;

            MainHeaderLabel.Background = new SolidColorBrush(Color.FromRgb(231, 250, 191));

            cn.Open();
            string QueryCountFields = "SELECT COUNT(*) FROM Service";
            SqlCommand CmdQueryCount = new SqlCommand(QueryCountFields, cn);
            int Count = int.Parse(CmdQueryCount.ExecuteScalar().ToString());
            string query = "SELECT Title, Cost, DurationInSeconds,Discount, MainImagePath FROM Service";
            SqlCommand CmdQuery = new SqlCommand(query, cn);
            SqlDataReader dr = CmdQuery.ExecuteReader();

            int i = 0;

            while (dr.Read())
            {


                Grid sp = new Grid();

                sp.Width = 600;
                sp.Height = 180;
                sp.Background = new SolidColorBrush(Color.FromRgb(231, 250, 191));

                Grid.SetRow(sp, 1);
                Grid.SetRowSpan(MyScroll, 2);

                if (i == 0)
                {
                    sp.Margin = new Thickness(0, 10, 0, 10);
                }
                else
                {
                    sp.Margin = new Thickness(0, 203 * i, 0, 10);
                }


                sp.VerticalAlignment = VerticalAlignment.Top;
                sp.HorizontalAlignment = HorizontalAlignment.Center;
                MySecondGrid.Children.Add(sp);

                TextBlock HeaderTextBlock = new TextBlock();
                HeaderTextBlock.Width = 450;
                HeaderTextBlock.Height = 75;
                HeaderTextBlock.FontSize = 19;
                HeaderTextBlock.FontWeight = FontWeights.Bold;
                HeaderTextBlock.TextWrapping = TextWrapping.Wrap;
                HeaderTextBlock.VerticalAlignment = VerticalAlignment.Top;
                HeaderTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
                HeaderTextBlock.Margin = new Thickness(150, 0, 0, 0);
                HeaderTextBlock.Text = dr.GetString(0);

                Image img = new Image();
                BitmapImage source = new BitmapImage();

                if (!dr.IsDBNull(4))
                {
                    source.BeginInit();
                    string PhotoUri = dr.GetString(4).Substring(13);
                    source.UriSource = new Uri("/LearnSchool;component/Услуги школы/" + PhotoUri, UriKind.Relative);
                    source.EndInit();
                    img.Source = source;
                }

                img.Height = 180;
                img.Width = 150;
                img.HorizontalAlignment = HorizontalAlignment.Left;
                img.Clip = new EllipseGeometry() { RadiusX = 90, RadiusY = 90, Center = new Point(85, 74) };


                TextBlock DiscountTextBlock = new TextBlock();
                DiscountTextBlock.Width = 110;
                DiscountTextBlock.Height = 75;
                DiscountTextBlock.FontSize = 26;
                DiscountTextBlock.VerticalAlignment = VerticalAlignment.Top;
                DiscountTextBlock.HorizontalAlignment = HorizontalAlignment.Left;
                DiscountTextBlock.Margin = new Thickness(150, 55, 0, 0);

                TextBlock NotDiscountTextBlock = new TextBlock();
                NotDiscountTextBlock.Width = 310;
                NotDiscountTextBlock.Height = 75;
                NotDiscountTextBlock.FontSize = 26;
                NotDiscountTextBlock.VerticalAlignment = VerticalAlignment.Top;
                NotDiscountTextBlock.HorizontalAlignment = HorizontalAlignment.Left;
                NotDiscountTextBlock.Margin = new Thickness(210, 55, 0, 0);

                TextBlock DiscountBlock = new TextBlock();
                DiscountBlock.Width = 310;
                DiscountBlock.Height = 75;
                DiscountBlock.FontSize = 19;
                DiscountBlock.VerticalAlignment = VerticalAlignment.Top;
                DiscountBlock.HorizontalAlignment = HorizontalAlignment.Left;
                DiscountBlock.Margin = new Thickness(150, 85, 0, 0);

                if (!dr.IsDBNull(3))
                {
                    int Discount = dr.GetInt32(3);
                    int PriceWithDis = Convert.ToInt32(dr.GetDecimal(1));
                    int PriceWithoutDis = PriceWithDis + (PriceWithDis * Discount) / 100;
                    DiscountTextBlock.Text = PriceWithoutDis.ToString();
                    DiscountTextBlock.TextDecorations = TextDecorations.Strikethrough;
                    NotDiscountTextBlock.Text = PriceWithDis.ToString() + " рублей";
                    NotDiscountTextBlock.FontWeight = FontWeights.Bold;
                    DiscountBlock.Text = "*" + Discount.ToString() + "% скидка";
                }
                else
                {
                    int PriceWithDis = Convert.ToInt32(dr.GetDecimal(1));
                    NotDiscountTextBlock.Text = PriceWithDis.ToString() + " рублей";
                    NotDiscountTextBlock.FontWeight = FontWeights.Bold;
                }


                sp.Children.Add(img);
                sp.Children.Add(HeaderTextBlock);
                sp.Children.Add(DiscountTextBlock);
                sp.Children.Add(NotDiscountTextBlock);
                sp.Children.Add(DiscountBlock);

                i++;

            }
            cn.Close();
        }


        private void MySliderDragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            if (MySlider.Value == 0)
            {
                Services ifrm = new Services();
                ifrm.Left = this.Left;
                ifrm.Top = this.Top;
                ifrm.Variant = 0;
                ifrm.Show();
                this.Hide();
            }
            else
            {
                Check check = new Check();
                check.Owner = this;
                check.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                check.Show();
                this.Hide();

            }
        }

        private void AddNewButtonClick(object sender, RoutedEventArgs e)
        {
            NewService ifrm = new NewService();
            ifrm.Owner = this;
            ifrm.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            ifrm.Show();
            this.Hide();
        }

        private void ImageMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            MainWindow ifrm = new MainWindow();
            ifrm.Show();
            ifrm.Left = this.Left;
            ifrm.Top = this.Top;
            this.Hide();

        }


        private void WindowClosed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void AddNewServiceClientClick(object sender, RoutedEventArgs e)
        {
            RecordClient ifrm = new RecordClient();
            ifrm.Left = this.Left;
            ifrm.Top = this.Top;
            ifrm.Show();
            this.Hide();
        }

        private void ComingRecordsClick(object sender, RoutedEventArgs e)
        {

            ComingServices ifrm = new ComingServices();
            ifrm.Owner = this;
            ifrm.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            ifrm.Show();
            this.Hide();
        }
    }
}
