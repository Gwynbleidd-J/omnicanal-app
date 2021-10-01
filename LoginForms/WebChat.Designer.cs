
namespace LoginForms
{
    partial class WebChat
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
            this.tabControlWebChats = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.txtNewTabChat = new System.Windows.Forms.TextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tabControlWebChats.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlWebChats
            // 
            this.tabControlWebChats.Controls.Add(this.tabPage1);
            this.tabControlWebChats.Location = new System.Drawing.Point(13, 13);
            this.tabControlWebChats.Margin = new System.Windows.Forms.Padding(4);
            this.tabControlWebChats.Name = "tabControlWebChats";
            this.tabControlWebChats.SelectedIndex = 0;
            this.tabControlWebChats.Size = new System.Drawing.Size(911, 601);
            this.tabControlWebChats.TabIndex = 10;
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
            // txtNewTabChat
            // 
            this.txtNewTabChat.Location = new System.Drawing.Point(5, 532);
            this.txtNewTabChat.Margin = new System.Windows.Forms.Padding(4);
            this.txtNewTabChat.Name = "txtNewTabChat";
            this.txtNewTabChat.Size = new System.Drawing.Size(651, 22);
            this.txtNewTabChat.TabIndex = 4;
            this.txtNewTabChat.Visible = false;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(22, 136);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(4);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(219, 472);
            this.richTextBox1.TabIndex = 9;
            this.richTextBox1.Text = "";
            this.richTextBox1.Visible = false;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(22, 38);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(219, 22);
            this.textBox1.TabIndex = 8;
            this.textBox1.Visible = false;
            // 
            // WebChat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(952, 637);
            this.Controls.Add(this.tabControlWebChats);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.textBox1);
            this.Name = "WebChat";
            this.Text = "WebChat";
            this.tabControlWebChats.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TabControl tabControlWebChats;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtNewTabChat;
        public System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TextBox textBox1;
    }
}