using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Coding4Fun.Toolkit.Controls;


namespace Acr.UserDialogs
{
    public class UserDialogsImpl : AbstractUserDialogs
    {
        public override IDisposable Alert(AlertConfig config)
        {
            var dialog = new MessageDialog(config.Message, config.Title ?? String.Empty);
            dialog.Commands.Add(new UICommand(config.OkText, x => config.OnAction?.Invoke()));
            IAsyncOperation<IUICommand> dialogTask = null;

            return this.DispatchAndDispose(
                () => dialogTask = dialog.ShowAsync(),
                () => dialogTask?.Cancel()
            );
        }


        public override IDisposable ActionSheet(ActionSheetConfig config)
        {
            var dlg = new ActionSheetContentDialog();

            var vm = new ActionSheetViewModel
            {
                Title = config.Title,
                Message = config.Message,
                Cancel = new ActionSheetOptionViewModel(config.Cancel != null, config.Cancel?.Text, () =>
                {
                    dlg.Hide();
                    config.Cancel?.Action?.Invoke();
                }),

                Destructive = new ActionSheetOptionViewModel(config.Destructive != null, config.Destructive?.Text, () =>
                {
                    dlg.Hide();
                    config.Destructive?.Action?.Invoke();
                }),

                Options = config
                    .Options
                    .Select(x => new ActionSheetOptionViewModel(true, x.Text, () =>
                    {
                        dlg.Hide();
                        x.Action?.Invoke();
                    }, x.ItemIcon ?? config.ItemIcon))
                    .ToList()
            };

            dlg.DataContext = vm;
            IAsyncOperation<ContentDialogResult> dialogTask = null;

            return this.DispatchAndDispose(
                () => dialogTask = dlg.ShowAsync(),
                () => dialogTask?.Cancel()
            );
        }


        public override IDisposable Confirm(ConfirmConfig config)
        {
            var dialog = new MessageDialog(config.Message, config.Title ?? String.Empty);
            dialog.Commands.Add(new UICommand(config.OkText, x => config.OnAction?.Invoke(true)));
            dialog.DefaultCommandIndex = 0;

            dialog.Commands.Add(new UICommand(config.CancelText, x => config.OnAction?.Invoke(false)));
            dialog.CancelCommandIndex = 1;

            IAsyncOperation<IUICommand> dialogTask = null;
            return this.DispatchAndDispose(
                () => dialogTask = dialog.ShowAsync(),
                () => dialogTask?.Cancel()
            );
        }


        public override IDisposable DatePrompt(DatePromptConfig config)
        {
            var picker = new DatePickerControl();
            if (config.MinimumDate != null)
                picker.DatePicker.MinDate = config.MinimumDate.Value;

            if (config.MaximumDate != null)
                picker.DatePicker.MaxDate = config.MaximumDate.Value;

            var popup = this.CreatePopup(picker);
            if (!config.IsCancellable)
                picker.CancelButton.Visibility = Visibility.Collapsed;
            else
            {
                picker.CancelButton.Content = config.CancelText;
                picker.CancelButton.Click += (sender, args) =>
                {
                    var result = new DatePromptResult(false, this.GetDateForCalendar(picker.DatePicker));
                    config.OnAction?.Invoke(result);
                    popup.IsOpen = false;
                };
            }

            picker.OkButton.Content = config.OkText;
            picker.OkButton.Click += (sender, args) =>
            {
                var result = new DatePromptResult(true, this.GetDateForCalendar(picker.DatePicker));
                config.OnAction?.Invoke(result);
                popup.IsOpen = false;
            };
            if (config.SelectedDate != null)
            {
                picker.DatePicker.SelectedDates.Add(config.SelectedDate.Value);
                picker.DatePicker.SetDisplayDate(config.SelectedDate.Value);
            }
            return this.DispatchAndDispose(
                () => popup.IsOpen = true,
                () => popup.IsOpen = false
            );
        }


