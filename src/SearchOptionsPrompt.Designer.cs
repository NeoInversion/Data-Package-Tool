﻿namespace Data_Package_Images
{
    partial class SearchOptionsPrompt
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
            this.excludeGuildsCb = new System.Windows.Forms.CheckBox();
            this.excludeDmsCb = new System.Windows.Forms.CheckBox();
            this.excludeGroupDmsCb = new System.Windows.Forms.CheckBox();
            this.excludedIdsLb = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.saveBtn = new System.Windows.Forms.Button();
            this.addBtn = new System.Windows.Forms.Button();
            this.removeBtn = new System.Windows.Forms.Button();
            this.exactMatchRb = new System.Windows.Forms.RadioButton();
            this.wordsMatchRb = new System.Windows.Forms.RadioButton();
            this.regexMatchRb = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.removeWhitelistBtn = new System.Windows.Forms.Button();
            this.addWhitelistBtn = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.whitelistIdsLb = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // excludeGuildsCb
            // 
            this.excludeGuildsCb.AutoSize = true;
            this.excludeGuildsCb.Location = new System.Drawing.Point(137, 26);
            this.excludeGuildsCb.Name = "excludeGuildsCb";
            this.excludeGuildsCb.Size = new System.Drawing.Size(135, 17);
            this.excludeGuildsCb.TabIndex = 0;
            this.excludeGuildsCb.Text = "Exclude guild channels";
            this.excludeGuildsCb.UseVisualStyleBackColor = true;
            // 
            // excludeDmsCb
            // 
            this.excludeDmsCb.AutoSize = true;
            this.excludeDmsCb.Location = new System.Drawing.Point(137, 49);
            this.excludeDmsCb.Name = "excludeDmsCb";
            this.excludeDmsCb.Size = new System.Drawing.Size(127, 17);
            this.excludeDmsCb.TabIndex = 1;
            this.excludeDmsCb.Text = "Exclude dm channels";
            this.excludeDmsCb.UseVisualStyleBackColor = true;
            // 
            // excludeGroupDmsCb
            // 
            this.excludeGroupDmsCb.AutoSize = true;
            this.excludeGroupDmsCb.Location = new System.Drawing.Point(137, 72);
            this.excludeGroupDmsCb.Name = "excludeGroupDmsCb";
            this.excludeGroupDmsCb.Size = new System.Drawing.Size(157, 17);
            this.excludeGroupDmsCb.TabIndex = 2;
            this.excludeGroupDmsCb.Text = "Exclude group dm channels";
            this.excludeGroupDmsCb.UseVisualStyleBackColor = true;
            // 
            // excludedIdsLb
            // 
            this.excludedIdsLb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.excludedIdsLb.FormattingEnabled = true;
            this.excludedIdsLb.Location = new System.Drawing.Point(12, 120);
            this.excludedIdsLb.Name = "excludedIdsLb";
            this.excludedIdsLb.Size = new System.Drawing.Size(249, 95);
            this.excludedIdsLb.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 104);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(181, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Exclude specific channels/guilds ids:";
            // 
            // saveBtn
            // 
            this.saveBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.saveBtn.Location = new System.Drawing.Point(109, 351);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(75, 23);
            this.saveBtn.TabIndex = 5;
            this.saveBtn.Text = "Save";
            this.saveBtn.UseVisualStyleBackColor = true;
            this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
            // 
            // addBtn
            // 
            this.addBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.addBtn.Location = new System.Drawing.Point(265, 120);
            this.addBtn.Name = "addBtn";
            this.addBtn.Size = new System.Drawing.Size(24, 21);
            this.addBtn.TabIndex = 6;
            this.addBtn.Text = "+";
            this.addBtn.UseVisualStyleBackColor = true;
            this.addBtn.Click += new System.EventHandler(this.addBtn_Click);
            // 
            // removeBtn
            // 
            this.removeBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.removeBtn.Location = new System.Drawing.Point(265, 147);
            this.removeBtn.Name = "removeBtn";
            this.removeBtn.Size = new System.Drawing.Size(24, 21);
            this.removeBtn.TabIndex = 7;
            this.removeBtn.Text = "-";
            this.removeBtn.UseVisualStyleBackColor = true;
            this.removeBtn.Click += new System.EventHandler(this.removeBtn_Click);
            // 
            // exactMatchRb
            // 
            this.exactMatchRb.AutoSize = true;
            this.exactMatchRb.Location = new System.Drawing.Point(12, 25);
            this.exactMatchRb.Name = "exactMatchRb";
            this.exactMatchRb.Size = new System.Drawing.Size(84, 17);
            this.exactMatchRb.TabIndex = 8;
            this.exactMatchRb.Text = "Exact match";
            this.exactMatchRb.UseVisualStyleBackColor = true;
            // 
            // wordsMatchRb
            // 
            this.wordsMatchRb.AutoSize = true;
            this.wordsMatchRb.Location = new System.Drawing.Point(12, 48);
            this.wordsMatchRb.Name = "wordsMatchRb";
            this.wordsMatchRb.Size = new System.Drawing.Size(88, 17);
            this.wordsMatchRb.TabIndex = 9;
            this.wordsMatchRb.Text = "Words match";
            this.wordsMatchRb.UseVisualStyleBackColor = true;
            // 
            // regexMatchRb
            // 
            this.regexMatchRb.AutoSize = true;
            this.regexMatchRb.Location = new System.Drawing.Point(12, 71);
            this.regexMatchRb.Name = "regexMatchRb";
            this.regexMatchRb.Size = new System.Drawing.Size(88, 17);
            this.regexMatchRb.TabIndex = 10;
            this.regexMatchRb.Text = "Regex match";
            this.regexMatchRb.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Search type:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(134, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Exclusions:";
            // 
            // removeWhitelistBtn
            // 
            this.removeWhitelistBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.removeWhitelistBtn.Location = new System.Drawing.Point(265, 277);
            this.removeWhitelistBtn.Name = "removeWhitelistBtn";
            this.removeWhitelistBtn.Size = new System.Drawing.Size(24, 21);
            this.removeWhitelistBtn.TabIndex = 16;
            this.removeWhitelistBtn.Text = "-";
            this.removeWhitelistBtn.UseVisualStyleBackColor = true;
            this.removeWhitelistBtn.Click += new System.EventHandler(this.removeWhitelistBtn_Click);
            // 
            // addWhitelistBtn
            // 
            this.addWhitelistBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.addWhitelistBtn.Location = new System.Drawing.Point(265, 250);
            this.addWhitelistBtn.Name = "addWhitelistBtn";
            this.addWhitelistBtn.Size = new System.Drawing.Size(24, 21);
            this.addWhitelistBtn.TabIndex = 15;
            this.addWhitelistBtn.Text = "+";
            this.addWhitelistBtn.UseVisualStyleBackColor = true;
            this.addWhitelistBtn.Click += new System.EventHandler(this.addWhitelistBtn_Click);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 234);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(210, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Search only in specific channels/guilds ids:";
            // 
            // whitelistIdsLb
            // 
            this.whitelistIdsLb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.whitelistIdsLb.FormattingEnabled = true;
            this.whitelistIdsLb.Location = new System.Drawing.Point(12, 250);
            this.whitelistIdsLb.Name = "whitelistIdsLb";
            this.whitelistIdsLb.Size = new System.Drawing.Size(249, 95);
            this.whitelistIdsLb.TabIndex = 13;
            // 
            // SearchOptionsPrompt
            // 
            this.AcceptButton = this.saveBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(293, 378);
            this.Controls.Add(this.removeWhitelistBtn);
            this.Controls.Add(this.addWhitelistBtn);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.whitelistIdsLb);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.regexMatchRb);
            this.Controls.Add(this.wordsMatchRb);
            this.Controls.Add(this.exactMatchRb);
            this.Controls.Add(this.removeBtn);
            this.Controls.Add(this.addBtn);
            this.Controls.Add(this.saveBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.excludedIdsLb);
            this.Controls.Add(this.excludeGroupDmsCb);
            this.Controls.Add(this.excludeDmsCb);
            this.Controls.Add(this.excludeGuildsCb);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SearchOptionsPrompt";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Search Options";
            this.Load += new System.EventHandler(this.FiltersPrompt_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox excludeGuildsCb;
        private System.Windows.Forms.CheckBox excludeDmsCb;
        private System.Windows.Forms.CheckBox excludeGroupDmsCb;
        private System.Windows.Forms.ListBox excludedIdsLb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button saveBtn;
        private System.Windows.Forms.Button addBtn;
        private System.Windows.Forms.Button removeBtn;
        private System.Windows.Forms.RadioButton exactMatchRb;
        private System.Windows.Forms.RadioButton wordsMatchRb;
        private System.Windows.Forms.RadioButton regexMatchRb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button removeWhitelistBtn;
        private System.Windows.Forms.Button addWhitelistBtn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox whitelistIdsLb;
    }
}