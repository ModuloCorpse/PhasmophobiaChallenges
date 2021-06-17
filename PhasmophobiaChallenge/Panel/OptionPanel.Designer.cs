
namespace PhasmophobiaChallenge.Panel
{
    partial class OptionPanel
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
            this.PreviousLocal = new System.Windows.Forms.Button();
            this.NextLocal = new System.Windows.Forms.Button();
            this.Local = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.BackButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // PreviousLocal
            // 
            this.PreviousLocal.BackColor = System.Drawing.Color.Transparent;
            this.PreviousLocal.BackgroundImage = global::PhasmophobiaChallenge.Properties.Resources.left_ui_arrow_border;
            this.PreviousLocal.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.PreviousLocal.FlatAppearance.BorderSize = 0;
            this.PreviousLocal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PreviousLocal.Image = global::PhasmophobiaChallenge.Properties.Resources.left_ui_arrow;
            this.PreviousLocal.Location = new System.Drawing.Point(279, 74);
            this.PreviousLocal.Name = "PreviousLocal";
            this.PreviousLocal.Size = new System.Drawing.Size(60, 60);
            this.PreviousLocal.TabIndex = 6;
            this.PreviousLocal.UseVisualStyleBackColor = false;
            this.PreviousLocal.Click += new System.EventHandler(this.PreviousLocal_Click);
            // 
            // NextLocal
            // 
            this.NextLocal.BackColor = System.Drawing.Color.Transparent;
            this.NextLocal.BackgroundImage = global::PhasmophobiaChallenge.Properties.Resources.right_ui_arrow_border;
            this.NextLocal.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.NextLocal.FlatAppearance.BorderSize = 0;
            this.NextLocal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.NextLocal.Image = global::PhasmophobiaChallenge.Properties.Resources.right_ui_arrow;
            this.NextLocal.Location = new System.Drawing.Point(613, 74);
            this.NextLocal.Name = "NextLocal";
            this.NextLocal.Size = new System.Drawing.Size(60, 60);
            this.NextLocal.TabIndex = 7;
            this.NextLocal.UseVisualStyleBackColor = false;
            this.NextLocal.Click += new System.EventHandler(this.NextLocal_Click);
            // 
            // Local
            // 
            this.Local.BackColor = System.Drawing.Color.Transparent;
            this.Local.Font = new System.Drawing.Font("Yahfie", 36F, System.Drawing.FontStyle.Bold);
            this.Local.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(108)))), ((int)(((byte)(124)))));
            this.Local.Image = global::PhasmophobiaChallenge.Properties.Resources.main_menu_panel_button_background;
            this.Local.Location = new System.Drawing.Point(355, 83);
            this.Local.Name = "Local";
            this.Local.Size = new System.Drawing.Size(242, 40);
            this.Local.TabIndex = 8;
            this.Local.Text = "label1";
            this.Local.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = global::PhasmophobiaChallenge.Properties.Resources.main_menu_panel_button_background;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(345, 74);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(262, 60);
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // BackButton
            // 
            this.BackButton.BackColor = System.Drawing.Color.Transparent;
            this.BackButton.BackgroundImage = global::PhasmophobiaChallenge.Properties.Resources.main_menu_panel_button_background;
            this.BackButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BackButton.FlatAppearance.BorderSize = 0;
            this.BackButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BackButton.Font = new System.Drawing.Font("Yahfie", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BackButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(108)))), ((int)(((byte)(124)))));
            this.BackButton.Location = new System.Drawing.Point(345, 140);
            this.BackButton.Name = "BackButton";
            this.BackButton.Size = new System.Drawing.Size(262, 60);
            this.BackButton.TabIndex = 10;
            this.BackButton.Text = "Options";
            this.BackButton.UseVisualStyleBackColor = false;
            this.BackButton.Click += new System.EventHandler(this.BackButton_Click);
            // 
            // OptionPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::PhasmophobiaChallenge.Properties.Resources.phasmophobia_whiteboard;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.BackButton);
            this.Controls.Add(this.Local);
            this.Controls.Add(this.NextLocal);
            this.Controls.Add(this.PreviousLocal);
            this.Controls.Add(this.pictureBox1);
            this.DoubleBuffered = true;
            this.Name = "OptionPanel";
            this.Size = new System.Drawing.Size(960, 540);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button PreviousLocal;
        private System.Windows.Forms.Button NextLocal;
        private System.Windows.Forms.Label Local;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button BackButton;
    }
}
