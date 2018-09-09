using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web.Hosting;
using System.Xml.Serialization;
using CustomBasicAuthWebApp.Models;

namespace CustomBasicAuthWebApp.DataAccessLayer
{
    /// <summary>
    /// Handles the presistency of the User model in a xml file
    /// </summary>
    public class XmlUserDbDAL : IUserDbDAL
    {
        public List<User> Users { get; set; }

        private string XmlDataPath { get; set; }

        public XmlUserDbDAL()
        {
            Users = new List<User>();

            try
            {
                string configPath = ConfigurationManager.AppSettings["XmlDataPath"];
                XmlDataPath = MapPath(configPath);

                if (File.Exists(XmlDataPath))
                {
                    using (var sw = new StreamReader(XmlDataPath, Encoding.Default))
                    {
                        var ser = new XmlSerializer(typeof(List<User>));
                        Users = (List<User>)ser.Deserialize(sw);
                    }
                }

            }
            catch
            {
                throw new Exception("Problem reading from xml data file.");
            }

        }

        public void SaveChanges()
        {
            try
            {
                var sw = new StreamWriter(XmlDataPath, false, Encoding.Default);
                var ser = new XmlSerializer(typeof(List<User>));
                ser.Serialize(sw, this.Users);
                sw.Close();
            }
            catch
            {
                throw new Exception("Problem saving to xml data file.");
            }
        }

        private static string MapPath(string filePath)
        {
            // Support unit testing scenario where hosting environment is not initialized.
            var hostingRoot = HostingEnvironment.IsHosted
            ? HostingEnvironment.MapPath("~/")
            : AppDomain.CurrentDomain.BaseDirectory;

            return Path.Combine(hostingRoot, filePath.Substring(1).Replace('/', '\\'));
        }
    }
}