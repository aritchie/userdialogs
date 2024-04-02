using AppKit;

namespace Acr.UserDialogs
{
    public static class ViewsExtensions
    {
        public static void AggregateSubviews(this NSView view, params NSView[] subviews)
        {
            foreach (var sv in subviews)
                view.AddSubview(sv);

            foreach (var sv in view.Subviews)
                sv.TranslatesAutoresizingMaskIntoConstraints = false;
        }
    }
}