        public override IDisposable TimePrompt(TimePromptConfig config)
        {
            var picker = new TimePickerControl();
            picker.TimePicker.MinuteIncrement = config.MinuteInterval;

            var popup = this.CreatePopup(picker);

            if (!config.IsCancellable)
                picker.CancelButton.Visibility = Visibility.Collapsed;
            else
            {
                picker.CancelButton.Content = config.CancelText;
                picker.CancelButton.Click += (sender, args) =>
                {
                    var result = new TimePromptResult(false, picker.TimePicker.Time);
                    config.OnAction?.Invoke(result);
                    popup.IsOpen = false;
                };
            }

            picker.OkButton.Content = config.OkText;
            picker.OkButton.Click += (sender, args) =>
            {
                var result = new TimePromptResult(true, picker.TimePicker.Time);
                config.OnAction?.Invoke(result);
                popup.IsOpen = false;
            };
            if (config.SelectedTime != null)
            {
                picker.TimePicker.Time = config.SelectedTime.Value;
            }
            return this.DispatchAndDispose(
                () => popup.IsOpen = true,
                () => popup.IsOpen = false
            );
        }


        public override IDisposable Login(LoginConfig config)
        {
            var vm = new LoginViewModel
            {
                LoginText = config.OkText,
                Title = config.Title ?? String.Empty,
                Message = config.Message ?? String.Empty,
                UserName = config.LoginValue,
                UserNamePlaceholder = config.LoginPlaceholder,
                PasswordPlaceholder = config.PasswordPlaceholder,
                CancelText = config.CancelText
            };
            vm.Login = new Command(() =>
                config.OnAction?.Invoke(new LoginResult(true, vm.UserName, vm.Password))
            );
            vm.Cancel = new Command(() =>
                config.OnAction?.Invoke(new LoginResult(false, vm.UserName, vm.Password))
            );
            var dlg = new LoginContentDialog
            {
                DataContext = vm
            };
            return this.DispatchAndDispose(
                () => dlg.ShowAsync(),
                dlg.Hide
            );
        }


        public override IDisposable Prompt(PromptConfig config)
        {
            var stack = new StackPanel();
            if (!String.IsNullOrWhiteSpace(config.Message))
                stack.Children.Add(new TextBlock { Text = config.Message, TextWrapping = TextWrapping.WrapWholeWords });

            var dialog = new ContentDialog
            {
                Title = config.Title ?? String.Empty,
                Content = stack,
                PrimaryButtonText = config.OkText
            };

            if (config.InputType == InputType.Password)
                this.SetPasswordPrompt(dialog, stack, config);
            else
                this.SetDefaultPrompt(dialog, stack, config);

            if (config.IsCancellable)
            {
                dialog.SecondaryButtonText = config.CancelText;
                dialog.SecondaryButtonCommand = new Command(() =>
                {
                    config.OnAction?.Invoke(new PromptResult(false, String.Empty));
                    dialog.Hide();
                });
            }

            return this.DispatchAndDispose(
                () => dialog.ShowAsync(),
                dialog.Hide
            );
        }


        public override IDisposable Toast(ToastConfig config)
        {
            ToastPrompt toast = null;

            return this.DispatchAndDispose(() =>
            {
                toast = new ToastPrompt
                {
                    Message = config.Message,
                    //Stretch = Stretch.Fill,
                    TextWrapping = TextWrapping.Wrap,
                    MillisecondsUntilHidden = Convert.ToInt32(config.Duration.TotalMilliseconds)
                };
                if (config.Icon != null)
                    toast.ImageSource = config.Icon.ToNative();

                if (config.MessageTextColor != null)
                    toast.Foreground = new SolidColorBrush(config.MessageTextColor.Value.ToNative());

                if (config.BackgroundColor != null)
                    toast.Background = new SolidColorBrush(config.BackgroundColor.Value.ToNative());

                toast.Show();
            },
            () => toast.Hide());
        }


        #region Internals

        protected virtual Popup CreatePopup(UIElement element)
        {
            var popup = new Popup
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            if (element != null)
                popup.Child = element;

            return popup;
        }


        protected virtual DateTime GetDateForCalendar(CalendarView calendar)
        {
            return calendar.SelectedDates.Any()
                ? calendar.SelectedDates.First().Date
                : DateTime.MinValue;
        }


