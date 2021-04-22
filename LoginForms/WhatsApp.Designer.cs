
namespace LoginForms
{
    partial class WhatsApp
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
            this.cmbConnector = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rtxtResponseMessage = new System.Windows.Forms.RichTextBox();
            this.rtxtSendMessage = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnEnviar = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbConnector
            // 
            this.cmbConnector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbConnector.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbConnector.FormattingEnabled = true;
            this.cmbConnector.Location = new System.Drawing.Point(18, 46);
            this.cmbConnector.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbConnector.Name = "cmbConnector";
            this.cmbConnector.Size = new System.Drawing.Size(966, 33);
            this.cmbConnector.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(15, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(234, 29);
            this.label1.TabIndex = 2;
            this.label1.Text = "WhatsApp connector";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(18, 106);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(518, 29);
            this.label2.TabIndex = 3;
            this.label2.Text = "Telefono cliente formato: país + área + telefono";
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(20, 160);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(964, 30);
            this.textBox1.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.rtxtResponseMessage);
            this.panel1.Controls.Add(this.rtxtSendMessage);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Location = new System.Drawing.Point(18, 203);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(966, 579);
            this.panel1.TabIndex = 5;
            // 
            // rtxtResponseMessage
            // 
            this.rtxtResponseMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
<<<<<<< HEAD
            this.rtxtResponseMessage.Location = new System.Drawing.Point(325, 31);
            this.rtxtResponseMessage.Name = "rtxtResponseMessage";
            this.rtxtResponseMessage.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Horizontal;
            this.rtxtResponseMessage.Size = new System.Drawing.Size(279, 329);
            this.rtxtResponseMessage.TabIndex = 2;
            this.rtxtResponseMessage.Text = "";
            this.rtxtResponseMessage.TextChanged += new System.EventHandler(this.rtxtResponseMessage_TextChanged);
=======
            this.rtxtResponseMessage.Location = new System.Drawing.Point(488, 48);
            this.rtxtResponseMessage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rtxtResponseMessage.Name = "rtxtResponseMessage";
            this.rtxtResponseMessage.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Horizontal;
            this.rtxtResponseMessage.Size = new System.Drawing.Size(416, 504);
            this.rtxtResponseMessage.TabIndex = 2;
            this.rtxtResponseMessage.Text = "";
>>>>>>> 885d3e1ce8e521142206e01136cb5dee6f567bd5
            // 
            // rtxtSendMessage
            // 
            this.rtxtSendMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtxtSendMessage.Location = new System.Drawing.Point(9, 48);
            this.rtxtSendMessage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rtxtSendMessage.Name = "rtxtSendMessage";
            this.rtxtSendMessage.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Horizontal;
            this.rtxtSendMessage.Size = new System.Drawing.Size(416, 504);
            this.rtxtSendMessage.TabIndex = 1;
            this.rtxtSendMessage.Text = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(4, 18);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(271, 25);
            this.label3.TabIndex = 0;
            this.label3.Text = "Mensaje - emoticones: win + .";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(18, 792);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(222, 24);
            this.checkBox1.TabIndex = 6;
            this.checkBox1.Text = "Activar interaccion vía chat";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(183, 866);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 35);
            this.button1.TabIndex = 7;
            this.button1.Text = "Cerrar";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btnEnviar
            // 
            this.btnEnviar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEnviar.Location = new System.Drawing.Point(604, 866);
            this.btnEnviar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnEnviar.Name = "btnEnviar";
            this.btnEnviar.Size = new System.Drawing.Size(112, 35);
            this.btnEnviar.TabIndex = 8;
            this.btnEnviar.Text = "Enviar Mensaje";
            this.btnEnviar.UseVisualStyleBackColor = true;
<<<<<<< HEAD
            this.btnEnviar.Click += new System.EventHandler(this.btnEnviar_Click);
=======
>>>>>>> 885d3e1ce8e521142206e01136cb5dee6f567bd5
            // 
            // WhatsApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1004, 952);
            this.Controls.Add(this.btnEnviar);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbConnector);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "WhatsApp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WhatsApp";
            this.Load += new System.EventHandler(this.WhatsApp_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbConnector;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnEnviar;
        private System.Windows.Forms.RichTextBox rtxtSendMessage;
        private System.Windows.Forms.RichTextBox rtxtResponseMessage;
    }
}