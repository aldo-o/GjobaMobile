using gjobaMobile.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SQLite;
using Windows.UI.Popups;
using System.Diagnostics;


namespace gjobaMobile
{
    public sealed partial class shto : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        DatabaseHelperClass Db_Helper = new DatabaseHelperClass();

        public shto()
        {
            this.InitializeComponent();

            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;
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

        private void pas_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(BasicPage1));
        }

        private async void ruaj_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tbMakina.Text) &&
                !string.IsNullOrEmpty(tbTarga.Text) &&
                !string.IsNullOrEmpty(tbShasia.Text))
            {
                Db_Helper.Insert(new Tabela.makinat(tbMakina.Text, tbTarga.Text.ToUpper(), tbShasia.Text.ToUpper()));
                Frame.Navigate(typeof(shfaqMakinat));
            }
            else
            {
                MessageDialog mesazh = new MessageDialog("Ju lutem, plotësoni fushat!");
                await mesazh.ShowAsync();
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

        private void enter_click(object sender, KeyRoutedEventArgs e)
        {
            
        }
    }
}
