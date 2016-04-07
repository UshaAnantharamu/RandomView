
using Android.App;
using Android.Util;
using Java.Lang;
namespace RandomView.Utils
{
    public class Screensize
    {

        /// <summary>
        ///     Initializes a new instance of the <see cref="Display" /> class.
        /// </summary>
        public Screensize()
        {
            var dm = Metrics;
            Height = dm.HeightPixels;
            Width = dm.WidthPixels;
            Xdpi = dm.Xdpi;
            Ydpi = dm.Ydpi;

            //FontManager = new FontManager(this);
        }

        /// <summary>
        ///     Gets the metrics.
        /// </summary>
        /// <value>The metrics.</value>
        public static DisplayMetrics Metrics
        {
            get
            {
                return Application.Context.Resources.DisplayMetrics;
            }
        }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents the current <see cref="Display" />.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents the current <see cref="Display" />.</returns>
        public override string ToString()
        {
            return string.Format("[Screen: Height={0}, Width={1}, Xdpi={2:0.0}, Ydpi={3:0.0}]", Height, Width, Xdpi, Ydpi);
        }

        #region IScreen implementation

        /// <summary>
        ///     Gets the screen height in pixels
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        ///     Gets the screen width in pixels
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        ///     Gets the screens X pixel density per inch
        /// </summary>
        public double Xdpi { get; private set; }

        /// <summary>
        ///     Gets the screens Y pixel density per inch
        /// </summary>
        public double Ydpi { get; private set; }

        //public IFontManager FontManager { get; private set; }

        /// <summary>
        ///     Convert width in inches to runtime pixels
        /// </summary>
        public double WidthRequestInInches(double inches)
        {
            return inches * Xdpi / Metrics.Density;
        }

        /// <summary>
        ///     Convert height in inches to runtime pixels
        /// </summary>
        public double HeightRequestInInches(double inches)
        {
            return inches * Ydpi / Metrics.Density;
        }

        #endregion
    }

    public class DeviceSpecificProperties
    {
        /// <summary>
        /// This method describes the height and width device specific properties
        /// </summary>
      
        public double inchX { get; set; }
        public double inchY { get; set; }

        Screensize display = new Screensize();
               

        /// <summary>
        /// Screens the width inches.
        /// </summary>
        /// <param name="screen">The screen.</param>
        /// <returns>System.Double.</returns>
        public static double ScreenWidthInches(Screensize screen)
        {
            return screen.Width / screen.Xdpi;
        }

        /// <summary>
        /// Screens the height inches.
        /// </summary>
        /// <param name="screen">The screen.</param>
        /// <returns>System.Double.</returns>
        public static double ScreenHeightInches(Screensize screen)
        {
            return screen.Height / screen.Ydpi;
        }


        public DeviceSpecificProperties()
        {


            inchX = display.WidthRequestInInches(ScreenWidthInches(display));
            inchY = display.HeightRequestInInches(ScreenHeightInches(display));

        }
    }
}