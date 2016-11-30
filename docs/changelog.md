# CHANGELOG

## 6.3.2
* [fix][ios] OnTextChanged event hooks were not working properly

## 6.3.1
* [fix][droid] NRE on OnTextChanged

## 6.3.0
* [fix][ios] prompt maxlength was preventing character deletion
* [feature] [all] PromptConfig.OnTextChanged action adds the ability to disable positive button as well as change current textbox value (great for formatting)
* [internal] refactoring internal android logic for dialog management

## 6.2.## 6
* [fix] [all] cancellation token memory leak (thanks to @smstuebe)
* [update] allow updates to google libs

## 6.2.5
* [fix] [all] default toast colours were not being used

## 6.2.4
* [feature] [ios] [uwp] You can now add a message to actionsheet (android coming soon)
* [fix] [droid] allow negatives for decimal input type on prompt
* [fix] [droid] backspace key not working on login dialog with appcompat mode
* [fix] [uwp] Text wrapping on prompt window
* [fix] [uwp] Text wrapping on toasts
* [fix] [uwp] Make loading work better with themes

## 6.2.3
* [fix] [droid] NRE on fallback toast click (when not in use)
* [fix] [uwp] deterministic progress dialog min and max set properly

## 6.2.2
* [fix] [uwp] progress indicator was not bound properly

## 6.2.1
* [fix] [ios] NRE on maxlength prompt
* [fix] [uwp] NRE on progress
* [fix] [uwp] NRE on prompt placeholder
* [fix] [uwp] prompt no longer requires Message property be set in the config

## 6.2.0
* [feature] maxlength for prompt dialog
* [feature] showloading can be called even when stoploading was not called
* [feature] onaction/async cannot be set together.  plugin will now throw error
* [feature] [droid] actionsheet is now dismissable by touching outside of the dialog
* [fix] [droid] date/time dialogs should not show keyboard
* [fix] [ios] alert dialogs and toasts all marshall back to main thread now
* [fix] [ios] time dialog is not using selected time
* [fix] [uwp] progress dialog showing percent even when indeterministic
* [fix] [uwp] fix toast from background thread

## 6.1.2
* [fix] [droid] prompt dialog with numeric restriction was not allowing input

## 6.1.0
* [BREAKING] all config objects now use a consistent naming for OnEvent called OnAction instead of OnOk, OnConfirm, OnEtc
* [BREAKING] progress dialog interface has changed ever so slightly
* [BREAKING] winphone 8 has been removed from the nuget package - it still exists on github!
* [feature] [ios] Add optional Init function to allow for top viewcontroller control similar to android top activity - this is good for extensions apparently (Thanks to Kyle Spearrin)
* [fix] [ios] showerror/success images not being displayed
* [fix] [ios] date/time dialogs no longer show a solid black background
* [fix] [droid] snackbar font color works now
* [fix] [droid] nre on unassigned toast action
* [fix] [droid] back button does cancels dialogs if dialog is cancellable
* [fix] [droid] prompt dialog now uses enter as ok click
* [fix] [uwp] show error/success now uses toasts
* [feature] ShowLoading will now close previous instances that were not closed (stupid proofing)
* [feature] add SelectedDate parameter to DatePromptAsync
* [feature] add SelectedTime parameter to TimePromptAsync

## 6.0.1
* [fix] [ios] date/time picker not returning proper value

## 6.0
This is a new version due to breaking changes

* [BREAKING] [feature] [ios] New snackbar for old toast setup that everyone complained about (thanks to Marc Bruins - https://github.com/MarcBruins)
* [feature] [ios] Better looking date/time picker (thanks to Marc Bruins - https://github.com/MarcBruins)
* [feature] Toasts are also dismissable from code now (via disposable)
* [feature] [droid] Make actionsheets look like bottomsheets from the design library - Set ActionSheetConfig.UseBottomSheet = true
* [feature] [droid] Ability to set style on alert based dialogs using AndroidStyleId
* [fix] [ios] destruction button should be at the top on iOS actionsheets
* [fix] [droid] fixes for fragment dialogs
* [fix] [droid] classic (non-appcompat) dialogs would not always appear from background threads