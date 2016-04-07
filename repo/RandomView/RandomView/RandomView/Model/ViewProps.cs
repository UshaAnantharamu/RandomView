using Android.Graphics;
namespace RandomView.Model
{
    public class ViewProps
    {
        public bool isCircle { get; set; }
        public int size { get; set; }
        public string imageURL { get; set; }
        public Android.Graphics.Color RandomColor { get; set; }
        public string title { get; set; }
        public Bitmap image { get; set; }
    }
}