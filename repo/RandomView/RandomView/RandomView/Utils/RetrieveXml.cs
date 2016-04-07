using Android.Provider;
using Java.Lang;
using Java.Net;
using Javax.Xml.Parsers;
using Org.Xml.Sax;
using Org.XmlPull.V1;
using System.Net;
using System.Xml;

namespace RandomView.Utils
{
    public class RetrieveXml
    {
        public static XmlElement GetXml(string url)
        {

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(url);
                XmlElement root = doc.DocumentElement;
                return root;
                
            }
            catch (Exception e)
            {
                return null;
            }

        }
    }
}