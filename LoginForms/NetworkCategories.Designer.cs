
namespace LoginForms
{
    partial class NetworkCategories
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnAccept = new System.Windows.Forms.Button();
            this.cmbNetwork = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnAccept
            // 
            this.btnAccept.Location = new System.Drawing.Point(135, 134);
            this.btnAccept.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(100, 28);
            this.btnAccept.TabIndex = 1;
            this.btnAccept.Text = "Aceptar";
            this.btnAccept.UseVisualStyleBackColor = true;
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // cmbNetwork
            // 
            this.cmbNetwork.FormattingEnabled = true;
            this.cmbNetwork.Location = new System.Drawing.Point(107, 90);
            this.cmbNetwork.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbNetwork.Name = "cmbNetwork";
            this.cmbNetwork.Size = new System.Drawing.Size(160, 24);
            this.cmbNetwork.TabIndex = 2;
            this.cmbNetwork.Text = "Selecciona Una Sucursal";
            this.cmbNetwork.SelectedIndexChanged += new System.EventHandler(this.cmbNetwork_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(68, 42);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(246, 34);
            this.label1.TabIndex = 3;
            this.label1.Text = "Tú chat con la sucursal ha terminado.\r\nSelecciona el nombre de la sucursal.";
            // 
            // NetworkCategories
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 218);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbNetwork);
            this.Controls.Add(this.btnAccept);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "NetworkCategories";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NetworkCategories";
            this.Load += new System.EventHandler(this.NetworkCategories_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.ComboBox cmbNetwork;
        private System.Windows.Forms.Label label1;
    }
}