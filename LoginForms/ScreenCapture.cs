using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Storage;

namespace LoginForms
{
    public class ScreenCapture
    {
        public void CapturarPantalla()
        {
            //Set A rectangle with the height and width of the screen

            Rectangle resolution = Screen.PrimaryScreen.Bounds;

            Bitmap memoryImage;
            memoryImage = new Bitmap(resolution.Width, resolution.Height);
            Size s = new Size(memoryImage.Width, memoryImage.Height);
            Graphics memoryGraphics = Graphics.FromImage(memoryImage);
            memoryGraphics.CopyFromScreen(0, 0, 0, 0, s);

            string fileName;
            //string path;
            var name = "capturita";
            try
            {
                ToBase64String(memoryImage, ImageFormat.Png);
                //GetFileArray(memoryGraphics);
                fileName = name + "_capture.png";

                if (memoryImage.Width >= 3840)
                    Save(memoryImage, 5760, 1080, 75, fileName);
                else
                    Save(memoryImage, 1920, 1080, 75, fileName);
                sendFileHttp(fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            memoryImage.Dispose();
            memoryGraphics.Dispose();
        }

        private static string ToBase64String(Bitmap bitmap, ImageFormat image)
        {
            string base64String = string.Empty;
            MemoryStream memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, image);

            memoryStream.Position = 0;
            byte[] byteBuffer = memoryStream.ToArray();

            memoryStream.Close();

            base64String = Convert.ToBase64String(byteBuffer);
            byteBuffer = null;
            Console.WriteLine(base64String);;
            return base64String;
        }



        public static void Save(Bitmap image, int maxWidth, int maxHeight, int quality, string filePath)
        {
            // Get the image's original width and height
            int originalWidth = image.Width;
            int originalHeight = image.Height;

            // To preserve the aspect ratio
            float ratioX = (float)maxWidth / (float)originalWidth;
            float ratioY = (float)maxHeight / (float)originalHeight;
            float ratio = Math.Min(ratioX, ratioY);

            // New width and height based on aspect ratio
            int newWidth = (int)(originalWidth * ratio);
            int newHeight = (int)(originalHeight * ratio);

            // Convert other formats (including CMYK) to RGB.
            Bitmap newImage = new Bitmap(newWidth, newHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            // Draws the image in the specified size with quality mode set to HighQuality
            using (Graphics graphics = Graphics.FromImage(newImage))
            {
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);
            }

            // Get an ImageCodecInfo object that represents the JPEG codec.
            ImageCodecInfo imageCodecInfo = GetEncoderInfo(ImageFormat.Jpeg);

            // Create an Encoder object for the Quality parameter.
            Encoder encoder = Encoder.Quality;

            // Create an EncoderParameters object. 
            EncoderParameters encoderParameters = new EncoderParameters(1);

            // Save the image as a JPEG file with quality level.
            EncoderParameter encoderParameter = new EncoderParameter(encoder, quality);
            encoderParameters.Param[0] = encoderParameter;
            newImage.Save(filePath, imageCodecInfo, encoderParameters);
            newImage.Dispose();
        }

        private static void sendFileHttp(string filePath)
        {
            // Inicializa webRequest con la url de envio
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:3001/api/prueba");

            // Asigna el separador de las lineas de Content-Disposition
            string boundaryString = "AaB03x";
            string fileUrl = filePath;

            // Asigna el header de la peticion
            httpWebRequest.Method = WebRequestMethods.Http.Post;
            httpWebRequest.ContentType = "multipart/form-data, boundary=" + boundaryString;
            httpWebRequest.KeepAlive = true;

            // Usar un Memorystream para el la data de la peticionU,
            // Este nos permitira sacar el tamano del contenido de la peticion.
            MemoryStream memoryStream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(memoryStream);

            // Incluir el parametro nombre con el nombre del archivo en el destino
            streamWriter.Write("\r\n--" + boundaryString + "\r\n");
            streamWriter.Write("Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}", "nombre", DateTime.Now.ToString("yyyyMMdd") + "_" + Path.GetFileNameWithoutExtension(filePath));

            // Incluir el parametro ruta con la ruta a la que llegara en el servidor
            streamWriter.Write("\r\n--" + boundaryString + "\r\n");
            streamWriter.Write("Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}", "ruta", "http://localhost:3001/api/prueba");

            // Incluir el parametro filename con el nombre del archivo y su extension
            streamWriter.Write("\r\n--" + boundaryString + "\r\n");
            streamWriter.Write("Content-Disposition: form-data;"
            + "name=\"{0}\";"
            + "filename=\"{1}\""
            + "\r\nContent-Type: {2}\r\n\r\n",
            "filename",
            Path.GetFileName(filePath),
            Path.GetExtension(filePath));

            // Se hace flush al StreamWriter para almacenar el contenido y no perder datos.
            streamWriter.Flush();

            // Se crea un FileStream para obtener el archivo en bytes
            FileStream fileStream = new FileStream(fileUrl, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[1024];
            int bytesRead = 0;

            // Se agregan los bytes del archivo al MemoryStream contenido en el StreamWriter
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0) memoryStream.Write(buffer, 0, bytesRead);

            fileStream.Close();

            // Cerrar el contenido con el separador y hacer flush final
            streamWriter.Write("\r\n--" + boundaryString + "--\r\n");
            streamWriter.Flush();

            // Asignar al header el tamano del contenido del cuerpo de la peticion
            httpWebRequest.ContentLength = memoryStream.Length;
            Console.WriteLine(httpWebRequest.GetRequestStream());

            // Volcar el contenido del MemoryStream al strea de la peticion
            using (Stream s = httpWebRequest.GetRequestStream())
            {
                memoryStream.WriteTo(s);
            }

            // Ejecutar peticion cargada con los datos a almacenar
            WebResponse response = httpWebRequest.GetResponse();
            StreamReader responseReader = new StreamReader(response.GetResponseStream());
            string replyFromServer = responseReader.ReadToEnd();

            memoryStream.Close();
        }

        #region Metodos tentativos para el uso en está clase
        private async Task<string> UploadImage(StorageFile file)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://your.url.com/");
            MultipartFormDataContent form = new MultipartFormDataContent();
            HttpContent content = new StringContent("fileToUpload");
            form.Add(content, "fileToUpload");
            var stream = await file.OpenStreamForReadAsync();
            content = new StreamContent(stream);
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "fileToUpload",
                FileName = file.Name
            };
            form.Add(content);
            var response = await client.PostAsync("upload.php", form);
            return response.Content.ReadAsStringAsync().Result;
        }

