# Frequently Asked Questions

1. I'm getting a nullreferenceexception when using loading.
    * This happens when you run loading (or almost any dialog) from the constructor of your page or viewmodel.  The view hasn't been rendered yet, therefore there is nothing to render to.

2. I'm getting "This is the PCL library, not the platform library.  Did you include the nuget package in your main "executable" project?"
    * Do exactly what it says

3. Navigating while inside of a loading/progress dialog causes exceptions or the progress no longer appears properly
    * Hide the progress dialog before navigating

4. I don't like the way X method works on platform Y
    * No problems.  Override the implementation like below


    on the platform
    public class MyCustomUserDialogs : Acr.UserDialogs.UserDialogImpl {
            public override ..
    }

    in appdelegate or the starting activity
    UserDialogs.Instance = new MyCustomUserDialogs();

5. Why don't you support the latest Android support libraries?

    * Because Xamarin breaks these frequently, one way or another - every... single... major release.  Be patient and wait!

6. Why don't you cancel a dialog when the app goes to the background (AND) why do I get an exception when I call for a dialog?

    * USER DIALOGS DOES NOT SOLVE WORLD PEACE! Guess what - most android API version and iOS don't call this.  This library is not a window state manager, if you call for a dialog, 
        it will try to present one.  If your app goes to the background and you call for a dialog, iOS & Android are tossing you the exception.  The library isn't here to save you from bad design choices.  
        Call us an anti-pattern if you want, we present dialogs!

7. Why does the library allow me to open multiple windows?

    * Similar to #6 - the library does not manage windows.  It opens dialogs - SURPRISE