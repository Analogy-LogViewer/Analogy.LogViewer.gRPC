namespace Analogy.LogServer.Tests
{
    partial class TestForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnProducer = new System.Windows.Forms.Button();
            this.btnConsumer = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btnProducer
            // 
            this.btnProducer.Location = new System.Drawing.Point(12, 12);
            this.btnProducer.Name = "btnProducer";
            this.btnProducer.Size = new System.Drawing.Size(166, 54);
            this.btnProducer.TabIndex = 0;
            this.btnProducer.Text = "Produce Messages";
            this.btnProducer.UseVisualStyleBackColor = true;
            this.btnProducer.Click += new System.EventHandler(this.btnProducer_Click);
            // 
            // btnConsumer
            // 
            this.btnConsumer.Location = new System.Drawing.Point(12, 72);
            this.btnConsumer.Name = "btnConsumer";
            this.btnConsumer.Size = new System.Drawing.Size(166, 54);
            this.btnConsumer.TabIndex = 1;
            this.btnConsumer.Text = "Consume Messages";
            this.btnConsumer.UseVisualStyleBackColor = true;
            this.btnConsumer.Click += new System.EventHandler(this.btnConsumer_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Location = new System.Drawing.Point(194, 19);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(588, 419);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = "";
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.btnConsumer);
            this.Controls.Add(this.btnProducer);
            this.Name = "TestForm";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnProducer;
        private System.Windows.Forms.Button btnConsumer;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}

