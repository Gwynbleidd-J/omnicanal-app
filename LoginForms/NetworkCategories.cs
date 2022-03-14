using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LoginForms.Shared;
using LoginForms.Models;
using Newtonsoft.Json;
using System.Web.UI.WebControls;
using LoginForms.Utils;

namespace LoginForms
{
    public partial class NetworkCategories : Form
    {

        //Formulario Hijo
        string chatid;
        Json jsonNetwork;
        RestHelper rh = new RestHelper();
        string valor;
        public NetworkCategories(string chatId)
        {
            InitializeComponent();
            chatid = chatId;
            //updateNetworkCategory();
            ComboBoxGetNetwork();
        }

        private async void btnAccept_Click(object sender, EventArgs e)
        {
            try
            {
                Console.WriteLine($"Number to send: {GlobalSocket.numberToClose}");
                if (!string.IsNullOrEmpty(valor))
                {
                    await rh.updateNetworkCategories(chatid, valor);
                    this.Dispose();
                }
                else
                {
                    MessageBox.Show("No has seleccionado una red", "Omnicanal", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error[btnAccept] {ex.Message}");
            }
        }

        private void cmbNetwork_SelectedIndexChanged(object sender, EventArgs e)
        {
            NetworkItems classItems = (NetworkItems)cmbNetwork.SelectedItem;
            valor = classItems.Id;
        }


        private async void ComboBoxGetNetwork()
        {
            try
            {
                string networkCategories = await rh.getNetworkCategories();
                Json jsonNetworkCategories = jsonNetwork = JsonConvert.DeserializeObject<Json>(networkCategories);
                for (int i = 0; i < jsonNetworkCategories.data.networks.Count; i++)
                {
                    cmbNetwork.Items.Add(new NetworkItems(jsonNetwork.data.networks[i].description, jsonNetwork.data.networks[i].typification, jsonNetwork.data.networks[i].id));
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error[getNetworkCategories] {ex}");
            }
        }
    }

    class NetworkItems
    {
        public string Name;
        public string Value;
        public string Id;

        public NetworkItems(string name, string value, string id)
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
