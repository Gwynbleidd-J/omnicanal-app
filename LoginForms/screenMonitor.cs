using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
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
        int idSupervisor = 0;
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
                Console.WriteLine("El agente a monitorear es:" +idAgent +comboBox1.Text);
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

                    //var ImageData = await rh.getMonitoring();


                    return "";
                }
                else {
                    return response.StatusCode.ToString();
                }
            }
        }

        public void setImage(Image img, string PPath) {

            pictureBox1.Invalidate();
            pictureBox1.Image = Image.FromFile(PPath);
            pictureBox1.ImageLocation = PPath;
            pictureBox1.Refresh();
            pictureBox1.Invalidate();
        }
        public void setImage( Bitmap imagen)
        {

            //pictureBox1.Invalidate();
            pictureBox1.Image = imagen;
            //pictureBox1.ImageLocation = PPath;
            //pictureBox1.Refresh();
            //pictureBox1.Invalidate();

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            idSupervisor = int.Parse(GlobalSocket.currentUser.ID);
            await rh.startMonitoring(idAgent, idSupervisor);
            //await CaptureScreenshotAsync();
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            await CaptureScreenshotAsync();
        }

        private void comboBox1_Click(object sender, EventArgs e)
        {
            Iniciado = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = Image.FromFile("C:\\Users\\KODE\\Documents\\omnicanal_app\\LoginForms\\bin\\Debug\\AgentScreenshots\\image.jpeg");
        }
    }
}
