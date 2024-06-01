namespace EdgeUninstaller
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button uninstallButton;
        private System.Windows.Forms.Label completeLabel;
        private System.Windows.Forms.Button restartButton;
        private System.Windows.Forms.CheckBox agreeCheckBox;
        private System.Windows.Forms.CheckBox webViewCheckBox;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            uninstallButton = new System.Windows.Forms.Button();
            completeLabel = new System.Windows.Forms.Label();
            restartButton = new System.Windows.Forms.Button();
            agreeCheckBox = new System.Windows.Forms.CheckBox();
            webViewCheckBox = new System.Windows.Forms.CheckBox();
            SuspendLayout();
            // 
            // uninstallButton
            // 
            uninstallButton.BackColor = System.Drawing.Color.White;
            uninstallButton.Enabled = false;
            uninstallButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            uninstallButton.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 128);
            uninstallButton.ForeColor = System.Drawing.Color.Red;
            uninstallButton.Location = new System.Drawing.Point(1, -1);
            uninstallButton.Margin = new System.Windows.Forms.Padding(0);
            uninstallButton.Name = "uninstallButton";
            uninstallButton.Size = new System.Drawing.Size(252, 76);
            uninstallButton.TabIndex = 0;
            uninstallButton.Text = "Microsoft Edgeをアンインストール";
            uninstallButton.UseVisualStyleBackColor = false;
            uninstallButton.Click += uninstallButton_Click;
            // 
            // completeLabel
            // 
            completeLabel.AutoSize = true;
            completeLabel.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            completeLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            completeLabel.Font = new System.Drawing.Font("Yu Gothic UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 128);
            completeLabel.ForeColor = System.Drawing.Color.Yellow;
            completeLabel.Location = new System.Drawing.Point(256, -1);
            completeLabel.Name = "completeLabel";
            completeLabel.Size = new System.Drawing.Size(62, 19);
            completeLabel.TabIndex = 1;
            completeLabel.Text = "削除完了";
            completeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            completeLabel.Visible = false;
            // 
            // restartButton
            // 
            restartButton.BackColor = System.Drawing.Color.White;
            restartButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            restartButton.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 128);
            restartButton.ForeColor = System.Drawing.Color.Red;
            restartButton.Location = new System.Drawing.Point(1, -1);
            restartButton.Margin = new System.Windows.Forms.Padding(0);
            restartButton.Name = "restartButton";
            restartButton.Size = new System.Drawing.Size(252, 76);
            restartButton.TabIndex = 2;
            restartButton.Text = "システムを再起動";
            restartButton.UseVisualStyleBackColor = false;
            restartButton.Visible = false;
            restartButton.Click += restartButton_Click;
            // 
            // agreeCheckBox
            // 
            agreeCheckBox.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            agreeCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            agreeCheckBox.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            agreeCheckBox.ForeColor = System.Drawing.Color.FromArgb(0, 64, 0);
            agreeCheckBox.Location = new System.Drawing.Point(256, 18);
            agreeCheckBox.Margin = new System.Windows.Forms.Padding(0);
            agreeCheckBox.Name = "agreeCheckBox";
            agreeCheckBox.Size = new System.Drawing.Size(76, 29);
            agreeCheckBox.TabIndex = 3;
            agreeCheckBox.Text = "確認";
            agreeCheckBox.UseVisualStyleBackColor = false;
            agreeCheckBox.CheckedChanged += agreeCheckBox_CheckedChanged;
            // 
            // webViewCheckBox
            // 
            webViewCheckBox.BackColor = System.Drawing.Color.Transparent;
            webViewCheckBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            webViewCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            webViewCheckBox.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            webViewCheckBox.ForeColor = System.Drawing.Color.Red;
            webViewCheckBox.Location = new System.Drawing.Point(256, 39);
            webViewCheckBox.Margin = new System.Windows.Forms.Padding(0);
            webViewCheckBox.Name = "webViewCheckBox";
            webViewCheckBox.Size = new System.Drawing.Size(90, 36);
            webViewCheckBox.TabIndex = 4;
            webViewCheckBox.Text = "最終確認";
            webViewCheckBox.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            webViewCheckBox.UseMnemonic = false;
            webViewCheckBox.UseVisualStyleBackColor = false;
            webViewCheckBox.CheckedChanged += webViewCheckBox_CheckedChanged;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.ControlDarkDark;
            ClientSize = new System.Drawing.Size(326, 72);
            Controls.Add(webViewCheckBox);
            Controls.Add(agreeCheckBox);
            Controls.Add(restartButton);
            Controls.Add(completeLabel);
            Controls.Add(uninstallButton);
            Name = "MainForm";
            Text = "Microsoft Edge アンインストーラー";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