        #region Métodos para guardar la imagen en el equipo local



        private static ImageCodecInfo GetEncoderInfo(ImageFormat format)
        {
            return ImageCodecInfo.GetImageDecoders().SingleOrDefault(c => c.FormatID == format.Guid);
        }
        #endregion

        #region Otros Metodos que se intentaran usar para enviar información al servidor.
        private byte[] GetFileArray(string filename)
        {
            FileStream fileStream = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Read);

            byte[] FileByArrayData = new byte[filename.Length];
            fileStream.Read(FileByArrayData, 0, System.Convert.ToInt32(filename.Length));

            fileStream.Close();

            return FileByArrayData;
        }

        public string UploadVideoFile(string URL, byte[] VideoFileData)
        {
            string Response = null;
            HttpWebRequest WebReq = null;
            HttpWebResponse WebRes = null;
            StreamReader StreamResponseReader = null;
            Stream requestStream = null;
            try
            {
                WebReq = (HttpWebRequest)WebRequest.Create(URL);
                WebReq.Method = "POST";
                WebReq.Accept = "*/*";
                WebReq.Timeout = 50000;
                WebReq.KeepAlive = false;
                WebReq.AllowAutoRedirect = false;
                WebReq.AllowWriteStreamBuffering = true;
                WebReq.ContentType = "binary/octet-stream";
                WebReq.ContentLength = VideoFileData.Length;

                requestStream = WebReq.GetRequestStream();
                requestStream.Write(VideoFileData, 0, VideoFileData.Length);
                requestStream.Close();

                WebRes = (HttpWebResponse)WebReq.GetResponse();
                StreamResponseReader = new StreamReader(WebRes.GetResponseStream(), System.Text.Encoding.UTF8);
                Response = StreamResponseReader.ReadToEnd();
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (WebReq != null)
                {
                    WebReq.Abort();
                    WebReq = null;
                }

                if (WebRes != null)
                {
                    WebRes.Close();
                    WebRes = null;
                }

                if (StreamResponseReader != null)
                {
                    StreamResponseReader.Close();
                    StreamResponseReader = null;
                }

                if (requestStream != null)
                {
                    requestStream = null;
                }
            }

            return Response;
        }
        #endregion

 

        #endregion

    }

}
