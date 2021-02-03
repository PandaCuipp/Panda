namespace CreateNCSql
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.txtDate = new System.Windows.Forms.TextBox();
            this.txtTotalMoney = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.txtNo = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.txtNo)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 201);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(757, 497);
            this.textBox1.TabIndex = 0;
            // 
            // txtDate
            // 
            this.txtDate.Location = new System.Drawing.Point(194, 11);
            this.txtDate.Name = "txtDate";
            this.txtDate.PlaceholderText = "单据日期";
            this.txtDate.Size = new System.Drawing.Size(131, 23);
            this.txtDate.TabIndex = 1;
            // 
            // txtTotalMoney
            // 
            this.txtTotalMoney.Location = new System.Drawing.Point(331, 12);
            this.txtTotalMoney.Name = "txtTotalMoney";
            this.txtTotalMoney.PlaceholderText = "总金额";
            this.txtTotalMoney.Size = new System.Drawing.Size(131, 23);
            this.txtTotalMoney.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(93, 172);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "生成脚本";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtNo
            // 
            this.txtNo.Location = new System.Drawing.Point(75, 12);
            this.txtNo.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.txtNo.Name = "txtNo";
            this.txtNo.Size = new System.Drawing.Size(113, 23);
            this.txtNo.TabIndex = 4;
            this.txtNo.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "单据尾号：";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 172);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "自动赋值";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(781, 710);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtNo);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtTotalMoney);
            this.Controls.Add(this.txtDate);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.txtNo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox txtDate;
        private System.Windows.Forms.TextBox txtTotalMoney;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.NumericUpDown txtNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
    }
}

