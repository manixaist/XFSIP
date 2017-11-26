using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

// https://developer.xamarin.com/guides/xamarin-forms/xaml/xamlc/
// XAMLC offers a number of a benefits:
// It performs compile-time checking of XAML, notifying the user of any errors.
// It removes some of the load and instantiation time for XAML elements.
// It helps to reduce the file size of the final assembly by no longer including .xaml files.
[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace XFSIP
{
    public partial class App : Application
    {
        public App()
        {

#if DEBUG
            System.Diagnostics.Debug.WriteLine("====== resource debug info =========");
            var assembly = typeof(App).GetTypeInfo().Assembly;
            foreach (var res in assembly.GetManifestResourceNames())
                System.Diagnostics.Debug.WriteLine("found resource: " + res);
            System.Diagnostics.Debug.WriteLine("====================================");
#endif

            // This lookup NOT required for Windows platforms - the Culture will be automatically set
            if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
            {
                // determine the correct, supported .NET culture on the platform at runtime
                // This is acheived via the DependencyService, which can invoke declared interfaces
                // to the runtime implemenation, and that code should implement the interface
                // and return a result if needed, and here it is needed.
                // ILocalize.cs defines this interface, and Localize.cs in android and iOS implement it
                var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
                Resx.AppResources.Culture = ci; // set the RESX for resource localization
                DependencyService.Get<ILocalize>().SetLocale(ci); // set the Thread for locale-aware methods
            }

            // Now that the CultureInfo is set, load the XAML (generated CS code)
            InitializeComponent();

            // Create the main page (signin)
            // MainPage here is a property on the Xamarin.Forms.Application base object
            // Setting the SigninPage to this makes it the 'main' page or starting page
            // This needs to be a 'NavigationPage' on Android as Android doesn't support
            // PushAsync on all devices the same (typical)
            MainPage = new NavigationPage(new XFSIP.Views.SigninPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
