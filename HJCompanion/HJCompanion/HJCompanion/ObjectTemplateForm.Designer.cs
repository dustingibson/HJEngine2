namespace HJCompanion
{
    partial class ObjectTemplateForm
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
            this.propListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.valueText = new System.Windows.Forms.TabPage();
            this.previewPicture = new System.Windows.Forms.PictureBox();
            this.addButton = new System.Windows.Forms.Button();
            this.browseButton = new System.Windows.Forms.Button();
            this.fileText = new System.Windows.Forms.TextBox();
            this.typeCombo = new System.Windows.Forms.ComboBox();
            this.valText = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.nameText = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.previewImagePicture = new System.Windows.Forms.PictureBox();
            this.imageListView = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label4 = new System.Windows.Forms.Label();
            this.imageNameText = new System.Windows.Forms.TextBox();
            this.imageAddButton = new System.Windows.Forms.Button();
            this.imageBrowseButton = new System.Windows.Forms.Button();
            this.imageFileText = new System.Windows.Forms.TextBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.objNameText = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.propOpenDialog = new System.Windows.Forms.OpenFileDialog();
            this.imgFileOpenDlg = new System.Windows.Forms.OpenFileDialog();
            this.tabControl1.SuspendLayout();
            this.valueText.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.previewPicture)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.previewImagePicture)).BeginInit();
            this.SuspendLayout();
            // 
            // propListView
            // 
            this.propListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.propListView.Dock = System.Windows.Forms.DockStyle.Top;
            this.propListView.FullRowSelect = true;
            this.propListView.GridLines = true;
            this.propListView.HideSelection = false;
            this.propListView.Location = new System.Drawing.Point(3, 3);
            this.propListView.Name = "propListView";
            this.propListView.Size = new System.Drawing.Size(496, 178);
            this.propListView.TabIndex = 0;
            this.propListView.UseCompatibleStateImageBehavior = false;
            this.propListView.View = System.Windows.Forms.View.Details;
            this.propListView.SelectedIndexChanged += new System.EventHandler(this.propListView_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 221;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Type";
            this.columnHeader2.Width = 235;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.valueText);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(510, 550);
            this.tabControl1.TabIndex = 1;
            // 
            // valueText
            // 
            this.valueText.Controls.Add(this.previewPicture);
            this.valueText.Controls.Add(this.addButton);
            this.valueText.Controls.Add(this.browseButton);
            this.valueText.Controls.Add(this.fileText);
            this.valueText.Controls.Add(this.typeCombo);
            this.valueText.Controls.Add(this.valText);
            this.valueText.Controls.Add(this.label3);
            this.valueText.Controls.Add(this.label2);
            this.valueText.Controls.Add(this.label1);
            this.valueText.Controls.Add(this.nameText);
            this.valueText.Controls.Add(this.propListView);
            this.valueText.Location = new System.Drawing.Point(4, 22);
            this.valueText.Name = "valueText";
            this.valueText.Padding = new System.Windows.Forms.Padding(3);
            this.valueText.Size = new System.Drawing.Size(502, 524);
            this.valueText.TabIndex = 0;
            this.valueText.Text = "Properties";
            this.valueText.UseVisualStyleBackColor = true;
            this.valueText.Click += new System.EventHandler(this.valueText_Click);
            // 
            // previewPicture
            // 
            this.previewPicture.Location = new System.Drawing.Point(174, 392);
            this.previewPicture.Name = "previewPicture";
            this.previewPicture.Size = new System.Drawing.Size(130, 116);
            this.previewPicture.TabIndex = 17;
            this.previewPicture.TabStop = false;
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(25, 366);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(52, 23);
            this.addButton.TabIndex = 9;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // browseButton
            // 
            this.browseButton.Location = new System.Drawing.Point(83, 366);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(75, 23);
            this.browseButton.TabIndex = 8;
            this.browseButton.Text = "Browse";
            this.browseButton.UseVisualStyleBackColor = true;
            // 
            // fileText
            // 
            this.fileText.Location = new System.Drawing.Point(164, 366);
            this.fileText.Name = "fileText";
            this.fileText.Size = new System.Drawing.Size(280, 20);
            this.fileText.TabIndex = 7;
            // 
            // typeCombo
            // 
            this.typeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.typeCombo.FormattingEnabled = true;
            this.typeCombo.Items.AddRange(new object[] {
            "string",
            "int",
            "float",
            "double",
            "bool",
            "image",
            "audio"});
            this.typeCombo.Location = new System.Drawing.Point(65, 227);
            this.typeCombo.Name = "typeCombo";
            this.typeCombo.Size = new System.Drawing.Size(380, 21);
            this.typeCombo.TabIndex = 6;
            // 
            // valText
            // 
            this.valText.Location = new System.Drawing.Point(64, 254);
            this.valText.Multiline = true;
            this.valText.Name = "valText";
            this.valText.Size = new System.Drawing.Size(380, 92);
            this.valText.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 230);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Type";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 254);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Value";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 204);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Name";
            // 
            // nameText
            // 
            this.nameText.Location = new System.Drawing.Point(64, 201);
            this.nameText.Name = "nameText";
            this.nameText.Size = new System.Drawing.Size(381, 20);
            this.nameText.TabIndex = 1;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.previewImagePicture);
            this.tabPage2.Controls.Add(this.imageListView);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.imageNameText);
            this.tabPage2.Controls.Add(this.imageAddButton);
            this.tabPage2.Controls.Add(this.imageBrowseButton);
            this.tabPage2.Controls.Add(this.imageFileText);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(502, 524);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Images";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // previewImagePicture
            // 
            this.previewImagePicture.Location = new System.Drawing.Point(130, 200);
            this.previewImagePicture.Name = "previewImagePicture";
            this.previewImagePicture.Size = new System.Drawing.Size(220, 196);
            this.previewImagePicture.TabIndex = 16;
            this.previewImagePicture.TabStop = false;
            // 
            // imageListView
            // 
            this.imageListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3});
            this.imageListView.Dock = System.Windows.Forms.DockStyle.Top;
            this.imageListView.HideSelection = false;
            this.imageListView.Location = new System.Drawing.Point(3, 3);
            this.imageListView.Name = "imageListView";
            this.imageListView.Size = new System.Drawing.Size(496, 178);
            this.imageListView.TabIndex = 15;
            this.imageListView.UseCompatibleStateImageBehavior = false;
            this.imageListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Name";
            this.columnHeader3.Width = 425;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(36, 439);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Name";
            // 
            // imageNameText
            // 
            this.imageNameText.Location = new System.Drawing.Point(77, 436);
            this.imageNameText.Name = "imageNameText";
            this.imageNameText.Size = new System.Drawing.Size(381, 20);
            this.imageNameText.TabIndex = 13;
            // 
            // imageAddButton
            // 
            this.imageAddButton.Location = new System.Drawing.Point(39, 474);
            this.imageAddButton.Name = "imageAddButton";
            this.imageAddButton.Size = new System.Drawing.Size(52, 23);
            this.imageAddButton.TabIndex = 12;
            this.imageAddButton.Text = "Add";
            this.imageAddButton.UseVisualStyleBackColor = true;
            this.imageAddButton.Click += new System.EventHandler(this.imageAddButton_Click);
            // 
            // imageBrowseButton
            // 
            this.imageBrowseButton.Location = new System.Drawing.Point(97, 474);
            this.imageBrowseButton.Name = "imageBrowseButton";
            this.imageBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.imageBrowseButton.TabIndex = 11;
            this.imageBrowseButton.Text = "Browse";
            this.imageBrowseButton.UseVisualStyleBackColor = true;
            this.imageBrowseButton.Click += new System.EventHandler(this.imageBrowseButton_Click);
            // 
            // imageFileText
            // 
            this.imageFileText.Location = new System.Drawing.Point(178, 474);
            this.imageFileText.Name = "imageFileText";
            this.imageFileText.Size = new System.Drawing.Size(280, 20);
            this.imageFileText.TabIndex = 10;
            this.imageFileText.TextChanged += new System.EventHandler(this.imageFileText_TextChanged);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(7, 556);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 2;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(96, 556);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // objNameText
            // 
            this.objNameText.Location = new System.Drawing.Point(226, 559);
            this.objNameText.Name = "objNameText";
            this.objNameText.Size = new System.Drawing.Size(280, 20);
            this.objNameText.TabIndex = 18;
            this.objNameText.TextChanged += new System.EventHandler(this.objNameText_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(185, 562);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "Name";
            // 
            // propOpenDialog
            // 
            this.propOpenDialog.FileName = "openFileDialog1";
            // 
            // imgFileOpenDlg
            // 
            this.imgFileOpenDlg.FileName = "openFileDialog2";
            // 
            // ObjectTemplateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 591);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.objNameText);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.tabControl1);
            this.Name = "ObjectTemplateForm";
            this.Text = "ObjectTemplateForm";
            this.Load += new System.EventHandler(this.ObjectTemplateForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.valueText.ResumeLayout(false);
            this.valueText.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.previewPicture)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.previewImagePicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView propListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage valueText;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ComboBox typeCombo;
        private System.Windows.Forms.TextBox valText;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox nameText;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.TextBox fileText;
        private System.Windows.Forms.PictureBox previewImagePicture;
        private System.Windows.Forms.ListView imageListView;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox imageNameText;
        private System.Windows.Forms.Button imageAddButton;
        private System.Windows.Forms.Button imageBrowseButton;
        private System.Windows.Forms.TextBox imageFileText;
        private System.Windows.Forms.PictureBox previewPicture;
        private System.Windows.Forms.TextBox objNameText;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.OpenFileDialog propOpenDialog;
        private System.Windows.Forms.OpenFileDialog imgFileOpenDlg;
    }
}