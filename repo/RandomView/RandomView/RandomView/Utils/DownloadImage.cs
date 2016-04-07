using Android.Graphics;
using Android.OS;
using Android.Widget;
using Java.Lang;
using RandomView.Model;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
namespace RandomView.Utils
{
    public class DownloadImage
    {
        public ViewProps props { get; set; }

        public DownloadImage(ViewProps prop)
        {
            props = prop;
        }

        public async Task<ViewProps> downloadAsync()
        {
            WebClient webClient = new WebClient();
            var url = new Uri(props.imageURL);
            byte[] bytes = null;

            try
            {
                bytes = await webClient.DownloadDataTaskAsync(url);
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("Task Canceled!");
                return null;
            }

            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string localFilename = "downloaded.png";
            string localPath = System.IO.Path.Combine(documentsPath, localFilename);

            FileStream fs = new FileStream(localPath, FileMode.OpenOrCreate);
            await fs.WriteAsync(bytes, 0, bytes.Length);

            Console.WriteLine("localPath:" + localPath);
            fs.Close();

            BitmapFactory.Options options = new BitmapFactory.Options();
            options.InJustDecodeBounds = true;
            await BitmapFactory.DecodeFileAsync(localPath, options);

            options.InSampleSize = options.OutWidth > options.OutHeight ? options.OutHeight / props.size : options.OutWidth / props.size;
            options.InJustDecodeBounds = false;

            Bitmap bitmap = await BitmapFactory.DecodeFileAsync(localPath, options);

            props.image = bitmap;

            return props;
        }

    }


}