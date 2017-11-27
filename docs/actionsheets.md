# ActionSheets


## Config

## Theming/Defaults

- ActionSheetConfig
    - DefaultAndroidStyleId
    - DefaultCancelText
    - DefaultDestructiveText
    - DefaultItemIcon
    
## Example

    IUserDialogs dialogs = UserDialogs.Instance;
    var action = await dialogs.ActionSheetAsync("Choose an option", "Cancel", null,
                                                buttons: new string[] {
                                                    "Take a picture", 
                                                    "Select a photo from library"
                                                });
