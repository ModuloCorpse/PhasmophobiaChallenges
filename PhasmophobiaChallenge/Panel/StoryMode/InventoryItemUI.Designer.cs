
namespace PhasmophobiaChallenge.Panel.StoryMode
{
    partial class InventoryItemUI
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.ItemInventory = new System.Windows.Forms.Label();
            this.BuyItem = new System.Windows.Forms.Button();
            this.SellItem = new System.Windows.Forms.Button();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.BuyItem);
            this.flowLayoutPanel1.Controls.Add(this.SellItem);
            this.flowLayoutPanel1.Controls.Add(this.ItemInventory);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(260, 25);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // ItemInventory
            // 
            this.ItemInventory.Font = new System.Drawing.Font("Yahfie", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ItemInventory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(108)))), ((int)(((byte)(124)))));
            this.ItemInventory.Location = new System.Drawing.Point(55, 0);
            this.ItemInventory.Name = "ItemInventory";
            this.ItemInventory.Size = new System.Drawing.Size(200, 23);
            this.ItemInventory.TabIndex = 20;
            this.ItemInventory.Text = "label1";
            this.ItemInventory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BuyItem
            // 
            this.BuyItem.BackColor = System.Drawing.Color.Transparent;
            this.BuyItem.BackgroundImage = global::PhasmophobiaChallenge.Properties.Resources.right_ui_arrow_border;
            this.BuyItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BuyItem.FlatAppearance.BorderSize = 0;
            this.BuyItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BuyItem.Image = global::PhasmophobiaChallenge.Properties.Resources.add_ui_small;
            this.BuyItem.Location = new System.Drawing.Point(3, 3);
            this.BuyItem.Name = "BuyItem";
            this.BuyItem.Size = new System.Drawing.Size(20, 20);
            this.BuyItem.TabIndex = 18;
            this.BuyItem.UseVisualStyleBackColor = false;
            this.BuyItem.Click += new System.EventHandler(this.BuyItem_Click);
            // 
            // SellItem
            // 
            this.SellItem.BackColor = System.Drawing.Color.Transparent;
            this.SellItem.BackgroundImage = global::PhasmophobiaChallenge.Properties.Resources.right_ui_arrow_border;
            this.SellItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.SellItem.FlatAppearance.BorderSize = 0;
            this.SellItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SellItem.Image = global::PhasmophobiaChallenge.Properties.Resources.remove_ui_small;
            this.SellItem.Location = new System.Drawing.Point(29, 3);
            this.SellItem.Name = "SellItem";
            this.SellItem.Size = new System.Drawing.Size(20, 20);
            this.SellItem.TabIndex = 19;
            this.SellItem.UseVisualStyleBackColor = false;
            this.SellItem.Click += new System.EventHandler(this.SellItem_Click);
            // 
            // InventoryItemUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "InventoryItemUI";
            this.Size = new System.Drawing.Size(260, 25);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button BuyItem;
        private System.Windows.Forms.Button SellItem;
        private System.Windows.Forms.Label ItemInventory;
    }
}
