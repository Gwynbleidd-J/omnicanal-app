
using LoginForms.Models;
using LoginForms.Shared;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace LoginForms
{
    public partial class CallTypification : Form
    {
        CallsView calls = new CallsView();
        RestHelper rh = new RestHelper();
        Json jsonNetwork;
        string valor;
        string score;
        //string userid;
        public CallTypification()//string userId
        {
            //userid = userId;
            InitializeComponent();
            ComboBoxGetNetwork();
            GlobalSocket.algo.Text = "ACW";

        }

        private async void CallTypification_Load(object sender, EventArgs e)
        {
            await rh.ChangeStatus(GlobalSocket.currentUser.ID, "9");
        }

        private void cmbNetwork_SelectedIndexChanged(object sender, EventArgs e)
        {
            CallItems classItems = (CallItems)cmbNetwork.SelectedItem;
            valor = classItems.Id;
        }

        private void cmbScore_SelectedIndexChanged(object sender, EventArgs e)
        {
             score = cmbScore.SelectedItem.ToString(); //este es el machin
            Console.WriteLine($"Valor seleccionado:{score}");
        }

        private async void ComboBoxGetNetwork()
        {
            try
            {
                string networkCategories = await rh.getNetworkCategories();
                Json jsonNetworkCategories = jsonNetwork = JsonConvert.DeserializeObject<Json>(networkCategories);
                for (int i = 0; i < jsonNetworkCategories.data.networks.Count; i++)
                {
                    cmbNetwork.Items.Add(new CallItems(jsonNetwork.data.networks[i].description, jsonNetwork.data.networks[i].typification, jsonNetwork.data.networks[i].id));
                }

            }
            catch (Exception ex)
            {
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
            try 
            {
                await rh.UpdateNetworkCategoryCalls(valor, score, txtComments.Text);
                await rh.ChangeStatus(GlobalSocket.currentUser.ID, "7");
                GlobalSocket.algo.Text = "Disponible";
                //GlobalSocket.SoftPhoneReconnect.Connect();
                //MessageBox.Show("Datos guardados correctamente", "Omnicanal", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                this.Dispose();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error[Typification Method]: {ex.Message}");
            }
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
