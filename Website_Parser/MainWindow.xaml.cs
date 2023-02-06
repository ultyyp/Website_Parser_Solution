
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
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
using AngleSharp.Dom;
using ProglibIO;

namespace Website_Parser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            StatusLabel.Content = "Extracting...";
            Stopwatch sw = Stopwatch.StartNew();
            ConcurrentBag<Vacancy> concBag = await ProglibIO.Functions.GetVacanciesFromAllPagesAsync();

            foreach (var vac in concBag)
            {
                VacancyGrid.Items.Add(vac);
            }

            sw.Stop();
            StatusLabel.Content = "Done! Elapsed: " + sw.Elapsed.ToString();
            AmmountLabel.Content = "Vacancies: " + VacancyGrid.Items.Count.ToString();
        }

        private void VacancyGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var vacancy = (Vacancy)VacancyGrid.SelectedItem;
            OpenUrl(vacancy.VacancyURL);
        }


        private void OpenUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
