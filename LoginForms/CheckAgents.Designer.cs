﻿
namespace LoginForms
{
    partial class CheckAgents
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
            this.flpAgentInfo.Location = new System.Drawing.Point(13, 13);
            this.flpAgentInfo.Name = "flpAgentInfo";
            this.flpAgentInfo.Size = new System.Drawing.Size(622, 497);
            this.flpAgentInfo.TabIndex = 1;
            // 
            // CheckAgents
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(645, 521);
            this.Controls.Add(this.flpAgentInfo);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "CheckAgents";
            this.Text = "SIDI Omnichannel";
            this.Load += new System.EventHandler(this.CheckAgents_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpAgentInfo;
    }
}