# Toasts


## Config
```csharp
var toastConfig = new ToastConfig("Toasting...");
toastConfig.SetDuration(3000);
toastConfig.SetBackgroundColor(System.Drawing.Color.FromArgb(12, 131, 193));

UserDialogs.Instance.Toast(toastConfig);
```

## Themeing/Defaults
```csharp
TimeSpan DefaultDuration { get; set; } = TimeSpan.FromSeconds(2.5);
Color? DefaultMessageTextColor { get; set; }
Color? DefaultActionTextColor { get; set; }
Color? DefaultBackgroundColor { get; set; }
```
