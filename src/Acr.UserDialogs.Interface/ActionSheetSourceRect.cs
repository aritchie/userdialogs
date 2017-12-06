namespace Acr.UserDialogs
{
    public class ActionSheetSourceRect
    {
        public double X { get; }
        public double Y { get; }

        public double Width { get; }
        public double Height { get; }

        public ActionSheetSourceRect(double x, double y, double width, double height)
        {
            X = x;
            Y = y;

            Width = width;
            Height = height;
        }
    }
}