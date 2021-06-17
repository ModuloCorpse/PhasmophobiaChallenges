
namespace PhasmophobiaChallenge.Panel.Discord
{
    partial class DiscordPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiscordPanel));
            this.BackButton = new System.Windows.Forms.Button();
            this.SettingsButton = new System.Windows.Forms.Button();
            this.FormFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.AddButton = new System.Windows.Forms.Button();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.EditButton = new System.Windows.Forms.Button();
            this.WaitingAnimation = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.WaitingAnimation)).BeginInit();
            this.SuspendLayout();
            // 
            // BackButton
            // 
            this.BackButton.BackColor = System.Drawing.Color.Transparent;
            this.BackButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BackButton.BackgroundImage")));
            this.BackButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BackButton.FlatAppearance.BorderSize = 0;
            this.BackButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BackButton.Location = new System.Drawing.Point(882, 3);
            this.BackButton.Name = "BackButton";
            this.BackButton.Size = new System.Drawing.Size(75, 58);
            this.BackButton.TabIndex = 3;
            this.BackButton.UseVisualStyleBackColor = false;
            this.BackButton.Click += new System.EventHandler(this.BackButton_Click);
            // 
            // SettingsButton
            // 
            this.SettingsButton.BackColor = System.Drawing.Color.Transparent;
            this.SettingsButton.BackgroundImage = global::PhasmophobiaChallenge.Properties.Resources.tv_remote_icon;
            this.SettingsButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.SettingsButton.FlatAppearance.BorderSize = 0;
            this.SettingsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SettingsButton.Location = new System.Drawing.Point(3, 3);
            this.SettingsButton.Name = "SettingsButton";
            this.SettingsButton.Size = new System.Drawing.Size(50, 50);
            this.SettingsButton.TabIndex = 6;
            this.SettingsButton.UseVisualStyleBackColor = false;
            this.SettingsButton.Click += new System.EventHandler(this.SettingsButton_Click);
            // 
            // FormFlowLayoutPanel
            // 
            this.FormFlowLayoutPanel.AutoScroll = true;
            this.FormFlowLayoutPanel.BackColor = System.Drawing.Color.Transparent;
            this.FormFlowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.FormFlowLayoutPanel.Location = new System.Drawing.Point(23, 73);
            this.FormFlowLayoutPanel.Name = "FormFlowLayoutPanel";
            this.FormFlowLayoutPanel.Size = new System.Drawing.Size(450, 445);
            this.FormFlowLayoutPanel.TabIndex = 7;
            this.FormFlowLayoutPanel.WrapContents = false;
            // 
            // AddButton
            // 
            this.AddButton.Location = new System.Drawing.Point(494, 73);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(125, 35);
            this.AddButton.TabIndex = 8;
            this.AddButton.Text = "Add";
            this.AddButton.UseVisualStyleBackColor = true;
            // 
            // DeleteButton
            // 
            this.DeleteButton.Location = new System.Drawing.Point(494, 114);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(125, 35);
            this.DeleteButton.TabIndex = 9;
            this.DeleteButton.Text = "Delete";
            this.DeleteButton.UseVisualStyleBackColor = true;
            // 
            // EditButton
            // 
            this.EditButton.Location = new System.Drawing.Point(494, 155);
            this.EditButton.Name = "EditButton";
            this.EditButton.Size = new System.Drawing.Size(125, 35);
            this.EditButton.TabIndex = 10;
            this.EditButton.Text = "Edit";
            this.EditButton.UseVisualStyleBackColor = true;
            // 
            // WaitingAnimation
            // 
            this.WaitingAnimation.BackColor = System.Drawing.Color.Transparent;
            this.WaitingAnimation.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.WaitingAnimation.Image = global::PhasmophobiaChallenge.Properties.Resources.waiting_form;
            this.WaitingAnimation.Location = new System.Drawing.Point(638, 155);
            this.WaitingAnimation.Name = "WaitingAnimation";
            this.WaitingAnimation.Size = new System.Drawing.Size(300, 300);
            this.WaitingAnimation.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.WaitingAnimation.TabIndex = 11;
            this.WaitingAnimation.TabStop = false;
            this.WaitingAnimation.Visible = false;
            // 
            // DiscordPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::PhasmophobiaChallenge.Properties.Resources.edgefield_street_house;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.WaitingAnimation);
            this.Controls.Add(this.EditButton);
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.AddButton);
            this.Controls.Add(this.FormFlowLayoutPanel);
            this.Controls.Add(this.SettingsButton);
            this.Controls.Add(this.BackButton);
            this.DoubleBuffered = true;
            this.Name = "DiscordPanel";
            this.Size = new System.Drawing.Size(960, 540);
            ((System.ComponentModel.ISupportInitialize)(this.WaitingAnimation)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button BackButton;
        private System.Windows.Forms.Button SettingsButton;
        private System.Windows.Forms.FlowLayoutPanel FormFlowLayoutPanel;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.Button EditButton;
        private System.Windows.Forms.PictureBox WaitingAnimation;
    }
}
