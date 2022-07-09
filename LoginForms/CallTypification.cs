using LoginForms.Models;
using LoginForms.Shared;
using LoginForms.Utils;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace LoginForms
{
    public partial class CallTypification : Form
    {
        public CallsView calls { get; set; }
        RestHelper rh = new RestHelper();
        Json jsonNetwork;
        string valor;
        string score;
        string appPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\ApplicationLogs\";
        //string userid;
        public CallTypification()//string userId
        {
            Log log = new Log(appPath);
            //userid = userId;
            InitializeComponent();
            ComboBoxGetNetwork();
            log.Add($"[CallTypification][Constructor]: Combo box en ACW");
            GlobalSocket.algo.Text = "ACW";

        }

        private async void CallTypification_Load(object sender, EventArgs e)
        {
            Log log = new Log(appPath);
            await rh.ChangeStatus(GlobalSocket.currentUser.ID, "9");
            log.Add($"[CallTypification][CallTypification_Load]: usuarioId:{GlobalSocket.currentUser.ID} asignando el statusId: 9");
        }

        private void cmbNetwork_SelectedIndexChanged(object sender, EventArgs e)
        {
            Log log = new Log(appPath);
            CallItems classItems = (CallItems)cmbNetwork.SelectedItem;
            valor = classItems.Id;
            log.Add($"[CallTypification][cmbNetwork_SelectedIndexChanged]: seleccionando la red: {classItems.Value}: {classItems.Name} con el id:{valor}");
        }

        private void cmbScore_SelectedIndexChanged(object sender, EventArgs e)
        {
            Log log = new Log(appPath);
            score = cmbScore.SelectedItem.ToString(); //este es el machin
            log.Add($"[CallTypification][cmbScore_SelectedIndexChanged]: el score seleccionado es:{score}");
            Console.WriteLine($"Valor seleccionado:{score}");
        }

        private async void ComboBoxGetNetwork()
        {
            Log log = new Log(appPath);
            try
            {
                string networkCategories = await rh.getNetworkCategories();
                Json jsonNetworkCategories = jsonNetwork = JsonConvert.DeserializeObject<Json>(networkCategories);
                log.Add($"[CallTypification][ComboBoxGetNetwork]:Redes Encontradas:{jsonNetworkCategories.data.networks.Count}");
                for (int i = 0; i < jsonNetworkCategories.data.networks.Count; i++)
                {
                    cmbNetwork.Items.Add(new CallItems(jsonNetwork.data.networks[i].description, jsonNetwork.data.networks[i].typification, jsonNetwork.data.networks[i].id));
                }

            }
            catch (Exception ex)
            {
                log.Add($"[CallTypification][ComboBoxGetNetwork]:{ex.Message}");
                Console.WriteLine($"Error[getNetworkCategories] {ex}");
            }
        }


        public void btnSave_Click(object sender, EventArgs e)
        {
            if (cmbNetwork.SelectedItem == null)
            {
                MessageBox.Show("Llena todos los campos", "Omnicanal", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                Typification();
            }

        }

        private void txtComments_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                if (cmbNetwork.SelectedItem == null)
                {
                    MessageBox.Show("Llena todos los campos", "Omnicanal", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    Typification();
                }

            }
        }

        private async void Typification()
        {
            Log log = new Log(appPath);
            try 
            {
                await rh.UpdateNetworkCategoryCalls(valor, score, txtComments.Text);
                await rh.ChangeStatus(GlobalSocket.currentUser.ID, "7");
                GlobalSocket.algo.Text = "Disponible";
                //GlobalSocket.SoftPhoneReconnect.Connect();
                //MessageBox.Show("Datos guardados correctamente", "Omnicanal", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                log.Add($"[CallTypification][Typification]:Tipificando con los datos:{valor} {score} {txtComments.Text}");
                log.Add($"[CallTypification][Typification]:Setenado el status del usuario a statusId:7");
                this.Dispose();

            }
            catch (Exception ex)
            {
                log.Add($"[CallTypification][Typification]:{ex.Message}");
                Console.WriteLine($"Error[Typification Method]: {ex.Message}");
            }
        }

        private void CallTypification_FormClosed(object sender, FormClosedEventArgs e)
        {
            Log log = new Log(appPath);
            calls.CallPBX();
            log.Add($"[CallTypification][Typification]:llamadando al metodo CallPBX del CallsView");

        }
    }

    class CallItems
    {
        public string Name;
        public string Value;
        public string Id;

        public CallItems(string name, string value, string id)
        {
            Name = name;
            Value = value;
            Id = id;
        }

        public override string ToString()
        {
            return Name;
        }
    }

}
