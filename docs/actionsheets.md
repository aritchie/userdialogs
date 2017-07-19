# ActionSheets
| iOS | Android |
| :-: | :-: |
| <img src="http://i.imgur.com/uy770tw.gif" width="370" height="644"/> | <img src="http://i.imgur.com/5feffQq.gif" width="370" height="636"/> |

Image icons made by [Chanut is Industries](http://www.flaticon.com/authors/chanut-is-industries)

## Config
```csharp
var actionSheetConfig = new ActionSheetConfig();
actionSheetConfig.SetTitle("Title");
actionSheetConfig.SetMessage("Message");
actionSheetConfig.SetUseBottomSheet(true);

// Add items and images
IBitmap itemImage = null;
try
{
    itemImage = BitmapLoader.Current.LoadFromResource("myImage.png", null, null).Result;
}
catch { }
actionSheetConfig.Add("itemName", null, itemImage);

actionSheetConfig.SetDestructive("Destructive", null, itemImage);
actionSheetConfig.SetCancel("Cancel", null, itemImage);

UserDialogs.Instance.ActionSheet(actionSheetConfig);
```

## Theming/Defaults

- ActionSheetConfig
    - DefaultAndroidStyleId
    - DefaultCancelText
    - DefaultDestructiveText
    - DefaultItemIcon
