using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace LoginForms
{
    public partial class screenMonitor : Form
    {
        string path = "";
        string appPath = "";
        private static readonly string baseUrl = ConfigurationManager.AppSettings["IpServidor"];

        public screenMonitor()
        {
            InitializeComponent();
            pictureBox1.MinimumSize = new Size(400, 400);
            appPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\AgentScreenshots\"; 
            if (Directory.Exists(appPath) == false)                                              
            {                                                                                    
                Directory.CreateDirectory(appPath);                                              
            }
            path = appPath + "image.jpeg";
        }

        public async Task CaptureScreenshotAsync()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                    bitmap.Save(path, ImageFormat.Jpeg);
                    await UploadAsync(baseUrl, bitmap.ToString());
                }
            }
        }

        private async Task<string> UploadAsync(string url, string filename)
        {
            HttpContent stringContent = new StringContent(filename);
            //HttpContent fileStreamContent = new StreamContent(fileStream);
            //HttpContent bytesContent = new ByteArrayContent(fileBytes);

            using (var client = new HttpClient())
            using (var formData = new MultipartFormDataContent())
            {
                // Add the HttpContent objects to the form data

                // <input type="text" name="filename" />
                formData.Add(stringContent, "filename", "filename");

                var response = await client.PostAsync(url, formData);
                HttpContent content = response.Content;
                string data = await content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
                {
                    var cleanData = (JObject)JsonConvert.DeserializeObject(data);
                    var algo = cleanData["data"].Children();
                    var urlPath = algo["nombreData"].Value<string>();

                    pictureBox1.ImageLocation = urlPath;
                    return data;
                }
                else {
                    return response.StatusCode.ToString();
                }
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await CaptureScreenshotAsync();
        }
    }
}
