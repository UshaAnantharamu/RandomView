
using Android.Graphics;
using Java.Util;
using RandomView.Model;
using System.Drawing;
namespace RandomView.Utils
{
    public class RandomShapeAndSizeGenerator
    {
        public ViewProps prop { get; set; }

        public RandomShapeAndSizeGenerator(ViewProps props)
        {
            props.isCircle = isCircleImage();
            props.size = GetRandomSize();
            props.RandomColor = GetRandomColor();
            prop = props;
        }

        public bool isCircleImage()
        {
            Random rnd = new Random();
            bool res = rnd.NextBoolean();

            return res;
        }

        public int GetRandomSize()
        {
            Random rnd = new Random();
            int res = rnd.NextInt(300 - 50) + 50; ;

            return res;
        }

        public Android.Graphics.Color GetRandomColor()
        {
            Random randomGen = new Random();
            KnownColor[] names = (KnownColor[])System.Enum.GetValues(typeof(KnownColor));
            KnownColor randomColorName = names[randomGen.NextInt(names.Length)];
            System.Drawing.Color randomColor = System.Drawing.Color.FromKnownColor(randomColorName);

            Android.Graphics.Color color = new Android.Graphics.Color() { R = randomColor.R ,G = randomColor.G,B = randomColor.B,A = randomColor.A};
            return color;

        }
    }
}