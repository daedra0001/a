namespace Crawler
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button_weibo = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_keyword = new System.Windows.Forms.TextBox();
            this.label_weibostate = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_maxthreadcnt = new System.Windows.Forms.TextBox();
            this.textBox_period = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button_weibo
            // 
            this.button_weibo.Location = new System.Drawing.Point(564, 46);
            this.button_weibo.Name = "button_weibo";
            this.button_weibo.Size = new System.Drawing.Size(75, 23);
            this.button_weibo.TabIndex = 0;
            this.button_weibo.Text = "开始";
            this.button_weibo.UseVisualStyleBackColor = true;
            this.button_weibo.Click += new System.EventHandler(this.button_weibo_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "微博关键词";
            // 
            // textBox_keyword
            // 
            this.textBox_keyword.Location = new System.Drawing.Point(86, 48);
            this.textBox_keyword.Name = "textBox_keyword";
            this.textBox_keyword.Size = new System.Drawing.Size(100, 21);
            this.textBox_keyword.TabIndex = 2;
            // 
            // label_weibostate
            // 
            this.label_weibostate.AutoSize = true;
            this.label_weibostate.Location = new System.Drawing.Point(95, 97);
            this.label_weibostate.Name = "label_weibostate";
            this.label_weibostate.Size = new System.Drawing.Size(0, 12);
            this.label_weibostate.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(209, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "最大并发数";
            // 
            // textBox_maxthreadcnt
            // 
            this.textBox_maxthreadcnt.Location = new System.Drawing.Point(280, 48);
            this.textBox_maxthreadcnt.Name = "textBox_maxthreadcnt";
            this.textBox_maxthreadcnt.Size = new System.Drawing.Size(100, 21);
            this.textBox_maxthreadcnt.TabIndex = 5;
            // 
            // textBox_period
            // 
            this.textBox_period.Location = new System.Drawing.Point(428, 48);
            this.textBox_period.Name = "textBox_period";
            this.textBox_period.Size = new System.Drawing.Size(100, 21);
            this.textBox_period.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(393, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "间隔";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 178);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(695, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "_________________________________________________________________________________" +
    "__________________________________";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(762, 551);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_period);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox_maxthreadcnt);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label_weibostate);
            this.Controls.Add(this.textBox_keyword);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_weibo);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_weibo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_keyword;
        private System.Windows.Forms.Label label_weibostate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_maxthreadcnt;
        private System.Windows.Forms.TextBox textBox_period;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
    }
}

