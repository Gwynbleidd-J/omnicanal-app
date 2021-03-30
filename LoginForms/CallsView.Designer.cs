
namespace LoginForms
{
    partial class CallsView
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
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnLine1 = new System.Windows.Forms.Button();
            this.btnLine2 = new System.Windows.Forms.Button();
            this.btnLine3 = new System.Windows.Forms.Button();
            this.btnDial = new System.Windows.Forms.Button();
            this.btnTransfer = new System.Windows.Forms.Button();
            this.txtTransferTarget = new System.Windows.Forms.TextBox();
            this.txtLineTarget = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Phone";
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(16, 44);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 305);
            this.panel1.TabIndex = 1;
            // 
            // btnLine1
            // 
            this.btnLine1.Location = new System.Drawing.Point(197, 15);
            this.btnLine1.Name = "btnLine1";
            this.btnLine1.Size = new System.Drawing.Size(56, 23);
            this.btnLine1.TabIndex = 2;
            this.btnLine1.Text = "1";
            this.btnLine1.UseVisualStyleBackColor = true;
            // 
            // btnLine2
            // 
            this.btnLine2.Location = new System.Drawing.Point(259, 15);
            this.btnLine2.Name = "btnLine2";
            this.btnLine2.Size = new System.Drawing.Size(56, 23);
            this.btnLine2.TabIndex = 3;
            this.btnLine2.Text = "2";
            this.btnLine2.UseVisualStyleBackColor = true;
            // 
            // btnLine3
            // 
            this.btnLine3.Location = new System.Drawing.Point(321, 15);
            this.btnLine3.Name = "btnLine3";
            this.btnLine3.Size = new System.Drawing.Size(56, 23);
            this.btnLine3.TabIndex = 4;
            this.btnLine3.Text = "3";
            this.btnLine3.UseVisualStyleBackColor = true;
            // 
            // btnDial
            // 
            this.btnDial.Location = new System.Drawing.Point(223, 144);
            this.btnDial.Name = "btnDial";
            this.btnDial.Size = new System.Drawing.Size(154, 204);
            this.btnDial.TabIndex = 5;
            this.btnDial.Text = "Dial";
            this.btnDial.UseVisualStyleBackColor = true;
            // 
            // btnTransfer
            // 
            this.btnTransfer.Location = new System.Drawing.Point(16, 368);
            this.btnTransfer.Name = "btnTransfer";
            this.btnTransfer.Size = new System.Drawing.Size(191, 23);
            this.btnTransfer.TabIndex = 6;
            this.btnTransfer.Text = "Transferencia";
            this.btnTransfer.UseVisualStyleBackColor = true;
            // 
            // txtTransferTarget
            // 
            this.txtTransferTarget.Location = new System.Drawing.Point(213, 368);
            this.txtTransferTarget.Name = "txtTransferTarget";
            this.txtTransferTarget.Size = new System.Drawing.Size(162, 20);
            this.txtTransferTarget.TabIndex = 7;
            // 
            // txtLineTarget
            // 
            this.txtLineTarget.Location = new System.Drawing.Point(14, 413);
            this.txtLineTarget.Multiline = true;
            this.txtLineTarget.Name = "txtLineTarget";
            this.txtLineTarget.Size = new System.Drawing.Size(361, 32);
            this.txtLineTarget.TabIndex = 8;
            // 
            // CallsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(387, 457);
            this.Controls.Add(this.txtLineTarget);
            this.Controls.Add(this.txtTransferTarget);
            this.Controls.Add(this.btnTransfer);
            this.Controls.Add(this.btnDial);
            this.Controls.Add(this.btnLine3);
            this.Controls.Add(this.btnLine2);
            this.Controls.Add(this.btnLine1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Name = "CallsView";
            this.Text = "CallsView";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnLine1;
        private System.Windows.Forms.Button btnLine2;
        private System.Windows.Forms.Button btnLine3;
        private System.Windows.Forms.Button btnDial;
        private System.Windows.Forms.Button btnTransfer;
        private System.Windows.Forms.TextBox txtTransferTarget;
        private System.Windows.Forms.TextBox txtLineTarget;
    }
}