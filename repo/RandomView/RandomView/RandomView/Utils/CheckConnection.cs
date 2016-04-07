using Android.Content;
using Android.Net;
namespace RandomView.Utils
{
    public class CheckConnection
    {
        public static bool iSConnected()
        {
            ConnectivityManager connectivityManager = (ConnectivityManager)Android.App.Application.Context.GetSystemService(Context.ConnectivityService);
            var activeNetworkInfo = connectivityManager.ActiveNetworkInfo;
                     

            if (activeNetworkInfo != null && activeNetworkInfo.IsConnected)
                return true;
            else
                return false;

        }
    }
}