namespace HJCompanion
{
    partial class ObjectInstanceForm
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
            this.saveButton = new System.Windows.Forms.Button();
            this.browseButton = new System.Windows.Forms.Button();
            this.fileText = new System.Windows.Forms.TextBox();
            this.valText = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.previewPicture = new System.Windows.Forms.PictureBox();
            this.propertyListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.okButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.previewPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(349, 412);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(52, 23);
            this.saveButton.TabIndex = 28;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            // 
            // browseButton
            // 
            this.browseButton.Location = new System.Drawing.Point(25, 358);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(75, 23);
            this.browseButton.TabIndex = 27;
            this.browseButton.Text = "Browse";
            this.browseButton.UseVisualStyleBackColor = true;
            // 
            // fileText
            // 
            this.fileText.Location = new System.Drawing.Point(121, 361);
            this.fileText.Name = "fileText";
            this.fileText.Size = new System.Drawing.Size(280, 20);
            this.fileText.TabIndex = 26;
            // 
            // valText
            // 
            this.valText.Location = new System.Drawing.Point(63, 205);
            this.valText.Multiline = true;
            this.valText.Name = "valText";
            this.valText.Size = new System.Drawing.Size(414, 147);
            this.valText.TabIndex = 24;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 205);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Value";
            // 
            // previewPicture
            // 
            this.previewPicture.Location = new System.Drawing.Point(121, 387);
            this.previewPicture.Name = "previewPicture";
            this.previewPicture.Size = new System.Drawing.Size(183, 137);
            this.previewPicture.TabIndex = 29;
            this.previewPicture.TabStop = false;
            // 
            // propertyListView
            // 
            this.propertyListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.propertyListView.HideSelection = false;
            this.propertyListView.Location = new System.Drawing.Point(13, 13);
            this.propertyListView.Name = "propertyListView";
            this.propertyListView.Size = new System.Drawing.Size(488, 186);
            this.propertyListView.TabIndex = 30;
            this.propertyListView.UseCompatibleStateImageBehavior = false;
            this.propertyListView.View = System.Windows.Forms.View.Details;
            this.propertyListView.SelectedIndexChanged += new System.EventHandler(this.propertyListView_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 257;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Type";
            this.columnHeader2.Width = 205;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(4, 543);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(52, 23);
            this.okButton.TabIndex = 31;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // ObjectInstanceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(513, 578);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.propertyListView);
            this.Controls.Add(this.previewPicture);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.fileText);
            this.Controls.Add(this.valText);
            this.Controls.Add(this.label2);
            this.Name = "ObjectInstanceForm";
            this.Text = "ObjectInstanceForm";
            this.Load += new System.EventHandler(this.ObjectInstanceForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.previewPicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox previewPicture;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.TextBox fileText;
        private System.Windows.Forms.TextBox valText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView propertyListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button okButton;
    }
}