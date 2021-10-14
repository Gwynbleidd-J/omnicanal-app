using LoginForms.Shared;
using System;
using System.Windows.Forms;

namespace LoginForms
{
    public partial class ChangeMaxActiveChats : Form
    {

        string AgentID;
        string valor;
        RestHelper rh = new RestHelper();

        public ChangeMaxActiveChats(string agentId)
        {
            InitializeComponent();
            AgentID = agentId;
        }

        private async void btnAccept_Click(object sender, EventArgs e)
        {
            try
            {
                int intTemp;
                string temp = textBox1.Text;
                if (temp != null && int.TryParse(temp, out intTemp))
                {
                    if (intTemp > 0)
                    {
                        valor = temp;
                        await rh.updateAgentMaxActiveChats(AgentID, valor);
                        MessageBox.Show("Chats simultaneos actualizados correctamente");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Por favor ingrese un dato positivo");
                    }
                }
                else
                {
                    MessageBox.Show("Por favor ingrese un dato valido");
                }
            }
            catch (Exception _e)
            {
                throw _e;
            }

        }
    }
}
