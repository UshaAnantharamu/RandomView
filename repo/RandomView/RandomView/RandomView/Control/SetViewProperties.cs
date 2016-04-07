using RandomView.Model;
using RandomView.Utils;
using System.Xml;
using System.Linq;
using System.Threading.Tasks;
using Android.Graphics;
using Java.Lang;

namespace RandomView.Control
{
    public class SetViewProperties
    {
        public static Bitmap getRoundedShape(ViewProps props)
        {
            Bitmap scaleBitmapImage = props.image;

            int targetWidth = props.size;
            int targetHeight = props.size;

            Bitmap targetBitmap = Bitmap.CreateBitmap(targetWidth,
                targetHeight, Bitmap.Config.Argb8888);

            Canvas canvas = new Canvas(targetBitmap);
            Android.Graphics.Path path = new Android.Graphics.Path();
            path.AddCircle(((float)targetWidth - 1) / 2,
                ((float)targetHeight - 1) / 2,
                (Math.Min(((float)targetWidth),
                    ((float)targetHeight)) / 2),
                Android.Graphics.Path.Direction.Ccw);

            canvas.ClipPath(path);
            Bitmap sourceBitmap = scaleBitmapImage;
            canvas.DrawBitmap(sourceBitmap,
                new Rect(0, 0, sourceBitmap.Width,
                    sourceBitmap.Height),
                new Rect(0, 0, targetWidth, targetHeight), null);
            return targetBitmap;
        }

        public static Bitmap getRandomBitmap(ViewProps props)
        {
            Bitmap bitmap = Bitmap.CreateBitmap(props.size, props.size, Bitmap.Config.Argb8888);
            Canvas canvas = new Canvas(bitmap);

            Paint paint = new Paint();
            paint.SetARGB(props.RandomColor.A, props.RandomColor.R, props.RandomColor.G, props.RandomColor.B);
            paint.SetStyle(Paint.Style.Fill);
            canvas.DrawPaint(paint);

            return bitmap;
        }

        public static ViewProps SetViewProperty(ViewProps props)
        {
            RandomShapeAndSizeGenerator generator = new RandomShapeAndSizeGenerator(props);

            if (generator.prop.isCircle)
            {
                generator.prop.image = getRandomBitmap(generator.prop);
                generator.prop.image = getRoundedShape(generator.prop);
            }
            else
            {
                generator.prop.image = getRandomBitmap(generator.prop);
            }

            return generator.prop;
        }

        public static async Task<ViewProps> SetProperties(ViewProps props)
        {
            RandomShapeAndSizeGenerator generator = new RandomShapeAndSizeGenerator(props);

            if (generator.prop.isCircle)
            {
                if (CheckConnection.iSConnected() == true)
                {
                    var root = RetrieveXml.GetXml("http://www.colourlovers.com/api/colors/random");
                    if (root != null && root.GetElementsByTagName("imageUrl").Count != 0)
                    {
                        generator.prop.imageURL = root.GetElementsByTagName("imageUrl")[0].InnerText;
                        generator.prop.title = root.GetElementsByTagName("title")[0].InnerText;

                        DownloadImage download = new DownloadImage(generator.prop);
                        generator.prop = await download.downloadAsync();

                        generator.prop.image = getRoundedShape(generator.prop);
                    }
                }
            }
            else
            {
                if (CheckConnection.iSConnected() == true)
                {
                    var root = RetrieveXml.GetXml("http://www.colourlovers.com/api/patterns/random");
                    if (root != null && root.GetElementsByTagName("imageUrl").Count != 0)
                    {
                        generator.prop.imageURL = root.GetElementsByTagName("imageUrl")[0].InnerText;
                        generator.prop.title = root.GetElementsByTagName("title")[0].InnerText;

                        DownloadImage download = new DownloadImage(generator.prop);
                        generator.prop = await download.downloadAsync();

                    }
                }
            }
            return generator.prop;
        }


    }
}