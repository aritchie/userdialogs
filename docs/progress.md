# Progress

```cs
Acr.UserDialogs.UserDialogs.Instance.ShowLoading
  ( title: "Loading message"
  );

// Do some work

Acr.UserDialogs.UserDialogs.Instance.HideLoading();
```

## Config
     public static string DefaultCancelText { get; set; } = "Cancel";
        public static string DefaultTitle { get; set; } = "Loading";
        public static MaskType DefaultMaskType { get; set; } = MaskType.Black;


        public string CancelText { get; set; }
        public string Title { get; set; }
        public bool AutoShow { get; set; }
        public bool IsDeterministic { get; set; }
        public MaskType MaskType { get; set; }
        public Action OnCancel { get; set; }
