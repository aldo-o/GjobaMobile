using gjobaMobile.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace gjobaMobile
{
    public class PassedData
    {
        public string totalGjoba { get; set; }
        public string shumaGjoba { get; set; }
        public string shkeljetGjoba { get; set; }
        public string pershkrimetGjoba { get; set; }
    }

    //public class Gjobat
    //{
    //    public int TotalFines { get; set; }
    //    public int TotalValue { get; set; }
    //    public List<object> Violations { get; set; }
    //}

    public sealed partial class BasicPage1 : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        public BasicPage1()
        {
            this.InitializeComponent();

            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;

            try
            {
                targa.Text = localSettings.Values["targa"].ToString();
                shasia.Text = localSettings.Values["shasia"].ToString();
            }
            catch
            {

            }

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        string nrGjoba, vlera, shkelje, pershkrimet;
        private void kontrollo_Click(object sender, RoutedEventArgs e)
        {
            shfaq();
        }

        public async void kerko(string targa, string shasia)
        {
            try
            {
                var searchUri = "http://www.asp.gov.al/index.php/sherbime/kontrolloni-gjobat-tuaja";
                var searchParameters = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("plate", targa.ToUpper()),
                    new KeyValuePair<string, string>("vin", shasia.ToUpper()) };

                using (var httpclient = new HttpClient())
                {
                    using (var content = new FormUrlEncodedContent(searchParameters))
                    {
                        using (var responseMessages = await httpclient.PostAsync(new Uri(searchUri, UriKind.Absolute), content))
                        {
                            string response = await responseMessages.Content.ReadAsStringAsync();

                            int kerkoNrGjoba = response.IndexOf("NR.GJOBAVE");
                            nrGjoba = response.Substring(kerkoNrGjoba + 46, 5);
                            nrGjoba = Regex.Match(nrGjoba, @"\d+").Value;

                            int kerkoVlera = response.IndexOf("VLERA TOTAL");
                            vlera = response.Substring(kerkoVlera + 47, 40);
                            vlera = vlera.Replace(",", "");
                            vlera = Regex.Match(vlera, @"\d+").Value;

                            int kerkoShkelje = response.IndexOf("SHKELJET");
                            shkelje = response.Substring(kerkoShkelje + 40, 100);
                            shkelje = filtroString(shkelje, "e;'>", "</td>").Replace(",", "").Replace(".", "").Replace("/", "").Replace(">", "");

                            int KerkoPershkrime = response.IndexOf("PERSHKRIMET");
                            pershkrimet = response.Substring(KerkoPershkrime + 40, 400);
                            pershkrimet = filtroString(pershkrimet, "blue;'>", "</td>");

                            if (response.IndexOf("Kombinimi targe") == -1)//Kerkimi behet
                            {
                                pr.IsActive = false;

                                Frame.Navigate(typeof(gjobat), new PassedData
                                {
                                    totalGjoba = nrGjoba,
                                    shumaGjoba = vlera,
                                    shkeljetGjoba = shkelje,
                                    pershkrimetGjoba = pershkrimet
                                });
                            }
                            else
                            {
                                pr.IsActive = false;
                                MessageDialog mesazh1 = new MessageDialog("Të dhënat jane nuk janë të sakta ose sistemi nuk është në funksionim për momentin!");
                                mesazh1.Title = "GABIM!";
                                mesazh1.Commands.Add(new UICommand { Label = "Mbyll" });
                                await mesazh1.ShowAsync();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                pr.IsActive = false;
                MessageDialog mesazh1 = new MessageDialog("Të dhënat jane nuk janë të sakta ose sistemi nuk është në funksionim për momentin!");
                mesazh1.Title = "GABIM!";
                mesazh1.Commands.Add(new UICommand { Label = "Mbyll" });
                mesazh1.ShowAsync();
            }
        }

        public static string filtroString(string strSource, string fillim, string fund)
        {
            int f, m;
            if (strSource.Contains(fillim) && strSource.Contains(fund))
            {
                f = strSource.IndexOf(fillim, 0) + fillim.Length;
                m = strSource.IndexOf(fund, f);
                return strSource.Substring(f, m - f);
            }
            else
            {
                return "";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            targa.Text = string.Empty;
            shasia.Text = string.Empty;
        }

        private void keyDown(object sender, KeyRoutedEventArgs e)
        {
            //shfaq();
            //LoseFocus(targa);
            //(shasia);
        }
        private void LoseFocus(object sender)
        {
            var control = sender as Control;
            var isTabStop = control.IsTabStop;
            control.IsTabStop = false;
            control.IsEnabled = false;
            control.IsEnabled = true;
            control.IsTabStop = isTabStop;
        }
        private void LooseFocusOnEnter(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                e.Handled = true;
                LoseFocus(sender);
            }
        }

        private async void shfaq()
        {
            if (!string.IsNullOrEmpty(targa.Text) && !string.IsNullOrEmpty(shasia.Text))
            {
                try
                {
                    pr.IsActive = true;

                    localSettings.Values["targa"] = targa.Text.ToUpper().Replace(" ", "");
                    localSettings.Values["shasia"] = shasia.Text.ToUpper().Replace(" ", "");

                    kerko(targa.Text, shasia.Text);

                    //int i = 0;
                    //while (string.IsNullOrEmpty(nrGjoba))
                    //{
                    //    await Task.Delay(1000);
                    //    i++;
                    //    if(i >= 5)
                    //    {
                    //        pr.IsActive = false;
                    //        MessageDialog mesazh1 = new MessageDialog("Të dhënat jane nuk janë të sakta ose sistemi nuk është në funksionim për momentin!");
                    //        mesazh1.ShowAsync();
                    //    }
                    //}
                }
                catch (Exception ex)
                {
                    pr.IsActive = false;
                    MessageDialog mesazh1 = new MessageDialog("Të dhënat jane nuk janë të sakta ose sistemi nuk është në funksionim për momentin!");
                    mesazh1.Title = "GABIM!";
                    mesazh1.Commands.Add(new UICommand { Label = "Mbyll" });
                    mesazh1.ShowAsync();
                }
            }
            else
            {
                MessageDialog mesazh1 = new MessageDialog("Plotësoni targën dhe shasinë!");
                mesazh1.Title = "GABIM!";
                mesazh1.Commands.Add(new UICommand { Label = "Mbyll" });
                await mesazh1.ShowAsync();
            }
        }

        private void Shto_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(shto));
        }

        private void liste_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(shfaqMakinat));
        }

        private void rreth_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(rreth));
        }

        private void kontakt_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(kontakt));
        }

        //async private void MerrTeDhenat()
        //{
        //    var client = new HttpClient(); // Add: using System.Net.Http;
        //    var response = await client.GetAsync(new Uri("http://gjobat.digi-travel.com/getfines.aspx?plate=" + targa.Text.ToUpper() + "&vin=" + shasia.Text.ToUpper() + ""));
        //    var result = await response.Content.ReadAsStringAsync();

        //    gjobat = result.Replace("\"", "'");
        //}

        //private void json()
        //{
        //    pr.IsActive = true;

        //    localSettings.Values["targa"] = targa.Text.ToUpper();
        //    localSettings.Values["shasia"] = shasia.Text.ToUpper();

        //    MerrTeDhenat();

        //    while (string.IsNullOrEmpty(gjobat))
        //    {
        //        await Task.Delay(1000);
        //    }

        //    Gjobat account = JsonConvert.DeserializeObject<Gjobat>(gjobat);
        //    string total = account.TotalFines.ToString();
        //    string shuma = account.TotalValue.ToString();

        //    pr.IsActive = false;

        //    Frame.Navigate(typeof(gjobat), new PassedData { totalGjoba = total, shumaGjoba = shuma });
        //}
    }
}
