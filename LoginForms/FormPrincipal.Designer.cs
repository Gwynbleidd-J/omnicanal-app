
namespace LoginForms
{
    partial class FormPrincipal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPrincipal));
            this.btnTelegram = new System.Windows.Forms.Button();
            this.btnLlamadas = new System.Windows.Forms.Button();
            this.btnWhatsApp = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnTelegram
            // 
            this.btnTelegram.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTelegram.Image = ((System.Drawing.Image)(resources.GetObject("btnTelegram.Image")));
            this.btnTelegram.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTelegram.Location = new System.Drawing.Point(13, 72);
            this.btnTelegram.Name = "btnTelegram";
            this.btnTelegram.Size = new System.Drawing.Size(95, 38);
            this.btnTelegram.TabIndex = 1;
            this.btnTelegram.Text = "Telegram";
            this.btnTelegram.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnTelegram.UseVisualStyleBackColor = true;
            this.btnTelegram.Click += new System.EventHandler(this.btnTelegram_Click);
            // 
            // btnLlamadas
            // 
            this.btnLlamadas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLlamadas.Image = ((System.Drawing.Image)(resources.GetObject("btnLlamadas.Image")));
            this.btnLlamadas.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLlamadas.Location = new System.Drawing.Point(12, 126);
            this.btnLlamadas.Name = "btnLlamadas";
            this.btnLlamadas.Size = new System.Drawing.Size(95, 40);
            this.btnLlamadas.TabIndex = 2;
            this.btnLlamadas.Text = "Llamadas";
            this.btnLlamadas.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLlamadas.UseVisualStyleBackColor = true;
            this.btnLlamadas.Click += new System.EventHandler(this.btnLlamadas_Click);
            // 
            // btnWhatsApp
            // 
            this.btnWhatsApp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWhatsApp.Image = ((System.Drawing.Image)(resources.GetObject("btnWhatsApp.Image")));
            this.btnWhatsApp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnWhatsApp.Location = new System.Drawing.Point(12, 12);
            this.btnWhatsApp.Name = "btnWhatsApp";
            this.btnWhatsApp.Size = new System.Drawing.Size(95, 38);
            this.btnWhatsApp.TabIndex = 3;
            this.btnWhatsApp.Text = "Telegram";
            this.btnWhatsApp.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnWhatsApp.UseVisualStyleBackColor = true;
            this.btnWhatsApp.Click += new System.EventHandler(this.btnWhatsApp_Click_1);
            // 
            // FormPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnWhatsApp);
            this.Controls.Add(this.btnLlamadas);
            this.Controls.Add(this.btnTelegram);
            this.Name = "FormPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormPrincipal";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnTelegram;
        private System.Windows.Forms.Button btnLlamadas;
        private System.Windows.Forms.Button btnWhatsApp;
    }
}