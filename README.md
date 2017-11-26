# XFSIP (Xamarin Forms Signin Prototype)
This is a sample app using Xamarin Forms to mimic a signin page.  The code is non-functional in the 
sense that it doesn't sign in to any service.  It is functional in the sense that it demonstrates
how to do several things in Xamarin Forms.

* Setup a basic cross platform solution with Android, iOs, UWM, and PCL projects
* Create the basic view in xaml (the fake signin page)
* Add an android-specific splash screen (I don't have a mac for iOS and UWP is super easy in the VS UI)
* RESX support to load strings from a resource based on locale (translation support)
* Implement a simple MVVM system where...
  * the view (xaml) is bound to a new viewmodel class
  * the VM implements a command interface for the view button
  * This information is essentially the Model here (username:password)
  * ViewModel uses this object to "signin" (mocked here)
    * If signin fails, show an error on the signin page
    * On Success (randy:123), navigate to a simple landing page with a new view/viewmodel
        
### Environment
Visual Studio Community 2017 with the Xamarin components installed and using the Android_Accelerated_Nougat 
(Android 7.1 - API 25) emulator.  If you can get the default project ("Welcome to Xamarin.Forms!") to build
and deploy you should be good.  Otherwise consult [this](https://developer.xamarin.com/guides/cross-platform/getting_started/).

### Signin MVVM (Model/View/ViewModel)
This view consists of several labels and entry controls and one button.  The password entry is marked as password and will 
display obscuring characters on the OS (e.g. little dots or stars).  Every control is bound to a Property in the ViewModel, 
and the XAML demonstrates how to set up this binding.  Static labels are bound to localizable RESX resoruces. **No Display
Text Should Be Hardcoded Ever**

If you enter anything other than "randy:123" as the username:password, then Signin will fail and an error message will display
in the view (IsVisible Property is bound to the ViewModel).  This also demonstrates how to propagate changes back by changing
these properties in code (clearing the error message on retry).

If you did enter "randy:123" the Signin MVVM will navigate (in the view model) to the only other MVVM in the prototype.

### UserLandingPage MVVM
This attempts to demonstrate a simple stack based navigation model.  It is very bare bones, but demonstrates a few interesting
things.  

* Creating a ViewModel and binding it at runtime rather than in XAML as with Signin 
  * Creation and binding is actually in the SigninViewModel (acts upon the new one)
* Getting a localized string in code rather than binding
* If the ViewModel Is Ignorant of the View, How Can It Invoke Nagivation?
  * A: It can't really, but it can hold it as a bound Property and remain mostly agnostic of it
* Removing the NavigationBar and BackButton on the screen.  The hardware back still works.
  * This is something that likely needs OnPlatform<> or will break on iOS but I can't test it
* There is no formal Model here as wrapping that one formatted string would be ridiculous.






