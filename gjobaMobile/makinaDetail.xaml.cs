using gjobaMobile.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
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
    public sealed partial class makinaDetail : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public makinaDetail()
        {
            this.InitializeComponent();

            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;
            shenim.Text = "SHËNIM:\nNëse ju jeni gjobitur nga policia dhe nuk ju shfaqet në program, nuk është gabim i programit, por gjoba juaj nuk ju është hedhur akoma në sistem.";

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

        DatabaseHelperClass Db_Helper;
        int SelectedContactID;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);

            Db_Helper = new DatabaseHelperClass();

            SelectedContactID = int.Parse(e.Parameter.ToString());
            Tabela.makinat m = new Tabela.makinat();
            m = Db_Helper.ReadContact(SelectedContactID);

            LBLmakina.Text = m.sqMakina;
            LBLtarga.Text = m.sqTarga;
            LBLshasia.Text = m.sqShasia;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void pas_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(shfaqMakinat));
        }

        private void kontrollo_Click(object sender, RoutedEventArgs e)
        {
            shfaq();
        }

        private void fshi_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog mesazh1 = new MessageDialog("A jeni i sigurt që dëshironi ta fshini makinën nga lista?");
            mesazh1.Title = "KUJDES!";
            mesazh1.Commands.Add(new UICommand("Po", new UICommandInvokedHandler(this.fshi)));
            mesazh1.Commands.Add(new UICommand("Jo"));
            mesazh1.ShowAsync();
        }
        private void fshi(IUICommand command)
        {
            Db_Helper.DeleteContact(SelectedContactID);
            Frame.Navigate(typeof(shfaqMakinat));
        }

        string nrGjoba, vlera, shkelje, pershkrimet;
        private async void shfaq()
        {
            if (!string.IsNullOrEmpty(LBLtarga.Text) && !string.IsNullOrEmpty(LBLshasia.Text))
            {
                try
                {
                    pr.IsActive = true;

                    kerko(LBLtarga.Text, LBLshasia.Text);

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

                            if (response.IndexOf("Kombinimi targe") == -1)
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

        private void rreth_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(rreth));
        }

        private void kontakt_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(kontakt));
        }
    }
}
