namespace HJCompanion
{
    partial class ControlsWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControlsWindow));
            this.cursorButton = new System.Windows.Forms.Button();
            this.objListView = new System.Windows.Forms.ListBox();
            this.addObjButton = new System.Windows.Forms.Button();
            this.editButton = new System.Windows.Forms.Button();
            this.deleteObjButton = new System.Windows.Forms.Button();
            this.placeButton = new System.Windows.Forms.Button();
            this.removeButton = new System.Windows.Forms.Button();
            this.moveButton = new System.Windows.Forms.Button();
            this.detailsButton = new System.Windows.Forms.Button();
            this.saveInstanceButton = new System.Windows.Forms.Button();
            this.delInstanceButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cursorButton
            // 
            this.cursorButton.Image = ((System.Drawing.Image)(resources.GetObject("cursorButton.Image")));
            this.cursorButton.Location = new System.Drawing.Point(12, 32);
            this.cursorButton.Name = "cursorButton";
            this.cursorButton.Size = new System.Drawing.Size(34, 37);
            this.cursorButton.TabIndex = 0;
            this.cursorButton.UseVisualStyleBackColor = true;
            this.cursorButton.Click += new System.EventHandler(this.cursorButton_Click);
            // 
            // objListView
            // 
            this.objListView.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.objListView.FormattingEnabled = true;
            this.objListView.Location = new System.Drawing.Point(0, 260);
            this.objListView.Name = "objListView";
            this.objListView.Size = new System.Drawing.Size(262, 303);
            this.objListView.TabIndex = 1;
            this.objListView.SelectedIndexChanged += new System.EventHandler(this.objListView_SelectedIndexChanged);
            // 
            // addObjButton
            // 
            this.addObjButton.Location = new System.Drawing.Point(132, 217);
            this.addObjButton.Name = "addObjButton";
            this.addObjButton.Size = new System.Drawing.Size(34, 37);
            this.addObjButton.TabIndex = 2;
            this.addObjButton.Text = "Add";
            this.addObjButton.UseVisualStyleBackColor = true;
            this.addObjButton.Click += new System.EventHandler(this.addObjButton_Click);
            // 
            // editButton
            // 
            this.editButton.Location = new System.Drawing.Point(172, 217);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(34, 37);
            this.editButton.TabIndex = 3;
            this.editButton.Text = "Edit";
            this.editButton.UseVisualStyleBackColor = true;
            this.editButton.Click += new System.EventHandler(this.editButton_Click);
            // 
            // deleteObjButton
            // 
            this.deleteObjButton.Location = new System.Drawing.Point(212, 217);
            this.deleteObjButton.Name = "deleteObjButton";
            this.deleteObjButton.Size = new System.Drawing.Size(34, 37);
            this.deleteObjButton.TabIndex = 4;
            this.deleteObjButton.Text = "Del";
            this.deleteObjButton.UseVisualStyleBackColor = true;
            this.deleteObjButton.Click += new System.EventHandler(this.deleteObjButton_Click);
            // 
            // placeButton
            // 
            this.placeButton.Image = ((System.Drawing.Image)(resources.GetObject("placeButton.Image")));
            this.placeButton.Location = new System.Drawing.Point(12, 75);
            this.placeButton.Name = "placeButton";
            this.placeButton.Size = new System.Drawing.Size(34, 37);
            this.placeButton.TabIndex = 5;
            this.placeButton.UseVisualStyleBackColor = true;
            this.placeButton.Click += new System.EventHandler(this.placeButton_Click);
            // 
            // removeButton
            // 
            this.removeButton.Image = ((System.Drawing.Image)(resources.GetObject("removeButton.Image")));
            this.removeButton.Location = new System.Drawing.Point(52, 75);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(34, 37);
            this.removeButton.TabIndex = 6;
            this.removeButton.UseVisualStyleBackColor = true;
            this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
            // 
            // moveButton
            // 
            this.moveButton.Image = ((System.Drawing.Image)(resources.GetObject("moveButton.Image")));
            this.moveButton.Location = new System.Drawing.Point(92, 75);
            this.moveButton.Name = "moveButton";
            this.moveButton.Size = new System.Drawing.Size(34, 37);
            this.moveButton.TabIndex = 7;
            this.moveButton.UseVisualStyleBackColor = true;
            this.moveButton.Click += new System.EventHandler(this.moveButton_Click);
            // 
            // detailsButton
            // 
            this.detailsButton.Image = ((System.Drawing.Image)(resources.GetObject("detailsButton.Image")));
            this.detailsButton.Location = new System.Drawing.Point(132, 75);
            this.detailsButton.Name = "detailsButton";
            this.detailsButton.Size = new System.Drawing.Size(34, 37);
            this.detailsButton.TabIndex = 8;
            this.detailsButton.UseVisualStyleBackColor = true;
            this.detailsButton.Click += new System.EventHandler(this.detailsButton_Click);
            // 
            // saveInstanceButton
            // 
            this.saveInstanceButton.Image = ((System.Drawing.Image)(resources.GetObject("saveInstanceButton.Image")));
            this.saveInstanceButton.Location = new System.Drawing.Point(12, 118);
            this.saveInstanceButton.Name = "saveInstanceButton";
            this.saveInstanceButton.Size = new System.Drawing.Size(34, 37);
            this.saveInstanceButton.TabIndex = 9;
            this.saveInstanceButton.UseVisualStyleBackColor = true;
            this.saveInstanceButton.Click += new System.EventHandler(this.saveInstanceButton_Click);
            // 
            // delInstanceButton
            // 
            this.delInstanceButton.Location = new System.Drawing.Point(12, 161);
            this.delInstanceButton.Name = "delInstanceButton";
            this.delInstanceButton.Size = new System.Drawing.Size(154, 21);
            this.delInstanceButton.TabIndex = 10;
            this.delInstanceButton.Text = "Remove All Instances";
            this.delInstanceButton.UseVisualStyleBackColor = true;
            this.delInstanceButton.Click += new System.EventHandler(this.delInstanceButton_Click);
            // 
            // ControlsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(262, 563);
            this.Controls.Add(this.delInstanceButton);
            this.Controls.Add(this.saveInstanceButton);
            this.Controls.Add(this.detailsButton);
            this.Controls.Add(this.moveButton);
            this.Controls.Add(this.removeButton);
            this.Controls.Add(this.placeButton);
            this.Controls.Add(this.deleteObjButton);
            this.Controls.Add(this.editButton);
            this.Controls.Add(this.addObjButton);
            this.Controls.Add(this.objListView);
            this.Controls.Add(this.cursorButton);
            this.Name = "ControlsWindow";
            this.Text = "ControlsWindow";
            this.Load += new System.EventHandler(this.ControlsWindow_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cursorButton;
        private System.Windows.Forms.ListBox objListView;
        private System.Windows.Forms.Button addObjButton;
        private System.Windows.Forms.Button editButton;
        private System.Windows.Forms.Button deleteObjButton;
        private System.Windows.Forms.Button placeButton;
        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.Button moveButton;
        private System.Windows.Forms.Button detailsButton;
        private System.Windows.Forms.Button saveInstanceButton;
        private System.Windows.Forms.Button delInstanceButton;
    }
}