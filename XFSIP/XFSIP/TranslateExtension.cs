using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

// Taken from https://developer.xamarin.com/guides/xamarin-forms/advanced/localization/
namespace XFSIP
{
    // You exclude the 'Extension' suffix when using in Xaml markup
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        readonly CultureInfo ci = null;
        const string ResourceId = "XFSIP.Resx.AppResources";

        // cache this as it will get called for every string resource
        private static readonly Lazy<ResourceManager> ResMgr = 
            new Lazy<ResourceManager>(() => new ResourceManager(ResourceId, typeof(TranslateExtension).GetTypeInfo().Assembly));

        public TranslateExtension()
        {
            // Android and iOS require platform specific code to get the culture info.  The
            // DependencyService will fethch the correct interface implementation at runtime
            if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
            {
                // This now invoke platform specific code to get the culture info
                ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            }
        }

        /// <summary>
        /// The ResourceId from the XAML e.g. 'NavigateTitle' in Title="{i18n:Translate NavigateTitle}"
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The only IMarkupExtension method, it is used to return the translated value of the resource
        /// identified by the 'Text' property (read from the XAML)
        /// </summary>
        /// <param name="serviceProvider">ignored</param>
        /// <returns>The translated string</returns>
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            // If there is no id, there is no translation
            if (Text == null)
                return "";

            // Invoke the resource manager (or get cached) and load the resource
            // from the assembly for the current CultureInfo
            var translation = ResMgr.Value.GetString(Text, ci);

            // The translation is missing (it should have fallen back to the
            // default base language even if not translated) this is a build error
            // (missing resource)
            if (translation == null)
            {
#if DEBUG
                throw new ArgumentException(
                    String.Format("Key '{0}' was not found in resources '{1}' for culture '{2}'.", Text, ResourceId, ci.Name),
                    "Text");
#else
                translation = Text; // returns the key, which GETS DISPLAYED TO THE USER
#endif
            }
            return translation;
        }
    }
}
