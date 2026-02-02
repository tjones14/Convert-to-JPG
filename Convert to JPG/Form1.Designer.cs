
namespace HEICtoJPG
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            dropPanel = new System.Windows.Forms.Panel();
            label1 = new System.Windows.Forms.Label();
            button1 = new System.Windows.Forms.Button();
            listBox1 = new System.Windows.Forms.ListBox();
            label2 = new System.Windows.Forms.Label();
            radioJPG = new System.Windows.Forms.RadioButton();
            radioPDF = new System.Windows.Forms.RadioButton();
            groupBox1 = new System.Windows.Forms.GroupBox();
            label3 = new System.Windows.Forms.Label();
            chkNormalizeSize = new System.Windows.Forms.CheckBox();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // dropPanel
            // 
            dropPanel.AllowDrop = true;
            dropPanel.AutoSize = true;
            dropPanel.BackColor = System.Drawing.Color.LightSteelBlue;
            dropPanel.Location = new System.Drawing.Point(12, 43);
            dropPanel.Name = "dropPanel";
            dropPanel.Size = new System.Drawing.Size(623, 340);
            dropPanel.TabIndex = 0;
            dropPanel.DragDrop += DropPanel_DragDrop;
            dropPanel.DragEnter += DropPanel_DragEnter;
            // 
            // label1
            // 
            label1.Dock = System.Windows.Forms.DockStyle.Top;
            label1.Font = new System.Drawing.Font("Leelawadee", 20F, System.Drawing.FontStyle.Bold);
            label1.ForeColor = System.Drawing.Color.RoyalBlue;
            label1.Location = new System.Drawing.Point(0, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(647, 40);
            label1.TabIndex = 0;
            label1.Text = "Drag and Drop images to be converted";
            label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // button1
            // 
            button1.BackColor = System.Drawing.Color.RoyalBlue;
            button1.Font = new System.Drawing.Font("Leelawadee", 9F, System.Drawing.FontStyle.Bold);
            button1.ForeColor = System.Drawing.Color.White;
            button1.Location = new System.Drawing.Point(504, 447);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(131, 36);
            button1.TabIndex = 1;
            button1.Text = "Process Images";
            button1.UseVisualStyleBackColor = false;
            button1.Click += Button1_Click;
            // 
            // listBox1
            // 
            listBox1.Enabled = false;
            listBox1.FormattingEnabled = true;
            listBox1.Location = new System.Drawing.Point(12, 434);
            listBox1.Name = "listBox1";
            listBox1.Size = new System.Drawing.Size(327, 49);
            listBox1.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(15, 412);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(105, 15);
            label2.TabIndex = 3;
            label2.Text = "Processing Images";
            // 
            // radioJPG
            // 
            radioJPG.AutoSize = true;
            radioJPG.Location = new System.Drawing.Point(21, 14);
            radioJPG.Name = "radioJPG";
            radioJPG.Size = new System.Drawing.Size(44, 19);
            radioJPG.TabIndex = 4;
            radioJPG.TabStop = true;
            radioJPG.Text = "JPG";
            radioJPG.UseVisualStyleBackColor = true;
            // 
            // radioPDF
            // 
            radioPDF.AutoSize = true;
            radioPDF.Location = new System.Drawing.Point(71, 14);
            radioPDF.Name = "radioPDF";
            radioPDF.Size = new System.Drawing.Size(46, 19);
            radioPDF.TabIndex = 5;
            radioPDF.TabStop = true;
            radioPDF.Text = "PDF";
            radioPDF.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(radioPDF);
            groupBox1.Controls.Add(radioJPG);
            groupBox1.Location = new System.Drawing.Point(504, 383);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(128, 45);
            groupBox1.TabIndex = 6;
            groupBox1.TabStop = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(431, 399);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(68, 15);
            label3.TabIndex = 7;
            label3.Text = "Convert To:";
            // 
            // chkNormalizeSize
            // 
            chkNormalizeSize.AutoSize = true;
            chkNormalizeSize.Enabled = false;
            chkNormalizeSize.Location = new System.Drawing.Point(530, 422);
            chkNormalizeSize.Name = "chkNormalizeSize";
            chkNormalizeSize.Size = new System.Drawing.Size(102, 19);
            chkNormalizeSize.TabIndex = 8;
            chkNormalizeSize.Text = "Normalize size";
            chkNormalizeSize.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(647, 495);
            Controls.Add(chkNormalizeSize);
            Controls.Add(label3);
            Controls.Add(groupBox1);
            Controls.Add(label2);
            Controls.Add(listBox1);
            Controls.Add(button1);
            Controls.Add(dropPanel);
            Controls.Add(label1);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "Convert to Jpg";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel dropPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton radioJPG;
        private System.Windows.Forms.RadioButton radioPDF;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkNormalizeSize;
    }
}

