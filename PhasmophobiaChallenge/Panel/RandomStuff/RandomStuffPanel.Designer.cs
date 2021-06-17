
namespace PhasmophobiaChallenge.Panel.RandomStuff
{
    partial class RandomStuffPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RandomStuffPanel));
            this.RandomizeButton = new System.Windows.Forms.Button();
            this.ResetButton = new System.Windows.Forms.Button();
            this.BackButton = new System.Windows.Forms.Button();
            this.ItemName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // RandomizeButton
            // 
            this.RandomizeButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.RandomizeButton.BackColor = System.Drawing.Color.Transparent;
            this.RandomizeButton.FlatAppearance.BorderSize = 0;
            this.RandomizeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RandomizeButton.Font = new System.Drawing.Font(GetFontFamily(), 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RandomizeButton.ForeColor = System.Drawing.Color.White;
            this.RandomizeButton.Location = new System.Drawing.Point(3, 478);
            this.RandomizeButton.Name = "RandomizeButton";
            this.RandomizeButton.Size = new System.Drawing.Size(400, 56);
            this.RandomizeButton.TabIndex = 1;
            this.RandomizeButton.Text = "Randomize";
            this.RandomizeButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.RandomizeButton.UseVisualStyleBackColor = false;
            this.RandomizeButton.Click += new System.EventHandler(this.RandomizeButton_Click);
            // 
            // ResetButton
            // 
            this.ResetButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ResetButton.BackColor = System.Drawing.Color.Transparent;
            this.ResetButton.FlatAppearance.BorderSize = 0;
            this.ResetButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ResetButton.Font = new System.Drawing.Font(GetFontFamily(), 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ResetButton.ForeColor = System.Drawing.Color.White;
            this.ResetButton.Location = new System.Drawing.Point(557, 478);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(400, 56);
            this.ResetButton.TabIndex = 2;
            this.ResetButton.Text = "Reset";
            this.ResetButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ResetButton.UseVisualStyleBackColor = false;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
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
            // ItemName
            // 
            this.ItemName.BackColor = System.Drawing.Color.Transparent;
            this.ItemName.Font = new System.Drawing.Font("October Crow", 42F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ItemName.ForeColor = System.Drawing.Color.White;
            this.ItemName.Location = new System.Drawing.Point(0, 191);
            this.ItemName.Name = "ItemName";
            this.ItemName.Size = new System.Drawing.Size(960, 84);
            this.ItemName.TabIndex = 4;
            this.ItemName.Text = "label1";
            this.ItemName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // RandomStuffPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::PhasmophobiaChallenge.Properties.Resources.asylum;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.ItemName);
            this.Controls.Add(this.BackButton);
            this.Controls.Add(this.ResetButton);
            this.Controls.Add(this.RandomizeButton);
            this.DoubleBuffered = true;
            this.Name = "RandomStuffPanel";
            this.Size = new System.Drawing.Size(960, 540);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button RandomizeButton;
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.Button BackButton;
        private System.Windows.Forms.Label ItemName;
    }
}
