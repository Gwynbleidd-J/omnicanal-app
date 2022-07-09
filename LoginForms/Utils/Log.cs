using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LoginForms.Utils
{
    public class Log
    {
        private string path = "";

        public Log(string path)
        {
            this.path = path;
        }

        public void Add(string sLog)
        {
            CreateDirectory();
            string nombre = GetNameFile();
            string cadena = "";

            cadena += DateTime.Now + " - " + sLog + Environment.NewLine;

            StreamWriter streamWriter = new StreamWriter(path+"/"+nombre, true);
            streamWriter.Write(cadena);
            streamWriter.Close();
        }

        #region HELPER

        private string GetNameFile()
        {
            string nombre = "";
            nombre = $"Omnicanal_Log_{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}.txt";

            return nombre;
        }
        private void CreateDirectory()
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
