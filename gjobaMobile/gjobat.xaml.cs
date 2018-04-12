using gjobaMobile.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Phone.UI.Input;
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
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class gjobat : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        public gjobat()
        {
            this.InitializeComponent();

            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            try
            {
                if (localSettings.Values["mesazh"].ToString() != "ok")
                {
                    MessageDialog mesazh1 = new MessageDialog("Nëse ju jeni gjobitur nga policia dhe nuk ju shfaqet në program, nuk është gabim i programit, por gjoba juaj nuk ju është hedhur akoma në sistem.");
                    mesazh1.Title = "SHËNIM!";
                    mesazh1.Commands.Add(new UICommand("Mbyll", new UICommandInvokedHandler(this.ls)));
                    mesazh1.ShowAsync();
                }
            }
            catch
            {

            }
        }

        private void ls(IUICommand command)
        {
            localSettings.Values["mesazh"] = "ok";
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);

            PassedData data = e.Parameter as PassedData;
            total.Text = data.totalGjoba;
            shuma.Text = data.shumaGjoba;
            shkeljet.Text = data.shkeljetGjoba;
            pershkrime.Text = data.pershkrimetGjoba;
            pershkrime.TextWrapping = TextWrapping.Wrap;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void pas_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(BasicPage1));
        }

        private void rreth_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(rreth));
        }

        private void kontakt_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(kontakt));
        }

        private void list_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(shfaqMakinat));
        }

        private void ruaj_Click(object sender, RoutedEventArgs e)
        {
            DatabaseHelperClass Db_Helper = new DatabaseHelperClass();
            Db_Helper.Insert(new Tabela.makinat("Auto", 
                localSettings.Values["targa"].ToString(),
                localSettings.Values["shasia"].ToString()));
            Frame.Navigate(typeof(shfaqMakinat));
        }

    }
}
