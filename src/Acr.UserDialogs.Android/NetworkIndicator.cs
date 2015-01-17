using System;
using Android.App;
using Android.Views;


namespace Acr.UserDialogs {

    public class NetworkIndicator : IProgressIndicator {
        private readonly Activity activity;


        public NetworkIndicator(Activity activity) {
            this.activity = activity;
        }


        private int percentComplete;
        public int PercentComplete {
            get { return this.percentComplete; }
            set {
                if (this.percentComplete == value)
                    return;

                if (value > 100)
                    this.percentComplete = 100;

                else if (value < 0)
                    this.percentComplete = 0;

                else
                    this.percentComplete = value;

                ;
                this.activity.SetProgress(this.percentComplete);
            }
        }


        public bool IsDeterministic { get; set; }


        public bool IsShowing {
            get { return true; }
        }


        public void Show() {
            this.activity.RequestWindowFeature(WindowFeatures.Progress);
            this.activity.RequestWindowFeature(WindowFeatures.IndeterminateProgress);
            Utils.RequestMainThread(() => {
                //this.activity.SetProgress(0);
                //this.activity.SetProgressBarVisibility(true);
                //this.activity.SetProgressBarIndeterminateVisibility(true);
                //this.activity.SetProgressBarIndeterminate(true);
            });
        }


        public void Hide() {
        }


        public void Dispose() {
            this.Hide();
        }
    }
}

/*
public void onCreate(Bundle savedInstanceState) {
    super.onCreate(savedInstanceState);
    requestWindowFeature(Window.FEATURE_INDETERMINATE_PROGRESS);
    requestWindowFeature(Window.FEATURE_PROGRESS);
    currentURL = BrowserActivity.this.getIntent().getExtras().getString("currentURL");

    setContentView(R.layout.browser);

    setProgressBarIndeterminateVisibility(true);
    setProgressBarVisibility(true);

    try {
        mWebView = (WebView) findViewById(R.id.webview);
        mWebView.getSettings().setJavaScriptEnabled(true);
        mWebView.setWebViewClient(new browserActivityClient());

        mWebView.setWebChromeClient(new WebChromeClient() {
           public void onProgressChanged(WebView view, int progress) {
               setProgress(progress * 100);
              if(progress == 100) {
                 setProgressBarIndeterminateVisibility(false);
                 setProgressBarVisibility(false);
              }
           }
        });
        mWebView.loadUrl(currentURL);
    } catch (Exception e) {
        Log.e(getClass().getSimpleName(), "Browser: " + e.getMessage());
        Toast.makeText(this, e.getMessage(), Toast.LENGTH_LONG).show();
    } */