using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Java.Lang;
using Android.Util;
using RandomView.Utils;
using System.Xml;
using Org.XmlPull.V1;
using RandomView.Control;
using RandomView.Model;
using Android.Hardware;
using System.Threading.Tasks;

namespace RandomView
{
    [Activity(Label = "RandomView", MainLauncher = true, Icon = "@drawable/icon", NoHistory = true)]
    public class MainActivity : Activity
    {
        /// <summary>
        /// Sync Lock used in shake sensor
        /// </summary>
        private static readonly object _syncLock = new object();
        /// <summary>
        ///Sensor Manager
        /// </summary>
        private SensorManager _sensorManager;
        /// <summary>
        ///Sensor 
        /// </summary>
        private Sensor _sensor;
        /// <summary>
        /// Shake Detector
        /// </summary>
        private ShakeDetector _shakeDetector;
        /// <summary>
        /// Layout Params to change the position of image
        /// </summary>
        private RelativeLayout.LayoutParams layoutParams;
        /// <summary>
        /// Ovverriding the OnCreate() for the MainActivity
        /// </summary>
        private JavaList<ViewProps> propsList = new JavaList<ViewProps>();
        /// <summary>
        /// Override OnCreate()
        /// </summary>
        protected async override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            /// <summary>
            /// Initialise the counter to 0
            /// </summary>
            int counter = 0;
            /// <summary>
            /// Define sensor manager,detector and events
            /// </summary>
            _sensorManager = (SensorManager)GetSystemService(Context.SensorService);
            _sensor = _sensorManager.GetDefaultSensor(SensorType.Accelerometer);
            _shakeDetector = new ShakeDetector();
            /// <summary>
            /// Preload 15 items
            /// </summary>
            ProgressDialog pd = new ProgressDialog(this);
            pd.SetTitle("Loading..");
            pd.Show();
            for (int i = 0; i < 15; i++)
            {
                if (CheckConnection.iSConnected())
                {
                    propsList.Add(await SetViewProperties.SetProperties(new ViewProps()));

                }
                else
                {
                    propsList.Add(SetViewProperties.SetViewProperty(new ViewProps()));

                }
            }
            pd.Dismiss();
            /// <summary>
            ///  Set our view from the "main" layout resource
            /// </summary>
            SetContentView(Resource.Layout.Main);
            /// <summary>
            ///  Device specific properties
            /// </summary>
            DeviceSpecificProperties props = new DeviceSpecificProperties();
            double Y = props.inchY;
            /// <summary>
            ///  //Define the views and layouts which can be accessed
            /// </summary>
            ImageView imageView = FindViewById<ImageView>(Resource.Id.imageView);
            imageView.SetImageResource(Resource.Drawable.Icon);
            RelativeLayout layout = FindViewById<RelativeLayout>(Resource.Id.mainLayout);

            _shakeDetector.Shaked += (sender1, shakeCount) =>
            {
                lock (_syncLock)
                {
                    imageView.Visibility = ViewStates.Invisible;
                }
            };
            /// <summary>
            ///  Events
            /// </summary>
            imageView.LongClick += (sender, e) =>
            {
                View v = (View)sender;

                ClipData.Item item = new ClipData.Item("category", "value");
                string[] mimeTypes = { ClipDescription.MimetypeTextPlain };

                ClipData dragData = new ClipData("category", mimeTypes, item);
                View.DragShadowBuilder myShadow = new View.DragShadowBuilder(imageView);

                imageView.StartDrag(dragData, myShadow, null, 0);

            };
            imageView.Drag += (sender, e) =>
            {
                View v = (View)sender;
                switch (e.Event.Action)
                {
                    case DragAction.Started:
                        layoutParams = (RelativeLayout.LayoutParams)v.LayoutParameters;
                        break;
                    case DragAction.Entered:
                        var x_cord = (int)e.Event.GetX();
                        var y_cord = (int)e.Event.GetY();
                        break;

                    case DragAction.Exited:
                        x_cord = (int)e.Event.GetX();
                        y_cord = (int)e.Event.GetY();
                        layoutParams.LeftMargin = x_cord;
                        layoutParams.TopMargin = y_cord;
                        v.LayoutParameters = layoutParams;
                        break;

                    case DragAction.Location:
                        x_cord = (int)e.Event.GetX();
                        y_cord = (int)e.Event.GetY();
                        break;

                    case DragAction.Drop:
                        break;

                    default: break;
                }
            };
            int prevtime = 0;
            imageView.Click += async (sender, e) =>
            {
                prevtime++;

                if (prevtime == 2)
                {
                    if (CheckConnection.iSConnected())
                    {
                        var res = await SetViewProperties.SetProperties(new ViewProps());
                        imageView.SetImageBitmap(res.image);

                        if (res.title != null)
                            ActionBar.Title = res.title;

                    }
                    else
                    {
                        var res = SetViewProperties.SetViewProperty(new ViewProps());

                        imageView.SetImageBitmap(res.image);

                        if (res.title != null)
                            ActionBar.Title = res.title;
                    }

                    prevtime = 0;
                }
            };
            layout.Touch += (sender, e) =>
            {
                try
                {
                    imageView.Visibility = ViewStates.Visible;
                    var x_cord = (int)e.Event.RawX;
                    var y_cord = (int)(e.Event.RawY - Y * 0.23);
                    layoutParams = new RelativeLayout.LayoutParams((int)100, (int)100);
                    layoutParams.LeftMargin = x_cord;
                    layoutParams.TopMargin = y_cord;
                    imageView.LayoutParameters = layoutParams;
                    if (e.Event.Action == MotionEventActions.Up)
                    {
                        try
                        {

                            if (counter < 15)
                            {

                                var res = propsList[counter];
                                imageView.SetImageBitmap(res.image);

                                if (res.title != null)
                                    ActionBar.Title = res.title;

                                counter++;

                                //if (CheckConnection.iSConnected())
                                //{
                                //    var res = await SetViewProperties.SetProperties(new ViewProps());
                                //    //var res = propsList[counter + 1];
                                //    imageView.SetImageBitmap(res.image);
                                //    ActionBar.Title = res.title;
                                //}
                                //else
                                //{
                                //    var res = SetViewProperties.SetViewProperty(new ViewProps());
                                //    imageView.SetImageBitmap(res.image);
                                //}

                            }
                            else
                            {
                                counter = 0;
                            }

                        }
                        catch (Java.Lang.Exception e1)
                        {
                            e1.PrintStackTrace();
                            var res = SetViewProperties.SetViewProperty(new ViewProps());
                            imageView.SetImageBitmap(res.image);
                        }
                    }
                }
                catch (Java.Lang.Exception e1)
                {
                    var res = SetViewProperties.SetViewProperty(new ViewProps());
                    imageView.SetImageBitmap(res.image);
                }
            };

        }
        protected override void OnResume()
        {
            base.OnResume();

            _sensorManager = (SensorManager)GetSystemService(Context.SensorService);
            _sensorManager.RegisterListener(_shakeDetector, _sensor, SensorDelay.Ui);
        }
        protected override void OnPause()
        {
            base.OnPause();

            _sensorManager = (SensorManager)GetSystemService(Context.SensorService);
            _sensorManager.UnregisterListener(_shakeDetector);
        }
        protected override void OnStart()
        {
            base.OnStart();
        }

    }
}

