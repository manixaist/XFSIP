using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;  // CallerMemberName attribute
using System.ComponentModel;            // INotifyPropertyChanged
using Xamarin.Forms;
using XFSIP.Resx;

namespace XFSIP.ViewModels
{
    /// <summary>
    /// ViewModel for the User Landing Page (simple display of user name welcome)
    /// </summary>
    class UserLandingViewModel : INotifyPropertyChanged
    {
        // Items bound to us will add to the event chain
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Creates a new simple view model for the landing page.
        /// </summary>
        /// <param name="userName">Username that just signed in</param>
        public UserLandingViewModel(string userName)
        {
            // Use the localized RESX library for the format, and insert the username
            welcomeMessage = String.Format(AppResources.UserLandingDisplayFormat, userName);
        }

        /// <summary>
        /// Exposed for binding the display welcome message
        /// </summary>
        public string WelcomeMessage { get { return welcomeMessage; } }

        /// <summary>
        /// Fires the PropertyChanged event for INotifyPropertyChanged implementation
        /// </summary>
        /// <param name="propertyName">Property that changed</param>
        /// <remarks>CallerMemberName attribute incurs no performance penalty</remarks>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Only do this if someone has asked for it (otherwise the event will be null)
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>Cache of the formatted welcome message</summary>
        private readonly string welcomeMessage = null;
    }
}
