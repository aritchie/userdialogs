# Progress
Interdeterminate (Loading) and Deterministic Progress ()

## Show/Hiding Loading vs Loading
ShowLoading and HideLoading are considered deprecated.

You should use the following pattern:

using (UserDialogs.Instance.Loading())
{
    // do async things here and await them
} // dialog will close here, if something crashes in the using statement, it will still cleanup


If you need to control show/hide, you can also do:

// store this somewhere
var loading = UserDialogs.Instance.Loading();


loading.Dispose(); // cleanup


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
