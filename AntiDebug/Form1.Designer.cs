namespace AntiDebug
{
    partial class fMain
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.fOpen = new System.Windows.Forms.Button();
            this.fSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(146, 20);
            this.textBox1.TabIndex = 0;
            // 
            // fOpen
            // 
            this.fOpen.Location = new System.Drawing.Point(168, 10);
            this.fOpen.Name = "fOpen";
            this.fOpen.Size = new System.Drawing.Size(75, 23);
            this.fOpen.TabIndex = 1;
            this.fOpen.Text = "File";
            this.fOpen.UseVisualStyleBackColor = true;
            this.fOpen.Click += new System.EventHandler(this.button1_Click);
            // 
            // fSave
            // 
            this.fSave.Location = new System.Drawing.Point(12, 38);
            this.fSave.Name = "fSave";
            this.fSave.Size = new System.Drawing.Size(231, 23);
            this.fSave.TabIndex = 2;
            this.fSave.Text = "Bummmmmp";
            this.fSave.UseVisualStyleBackColor = true;
            this.fSave.Click += new System.EventHandler(this.button2_Click);
            // 
            // fMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(260, 72);
            this.Controls.Add(this.fSave);
            this.Controls.Add(this.fOpen);
            this.Controls.Add(this.textBox1);
            this.Name = "fMain";
            this.Text = "Anti-Debug";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button fOpen;
        private System.Windows.Forms.Button fSave;
    }
}

