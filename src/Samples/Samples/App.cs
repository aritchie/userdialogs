using System;
using Xamarin.Forms;


namespace Samples {

    public class App : Application {
    
        public App() {
            this.MainPage = new ContentPage {
                Content = new StackLayout {
                    VerticalOptions = LayoutOptions.Center,
                    Children = {
						new Label {
							XAlign = TextAlignment.Center,
							Text = "Welcome to Xamarin Forms!"
						}
					}
                }
            };
        }

        protected override void OnStart() {}
        protected override void OnSleep() {}
        protected override void OnResume() {}
    }
}
