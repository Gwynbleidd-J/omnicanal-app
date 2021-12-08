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
using LoginForms.Shared;
using System.Collections.Generic;

namespace LoginForms
{
    public partial class screenMonitor : Form
    {
        RestHelper rh = new RestHelper();
        bool Iniciado = false;
        int idAgent = 0;
        int idSupervisor = int.Parse(GlobalSocket.currentUser.ID);
        string transferAgentName = "";

        string path = "";
        string appPath = "";
        private static readonly string baseUrl = ConfigurationManager.AppSettings["IpServidor"];

        public screenMonitor()
        {
            InitializeComponent();
            GetAllAgentsAsync();
            

            pictureBox1.MinimumSize = new Size(400, 400);
            appPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\AgentScreenshots\"; 
            if (Directory.Exists(appPath) == false)                                              
            {                                                                                    
                Directory.CreateDirectory(appPath);                                              
            }
            path = appPath + "image.jpeg";
            timer1.Enabled = false;
        }

        private async void GetAllAgentsAsync()
        {
            try
            {
                var agents = await rh.getAllAgents();
                var cleanData = (JObject)JsonConvert.DeserializeObject(agents);
                var algo = cleanData["data"].Children();

                List<KeyValuePair<string, string>> lista = new List<KeyValuePair<string, string>>();

                foreach (var item in algo)
                {
                    var agentID = item["id"].Value<string>();
                    var agentName = item["agente"].Value<string>();

                    lista.Add(new KeyValuePair<string, string>(agentID, agentName));
                }

                comboBox1.DataSource = lista;
                comboBox1.DisplayMember = "Value";
                comboBox1.ValueMember = "Key";

                comboBox1.SelectedIndex = 0;

                idAgent = int.Parse(comboBox1.SelectedValue.ToString());
                transferAgentName = comboBox1.GetItemText(comboBox1.SelectedItem);

                Console.WriteLine("\nEl agente seleccionado por ahora es:" + transferAgentName + " \nCon el valor:" + idAgent);
                Console.WriteLine("\nY los datos del combo son:" + comboBox1.GetItemText(comboBox1.SelectedItem));

            }
            catch (Exception _e)
            {

                throw _e;
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Iniciado)
            {
                idAgent = int.Parse(comboBox1.SelectedValue.ToString());
                transferAgentName = comboBox1.Text;
                //MessageBox.Show("Agente Seleccionado:" + comboBox1.Text + "\nIdentificador:" + comboBox1.SelectedValue);
            }
        }

        public async Task CaptureScreenshotAsync()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                    
                    //bitmap.Save(path, ImageFormat.Jpeg);
                    //pictureBox1.ImageLocation = path;

                    //await UploadAsync(baseUrl + "record/screen", bitmap.ToString());
                    //Image image = Image.FromFile(path);

                    await rh.shareScreenshot(bitmap, "19");

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
                // Add the HttpContent objects to the form datah

                // <input type="text" name="filename" />
                formData.Add(stringContent, "image", "image");

                //var response = await client.PostAsync(url, formData);
                var response = await client.GetAsync(url);

                HttpContent content = response.Content;

                var data = await content.ReadAsByteArrayAsync();
                using (var ms = new MemoryStream(data))
                {
                    Image imagen = Image.FromStream(ms);
                    pictureBox1.Image = imagen;
                }

                if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode.ToString() == "OK")
                {
                    //var cleanData = (JObject)JsonConvert.DeserializeObject(data);
                    //var algo = cleanData["data"].Children();
                    //var urlPath = algo["filename"].Value<string>();

                    //pictureBox1.ImageLocation = urlPath;
                    //File.Delete(path);

                    var ImageData = await rh.getMonitoring();


                    return "";
                }
                else {
                    return response.StatusCode.ToString();
                }
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            //var data = await rh.getMonitoring();
            //Image x = (Bitmap)((new ImageConverter()).ConvertFrom(data));
            //pictureBox1.Image = x;
            //await rh.startMonitoring(idAgent, idSupervisor);
            await CaptureScreenshotAsync();
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            await CaptureScreenshotAsync();
        }
    }
}
