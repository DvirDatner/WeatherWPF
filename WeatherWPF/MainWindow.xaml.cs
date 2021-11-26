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

namespace WeatherWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FullWeather fullWeather;

        public MainWindow()
        {
            InitializeComponent();
            ApiHelper.InitializeClient();

            spCurrent.Visibility = Visibility.Collapsed;
            spHourly.Visibility = Visibility.Collapsed;
            spDaily.Visibility = Visibility.Collapsed;
            tbErrorMsg.Visibility = Visibility.Collapsed;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            spCurrent.Visibility = Visibility.Collapsed;
            spHourly.Visibility = Visibility.Collapsed;
            spDaily.Visibility = Visibility.Collapsed;
            tbErrorMsg.Visibility = Visibility.Collapsed;

            string city = tbCity.Text.Trim();
            if (city.Length < 1)
            {
                tbErrorMsg.Text = "Error: Field 'City' can't be empty!";
                tbErrorMsg.Visibility = Visibility.Visible;
                return;
            }

            try
            {
                fullWeather = await WeatherProcessor.LoadWeather(city);

                InsertWeatherDetailsToScreen();

                rbCurrent.IsChecked = false;
                rbCurrent.IsChecked = true;
            }
            catch (Exception exp)
            {
                spCurrent.Visibility = Visibility.Collapsed;
                spHourly.Visibility = Visibility.Collapsed;
                spDaily.Visibility = Visibility.Collapsed;

                tbErrorMsg.Text = "Error: " + exp.Message;
                tbErrorMsg.Visibility = Visibility.Visible;
            }
        }

        private void InsertWeatherDetailsToScreen()
        {
            CreateCurrentWeatherAndInsertToScreen();
            CreateHourelyWeatherAndInsertToScreen();
            CreateDailyWeatherAndInsertToScreen();
        }

        private void CreateCurrentWeatherAndInsertToScreen()
        {
            spCurrent.Children.Clear();

            StackPanel spDate = CreateDateStackPanel(fullWeather.Current.Dt);

            spCurrent.Children.Add(spDate);

            StackPanel spWeatherDetails = CreateWeatherDetailsStackPanel(fullWeather.Current.Weather[0].Icon, fullWeather.Current.Weather[0].Description, fullWeather.Current.Temp, fullWeather.Current.Feels_like);

            spCurrent.Children.Add(spWeatherDetails);
        }

        private void CreateHourelyWeatherAndInsertToScreen()
        {
            spHourlyPanel.Children.Clear();

            StackPanel spStack = new StackPanel();
            spStack.Orientation = Orientation.Horizontal;
            spStack.Margin = new Thickness(10, 0, 10, 0);

            foreach (var hourWeather in fullWeather.Hourly)
            {
                StackPanel sp = new StackPanel();

                StackPanel spDate = CreateDateStackPanel(hourWeather.Dt);
                StackPanel spWeatherDetails = CreateWeatherDetailsStackPanel(hourWeather.Weather[0].Icon, hourWeather.Weather[0].Description, hourWeather.Temp, hourWeather.Feels_like);

                sp.Children.Add(spDate);
                sp.Children.Add(spWeatherDetails);

                spStack.Children.Add(sp);

                Rectangle rect = new Rectangle();
                rect.Width = 1;
                rect.Height = 100;
                rect.Fill = Brushes.Black;

                spStack.Children.Add(rect);
            }
            
            spHourlyPanel.Children.Add(spStack);
        }

        private void CreateDailyWeatherAndInsertToScreen()
        {
            spDailyPanel.Children.Clear();

            StackPanel spStack = new StackPanel();
            spStack.Orientation = Orientation.Horizontal;
            spStack.Margin = new Thickness(10, 0, 10, 0);

            foreach (var DayWeather in fullWeather.Daily)
            {
                StackPanel sp = new StackPanel();

                StackPanel spDate = CreateDateStackPanel(DayWeather.Dt);
                StackPanel spWeatherDetails = CreateWeatherDetailsStackPanel(DayWeather.Weather[0].Icon, DayWeather.Weather[0].Description, DayWeather.Temp, DayWeather.Feels_like);

                sp.Children.Add(spDate);
                sp.Children.Add(spWeatherDetails);

                spStack.Children.Add(sp);

                Rectangle rect = new Rectangle();
                rect.Width = 1;
                rect.Height = 100;
                rect.Fill = Brushes.Black;

                spStack.Children.Add(rect);
            }

            spDailyPanel.Children.Add(spStack);
        }

        private StackPanel CreateDateStackPanel(long dt)
        {
            StackPanel spDate = new StackPanel();
            spDate.Margin = new Thickness(10, 0, 10, 0);
            spDate.HorizontalAlignment = HorizontalAlignment.Center;

            TextBlock tbDate = new TextBlock();
            tbDate.FontSize = 15;
            tbDate.Margin = new Thickness(10, 0, 10, 0);
            TextBlock tbTime = new TextBlock();
            tbTime.FontSize = 15;
            tbTime.Margin = new Thickness(10, 0, 10, 0);

            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(dt).ToLocalTime();
            tbDate.Text = dtDateTime.Date.ToShortDateString();
            tbTime.Text = dtDateTime.TimeOfDay.ToString();

            spDate.Children.Add(tbDate);
            spDate.Children.Add(tbTime);

            return spDate;
        }

        private StackPanel CreateWeatherDetailsStackPanel(string icon, string description, double temp, double feelsLike)
        {
            StackPanel spWeatherDetails = new StackPanel();
            spWeatherDetails.Orientation = Orientation.Horizontal;
            spWeatherDetails.Margin = new Thickness(10, 0, 10, 0);
            spWeatherDetails.HorizontalAlignment = HorizontalAlignment.Center;

            StackPanel spWeatherDescription = CreateWeatherDescriptionStackPanel(icon, description);

            spWeatherDetails.Children.Add(spWeatherDescription);

            StackPanel spWeatherTemp = CreateWeatherTempStackPanel(temp, feelsLike);

            spWeatherDetails.Children.Add(spWeatherTemp);

            return spWeatherDetails;
        }

        private StackPanel CreateWeatherDetailsStackPanel(string icon, string description, AdvancedTempModel temp, BasicTempModel feelsLike)
        {
            StackPanel spWeatherDetails = new StackPanel();
            spWeatherDetails.Orientation = Orientation.Horizontal;
            spWeatherDetails.Margin = new Thickness(10, 0, 10, 0);
            spWeatherDetails.HorizontalAlignment = HorizontalAlignment.Center;

            StackPanel spWeatherDescription = CreateWeatherDescriptionStackPanel(icon, description);

            spWeatherDetails.Children.Add(spWeatherDescription);

            StackPanel spWeatherTemp = CreateWeatherTempStackPanel(temp, feelsLike);

            spWeatherDetails.Children.Add(spWeatherTemp);

            return spWeatherDetails;
        }

        private StackPanel CreateWeatherDescriptionStackPanel(string icon, string description)
        {
            StackPanel spWeatherDescription = new StackPanel();
            spWeatherDescription.Margin = new Thickness(10);

            Image imgIcon = new Image();
            imgIcon.Width = 50;
            imgIcon.Source = new BitmapImage(new Uri($"https://openweathermap.org/img/w/{icon}.png", UriKind.Absolute));
            TextBlock tbDescription = new TextBlock();
            tbDescription.FontSize = 15;
            tbDescription.Text = description;

            spWeatherDescription.Children.Add(imgIcon);
            spWeatherDescription.Children.Add(tbDescription);

            return spWeatherDescription;
        }

        private StackPanel CreateWeatherTempStackPanel(double temp, double feelsLike)
        {
            StackPanel spWeatherTemp = new StackPanel();
            spWeatherTemp.Margin = new Thickness(10);

            TextBlock tbTemp = new TextBlock();
            tbTemp.FontSize = 20;
            tbTemp.Text = "Temp: " + temp;
            TextBlock tbFeelsLike = new TextBlock();
            tbFeelsLike.FontSize = 20;
            tbFeelsLike.Text = "Feels like: " + feelsLike;

            spWeatherTemp.Children.Add(tbTemp);
            spWeatherTemp.Children.Add(tbFeelsLike);

            return spWeatherTemp;
        }

        private StackPanel CreateWeatherTempStackPanel(AdvancedTempModel temp, BasicTempModel feelsLike)
        {
            StackPanel spWeatherTemp = new StackPanel();
            spWeatherTemp.Margin = new Thickness(10);

            TextBlock tbTemp = new TextBlock();
            tbTemp.FontSize = 20;
            tbTemp.Text = "Temp: " + temp.Day;
            tbTemp.ToolTip = CreateTempToolTip(temp);
            TextBlock tbFeelsLike = new TextBlock();
            tbFeelsLike.FontSize = 20;
            tbFeelsLike.Text = "Feels like: " + feelsLike.Day;
            tbFeelsLike.ToolTip = CreateTempToolTip(feelsLike);

            spWeatherTemp.Children.Add(tbTemp);
            spWeatherTemp.Children.Add(tbFeelsLike);

            return spWeatherTemp;
        }

        private ToolTip CreateTempToolTip(BasicTempModel temp)
        {
            ToolTip tt = new ToolTip();

            StackPanel sp = CreateBasicTooltipStackPanel(temp.Morn, temp.Eve, temp.Night);

            tt.Content = sp;

            return tt;
        }

        private ToolTip CreateTempToolTip(AdvancedTempModel temp)
        {
            ToolTip tt = new ToolTip();

            StackPanel sp = CreateAdvancedTooltipStackPanel(temp.Morn, temp.Eve, temp.Night, temp.Min, temp.Max);

            tt.Content = sp;

            return tt;
        }

        private StackPanel CreateBasicTooltipStackPanel(double morning, double eve, double night)
        {
            StackPanel sp = new StackPanel();

            TextBlock tbMorning = new TextBlock();
            tbMorning.Text = "Morning: " + morning;
            TextBlock tbEvening = new TextBlock();
            tbEvening.Text = "Evening: " + eve;
            TextBlock tbNight = new TextBlock();
            tbNight.Text = "Night: " + night;

            sp.Children.Add(tbMorning);
            sp.Children.Add(tbEvening);
            sp.Children.Add(tbNight);

            return sp;
        }

        private StackPanel CreateAdvancedTooltipStackPanel(double morning, double eve, double night, double min, double max)
        {
            StackPanel sp = CreateBasicTooltipStackPanel(morning, eve, night);

            TextBlock tbMin = new TextBlock();
            tbMin.Text = "Min: " + min;
            TextBlock tbMax = new TextBlock();
            tbMax.Text = "Max: " + max;

            sp.Children.Add(tbMin);
            sp.Children.Add(tbMax);

            return sp;
        }

        private void RbCurrent_Checked(object sender, RoutedEventArgs e)
        {
            spCurrent.Visibility = Visibility.Visible;
            spHourly.Visibility = Visibility.Collapsed;
            spDaily.Visibility = Visibility.Collapsed;
            tbErrorMsg.Visibility = Visibility.Collapsed;
        }

        private void RbHourly_Checked(object sender, RoutedEventArgs e)
        {
            spCurrent.Visibility = Visibility.Collapsed;
            spHourly.Visibility = Visibility.Visible;
            spDaily.Visibility = Visibility.Collapsed;
            tbErrorMsg.Visibility = Visibility.Collapsed;
        }

        private void RbDaily_Checked(object sender, RoutedEventArgs e)
        {
            spCurrent.Visibility = Visibility.Collapsed;
            spHourly.Visibility = Visibility.Collapsed;
            spDaily.Visibility = Visibility.Visible;
            tbErrorMsg.Visibility = Visibility.Collapsed;
        }
    }
}
