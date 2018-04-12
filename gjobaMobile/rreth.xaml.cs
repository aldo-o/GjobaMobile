using gjobaMobile.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
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


namespace gjobaMobile
{
    public sealed partial class rreth : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public rreth()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            var version = Package.Current.Id.Version;
            string appVersion = String.Format("v{0}.{1}.{2}.{3}",
                version.Major, version.Minor, version.Build, version.Revision);

            info.Text = appVersion + "\n\n\nGjoba Mobile është një aplikacion e cila ju lejon të kontrolloni gjobat e makinave tuaja nga celulari.\nSHËNIM: Ky alikacion nuk është versioni Windows Phone i aplikacionit \"Komisariati Dixhital\" krijuar nga Ministria e Punëve të Brendshme dhe Policia e Shtetit dhe nuk ka lidhje aspak me këto institucione.\n\n\n• Pagesa bëhet brënda 5 diteve kur merr formë të prerë. Me kalimin e afatit paguhet 2%.\n\n• Për pagesat brenda 5 ditëve bëhet 20% zbritje në vlerën e kundravajtjes.\n\n• Për çdo shqetësim apo ankesë qytetarët mund të komunikojnë dhe në adresat e-mail: policiaeshtetit@asp.gov.al apo policiarrugore@asp.gov.al\n\n• Ky sistem bën të mundur verifikimin e detyrimeve të pashlyera që qytetari ka në të gjithë Shqipërinë.";
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

        private void mesazh_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(kontakt));
        }
    }
}
