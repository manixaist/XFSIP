using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.ComponentModel;
using Xamarin.Forms;
using XFSIP.Models;

namespace XFSIP.ViewModels
{
    /// <summary>
    /// ViewModel for the Signin Page.  It is responsible for responding to commands
    /// (only one from the button) and can be data-bound to the user and password
    /// fields in the view (without knowledge of that code/xaml here)
    /// </summary>
    class SigninViewModel : INotifyPropertyChanged
    {
        // Items bound to us will add to the event chain
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Public parameterless constructor is required by xaml
        /// </summary>
        public SigninViewModel()
        {
        }

        /// <summary>
        /// Exposes the UserSigninInfo object as a Property
        /// </summary>
        public UserSigninInfo UserSigninInfo
        {
            get
            {
                // This can be bound to as it is a public property, and the
                // view (xaml) does just that in this example project
                return userSigninInfo;
            }

            // This does not get invoked from the view (or anywhere currently)
            // but if the code needed to set a new object, this would trigger
            // the update back to the view.  So it's really just example code here
            set
            {
                userSigninInfo = value;
                OnPropertyChanged(); // It changed, so fire an update
            }
        }

        /// <summary>
        /// Exposes a bool that is true if the form is waiting for submission
        /// </summary>
        public bool WaitingForSubmit
        {
            get
            {
                return waitingForSubmit;
            }

            // This one is set in code, so it will go back to the View
            set
            {
                // Only update this if it changed, since it will update the
                // view binding(s)
                if (waitingForSubmit != value)
                {
                    waitingForSubmit = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Exposes a bool that is true if the form is in an error state
        /// (incorrect user:password given)
        /// </summary>
        public bool IsErrorState
        {
            get
            {
                return isErrorState;
            }

            set
            {
                // Only update this if it changed, since it will update the
                // view binding(s)
                if (isErrorState != value)
                {
                    isErrorState = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Exposed command, which again helps abstract the view from the viewmodel
        /// This is bound to the button command handler in xaml (but this code doesn't
        /// have to know or care about who invokes this command or how)
        /// </summary>
        public Command SigninCommand => new Command(async () => // Execute
        {
            // Print the values we have in the debugger.
            Debug.WriteLine(String.Format("Bindings=> UserSigninInfo: {0}, Password: {1}",
                                                    userSigninInfo.Username, userSigninInfo.Password));

            // The page is about to submit data (bound to button enabled state in example)
            WaitingForSubmit = false;

            // Reset error state on new attempt
            IsErrorState = false;

            // "Signin" to our service
            int result = await DoFakeSigninAsync(userSigninInfo.Username, userSigninInfo.Password);

            // Success or fail we're done signing in
            WaitingForSubmit = true;

            // Set error state
            IsErrorState = (result != 1);

            // Navigate based on result (or show error)
            Debug.WriteLine(String.Format("DoFakeSigninAsync => {0}",result));

        }, () => // CanExecute
        {
            // In case a button masher slips another command in 
            return waitingForSubmit == true;
        });

        // This is the object that backs the public property that is bound to in the view
        private UserSigninInfo userSigninInfo = new UserSigninInfo();

        // This is bound to the enabled state of the button
        private bool waitingForSubmit = true;

        // bound to error text
        private bool isErrorState = false;

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

        /// <summary>
        /// Simulate a signin over the wire.  An artificial async delay is added and there
        /// is a hardcoded user/pass for success
        /// </summary>
        /// <param name="username">username to use for logon</param>
        /// <param name="password">password to use for logon</param>
        /// <returns></returns>
        private static async Task<int> DoFakeSigninAsync(string username, string password)
        {
            // Capture the local value and not just the reference
            UserSigninInfo localSigninInfo = new UserSigninInfo() { Username = username, Password = password };
            return await Task<int>.Run(async () => 
            {
                // Make sure this data is getting to the task, use the local value within here
                Debug.WriteLine(String.Format("AsyncTask=> UserSigninInfo: {0}, Password: {1}",
                                                        localSigninInfo.Username, localSigninInfo.Password));
                int result = 0;
                // Simulate delay over wire
                await Task.Delay(5000);

                if (localSigninInfo.Username == "randy" && localSigninInfo.Password == "123")
                {
                    result = 1; // Success
                }

                return result;
            });
        }
    }
}
