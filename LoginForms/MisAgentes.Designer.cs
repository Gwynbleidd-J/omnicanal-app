
namespace LoginForms
{
    partial class MisAgentes
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
            this.flpAgentInfo = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // flpAgentInfo
            // 
            this.flpAgentInfo.AutoSize = true;
            this.flpAgentInfo.Location = new System.Drawing.Point(17, 16);
            this.flpAgentInfo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.flpAgentInfo.Name = "flpAgentInfo";
            this.flpAgentInfo.Size = new System.Drawing.Size(834, 529);
            this.flpAgentInfo.TabIndex = 0;
            // 
            // MisAgentes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(864, 558);
            this.Controls.Add(this.flpAgentInfo);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MisAgentes";
            this.Text = "MisAgentes";
            this.Load += new System.EventHandler(this.MisAgentes_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpAgentInfo;
    }
}