        protected virtual void SetPasswordPrompt(ContentDialog dialog, StackPanel stack, PromptConfig config)
        {
            var txt = new PasswordBox
            {
                PlaceholderText = config.Placeholder ?? String.Empty,
                Password = config.Text ?? String.Empty
            };
            if (config.MaxLength != null)
                txt.MaxLength = config.MaxLength.Value;

            stack.Children.Add(txt);
            dialog.PrimaryButtonCommand = new Command(() =>
            {
                config.OnAction?.Invoke(new PromptResult(true, txt.Password));
                dialog.Hide();
            });
            if (config.OnTextChanged == null)
                return;

            var args = new PromptTextChangedArgs { Value = txt.Password };
            config.OnTextChanged(args);
            dialog.IsPrimaryButtonEnabled = args.IsValid;

            txt.PasswordChanged += (sender, e) =>
            {
                args.IsValid = true; // reset
                args.Value = txt.Password;
                config.OnTextChanged(args);

                dialog.IsPrimaryButtonEnabled = args.IsValid;
                if (!args.Value.Equals(txt.Password))
                {
                    txt.Password = args.Value;
                }
            };
        }


        protected virtual void SetDefaultPrompt(ContentDialog dialog, StackPanel stack, PromptConfig config)
        {
            var txt = new TextBox
            {
                PlaceholderText = config.Placeholder ?? String.Empty,
                Text = config.Text ?? String.Empty
            };
            if (config.MaxLength != null)
                txt.MaxLength = config.MaxLength.Value;

            stack.Children.Add(txt);

            dialog.PrimaryButtonCommand = new Command(() =>
            {
                config.OnAction?.Invoke(new PromptResult(true, txt.Text.Trim()));
                dialog.Hide();
            });

            if (config.OnTextChanged == null)
                return;

            var args = new PromptTextChangedArgs { Value = txt.Text };
            config.OnTextChanged(args);
            dialog.IsPrimaryButtonEnabled = args.IsValid;

            txt.TextChanged += (sender, e) =>
            {
                args.IsValid = true; // reset
                args.Value = txt.Text;
                config.OnTextChanged(args);
                dialog.IsPrimaryButtonEnabled = args.IsValid;

                if (!args.Value.Equals(txt.Text))
                {
                    txt.Text = args.Value;
                    txt.SelectionStart = Math.Max(0, txt.Text.Length);
                    txt.SelectionLength = 0;
                }
            };
        }


        protected override IProgressDialog CreateDialogInstance(ProgressDialogConfig config) => new ProgressDialog(config);


        protected virtual void Dispatch(Action action)
        {
            //this.UiDispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action());
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action());
        }


        protected virtual IDisposable DispatchAndDispose(Action dispatch, Action dispose)
        {
            this.Dispatch(dispatch);
            return new DisposableAction(() =>
            {
                try
                {
                    this.Dispatch(dispose);
                }
                catch { }
            });
        }
        #endregion
    }
}

