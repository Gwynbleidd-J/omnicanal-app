
namespace LoginForms
{
    partial class Prueba
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.txtNewTabChat = new System.Windows.Forms.TextBox();
            this.tabControlChats = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnBeginChat = new System.Windows.Forms.Button();
            this.tabControlChats.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(17, 41);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(219, 22);
            this.textBox1.TabIndex = 0;
            this.textBox1.Visible = false;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(17, 139);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(4);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(219, 472);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            this.richTextBox1.Visible = false;
            // 
            // txtNewTabChat
            // 
            this.txtNewTabChat.Location = new System.Drawing.Point(5, 532);
            this.txtNewTabChat.Margin = new System.Windows.Forms.Padding(4);
            this.txtNewTabChat.Name = "txtNewTabChat";
            this.txtNewTabChat.Size = new System.Drawing.Size(651, 22);
            this.txtNewTabChat.TabIndex = 4;
            this.txtNewTabChat.Visible = false;
            // 
            // tabControlChats
            // 
            this.tabControlChats.Controls.Add(this.tabPage1);
            this.tabControlChats.Location = new System.Drawing.Point(8, 16);
            this.tabControlChats.Margin = new System.Windows.Forms.Padding(4);
            this.tabControlChats.Name = "tabControlChats";
            this.tabControlChats.SelectedIndex = 0;
            this.tabControlChats.Size = new System.Drawing.Size(911, 601);
            this.tabControlChats.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.button2);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.txtNewTabChat);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(903, 572);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(781, 528);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(115, 28);
            this.button2.TabIndex = 6;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(664, 528);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(109, 28);
            this.button1.TabIndex = 5;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            // 
            // btnBeginChat
            // 
            this.btnBeginChat.Location = new System.Drawing.Point(8, 621);
            this.btnBeginChat.Margin = new System.Windows.Forms.Padding(4);
            this.btnBeginChat.Name = "btnBeginChat";
            this.btnBeginChat.Size = new System.Drawing.Size(138, 37);
            this.btnBeginChat.TabIndex = 7;
            this.btnBeginChat.Text = "Enviar Mensaje";
            this.btnBeginChat.UseVisualStyleBackColor = true;
            this.btnBeginChat.Click += new System.EventHandler(this.btnBeginChat_Click);
            // 
            // Prueba
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(932, 666);
            this.Controls.Add(this.btnBeginChat);
            this.Controls.Add(this.tabControlChats);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.textBox1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Prueba";
            this.Tag = "Chats";
            this.Text = "Chats";
            this.Load += new System.EventHandler(this.Prueba_Load);
            this.tabControlChats.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        public System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TextBox txtNewTabChat;
        public System.Windows.Forms.TabControl tabControlChats;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnBeginChat;
    }
}