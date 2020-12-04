
namespace Analogy.LogViewer.gRPC
{
    partial class grpcUserSettingsUC
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtbRealTimeServerURL = new System.Windows.Forms.TextBox();
            this.txtbSelfHostingServerURL = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(242, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Analogy gRPC real time server URL:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(168, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Self Hosting Server URL:";
            // 
            // txtbRealTimeServerURL
            // 
            this.txtbRealTimeServerURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtbRealTimeServerURL.Location = new System.Drawing.Point(288, 10);
            this.txtbRealTimeServerURL.Name = "txtbRealTimeServerURL";
            this.txtbRealTimeServerURL.Size = new System.Drawing.Size(312, 27);
            this.txtbRealTimeServerURL.TabIndex = 2;
            this.txtbRealTimeServerURL.TextChanged += new System.EventHandler(this.txtbRealTimeServerURL_TextChanged);
            // 
            // txtbSelfHostingServerURL
            // 
            this.txtbSelfHostingServerURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtbSelfHostingServerURL.Location = new System.Drawing.Point(288, 43);
            this.txtbSelfHostingServerURL.Name = "txtbSelfHostingServerURL";
            this.txtbSelfHostingServerURL.Size = new System.Drawing.Size(312, 27);
            this.txtbSelfHostingServerURL.TabIndex = 3;
            this.txtbSelfHostingServerURL.TextChanged += new System.EventHandler(this.txtbSelfHostingServerURL_TextChanged);
            // 
            // grpcUserSettingsUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtbSelfHostingServerURL);
            this.Controls.Add(this.txtbRealTimeServerURL);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "grpcUserSettingsUC";
            this.Size = new System.Drawing.Size(618, 374);
            this.Load += new System.EventHandler(this.grpcUserSettingsUC_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtbRealTimeServerURL;
        private System.Windows.Forms.TextBox txtbSelfHostingServerURL;
    }
}