/*
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace Splat
{
    public class PlatformBitmapLoader : IBitmapLoader
    {
        public async Task<IBitmap> Load(Stream sourceStream, float? desiredWidth, float? desiredHeight)
        {
            return await GetDispatcher().RunAsync(async () => {
                using (var rwStream = new InMemoryRandomAccessStream()) {
                    var writer = rwStream.AsStreamForWrite();
                    await sourceStream.CopyToAsync(writer);
                    await writer.FlushAsync();
                    rwStream.Seek(0);

                    var decoder = await BitmapDecoder.CreateAsync(rwStream);

                    var transform = new BitmapTransform
                    {
                        ScaledWidth = (uint) (desiredWidth ?? decoder.OrientedPixelWidth),
                        ScaledHeight = (uint) (desiredHeight ?? decoder.OrientedPixelHeight),
                        InterpolationMode = BitmapInterpolationMode.Fant
                    };

                    var pixelData = await decoder.GetPixelDataAsync(decoder.BitmapPixelFormat, BitmapAlphaMode.Premultiplied, transform, ExifOrientationMode.RespectExifOrientation, ColorManagementMode.ColorManageToSRgb);
                    var pixels = pixelData.DetachPixelData();

                    var bmp = new WriteableBitmap((int)transform.ScaledWidth, (int)transform.ScaledHeight);
                    using (var bmpStream = bmp.PixelBuffer.AsStream()) {
                        bmpStream.Seek(0, SeekOrigin.Begin);
                        bmpStream.Write(pixels, 0, (int)bmpStream.Length);
                        return (IBitmap) new WriteableBitmapImageBitmap(bmp);
                    }
                }
            });
        }

        public async Task<IBitmap> LoadFromResource(string resource, float? desiredWidth, float? desiredHeight)
        {
            return await GetDispatcher().RunAsync(async () => {
                var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(resource));
                using (var stream = await file.OpenAsync(FileAccessMode.Read)) {
                    return await Load(stream.AsStreamForRead(), desiredWidth, desiredHeight);
                }
            });
        }

        public IBitmap Create(float width, float height)
        {
            return new WriteableBitmapImageBitmap(new WriteableBitmap((int)width, (int)height));
        }

        private static CoreDispatcher GetDispatcher()
        {
            CoreWindow currentThreadWindow = CoreWindow.GetForCurrentThread();

            return currentThreadWindow == null ? CoreApplication.MainView.CoreWindow.Dispatcher : currentThreadWindow.Dispatcher;
        }
    }

    class WriteableBitmapImageBitmap : IBitmap
    {
        internal WriteableBitmap inner;

        public float Width { get; protected set; }
        public float Height { get; protected set; }

        public WriteableBitmapImageBitmap(WriteableBitmap bitmap)
        {
            inner = bitmap;
            Width = (float)inner.PixelWidth;
            Height = (float)inner.PixelHeight;
        }

        public async Task Save(CompressedBitmapFormat format, float quality, Stream target)
        {
            // NB: Due to WinRT's brain-dead design, we're copying this image
            // like three times. Let Dreams Soar.
            var rwTarget = new InMemoryRandomAccessStream();
            var fmt = format == CompressedBitmapFormat.Jpeg ? BitmapEncoder.JpegEncoderId : BitmapEncoder.PngEncoderId;
            var encoder = await BitmapEncoder.CreateAsync(fmt, rwTarget, new[] { new KeyValuePair<string, BitmapTypedValue>("ImageQuality", new BitmapTypedValue(quality, PropertyType.Single)) });

            var pixels = new byte[inner.PixelBuffer.Length];
            await inner.PixelBuffer.AsStream().ReadAsync(pixels, 0, (int)inner.PixelBuffer.Length);

            encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied, (uint)inner.PixelWidth, (uint)inner.PixelHeight, 96, 96, pixels);
            await encoder.FlushAsync();
            await rwTarget.AsStream().CopyToAsync(target);
        }

        public void Dispose()
        {
            inner = null;
        }
    }

    class BitmapImageBitmap : IBitmap
    {
        internal BitmapImage inner;

        public float Width { get; protected set; }
        public float Height { get; protected set; }

        public BitmapImageBitmap(BitmapImage bitmap)
        {
            inner = bitmap;
            Width = (float)inner.PixelWidth;
            Height = (float)inner.PixelHeight;
        }

        public async Task Save(CompressedBitmapFormat format, float quality, Stream target)
        {
            string installedFolderImageSourceUri = inner.UriSource.OriginalString.Replace("ms-appx:/", "");
            var wb = new WriteableBitmap(inner.PixelWidth, inner.PixelHeight);
            var file = await StorageFile.GetFileFromPathAsync(inner.UriSource.OriginalString);
            await wb.SetSourceAsync(await file.OpenReadAsync());

            await (new WriteableBitmapImageBitmap(wb).Save(format, quality, target));
        }

        public void Dispose()
        {
            inner = null;
        }
    }

    public static class BitmapMixins
    {
        public static IBitmap FromNative(this BitmapImage This)
        {
            return new BitmapImageBitmap(This);
        }

        public static IBitmap FromNative(this WriteableBitmap This)
        {
            return new WriteableBitmapImageBitmap(This);
        }

        public static BitmapSource ToNative(this IBitmap This)
        {
            var wbib = This as WriteableBitmapImageBitmap;
            if (wbib != null) {
                return wbib.inner;
            }

            return ((BitmapImageBitmap)This).inner;
        }
    }

    static class DispatcherMixin
    {
        public static Task<T> RunAsync<T>(this CoreDispatcher This, Func<Task<T>> func, CoreDispatcherPriority prio = CoreDispatcherPriority.Normal)
        {
            var tcs = new TaskCompletionSource<T>();

            This.RunAsync(prio, () => {
                func().ContinueWith(t => {
                    if (t.IsFaulted) {
                        tcs.SetException(t.Exception);
                    } else {
                        tcs.SetResult(t.Result);
                    }
                });
            });

            return tcs.Task;
        }
    }
}
     */
