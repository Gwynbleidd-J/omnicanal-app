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

namespace LoginForms
{
    public partial class FormPrincipal : Form
    {
        RestHelper rh = new RestHelper();
        public FormPrincipal()
        {
            InitializeComponent();
        }
    }
}
