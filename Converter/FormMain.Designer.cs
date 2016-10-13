namespace Converter
{
	partial class FormMain
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
			this.buttonBrowseSource = new System.Windows.Forms.Button();
			this.textSource = new System.Windows.Forms.TextBox();
			this.listFiles = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.textDestination = new System.Windows.Forms.TextBox();
			this.buttonBrowseDestination = new System.Windows.Forms.Button();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.pictureBox = new System.Windows.Forms.PictureBox();
			this.buttonConvert = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// buttonBrowseSource
			// 
			this.buttonBrowseSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonBrowseSource.Location = new System.Drawing.Point(593, 17);
			this.buttonBrowseSource.Name = "buttonBrowseSource";
			this.buttonBrowseSource.Size = new System.Drawing.Size(75, 23);
			this.buttonBrowseSource.TabIndex = 0;
			this.buttonBrowseSource.Text = "Browse...";
			this.buttonBrowseSource.UseVisualStyleBackColor = true;
			this.buttonBrowseSource.Click += new System.EventHandler(this.buttonBrowseSource_Click);
			// 
			// textSource
			// 
			this.textSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textSource.Location = new System.Drawing.Point(75, 19);
			this.textSource.Name = "textSource";
			this.textSource.Size = new System.Drawing.Size(512, 20);
			this.textSource.TabIndex = 1;
			this.textSource.Text = "E:\\Development\\tiki1\\GameMainContent\\Islands";
			// 
			// listFiles
			// 
			this.listFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listFiles.FormattingEnabled = true;
			this.listFiles.IntegralHeight = false;
			this.listFiles.Location = new System.Drawing.Point(0, 0);
			this.listFiles.Name = "listFiles";
			this.listFiles.Size = new System.Drawing.Size(224, 351);
			this.listFiles.TabIndex = 2;
			this.listFiles.SelectedIndexChanged += new System.EventHandler(this.listFiles_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 22);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(44, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Source:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(63, 13);
			this.label2.TabIndex = 6;
			this.label2.Text = "Destination:";
			// 
			// textDestination
			// 
			this.textDestination.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textDestination.Location = new System.Drawing.Point(75, 45);
			this.textDestination.Name = "textDestination";
			this.textDestination.Size = new System.Drawing.Size(512, 20);
			this.textDestination.TabIndex = 5;
			this.textDestination.Text = "E:\\Development\\tiki3\\content\\genericdata\\entities";
			// 
			// buttonBrowseDestination
			// 
			this.buttonBrowseDestination.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonBrowseDestination.Location = new System.Drawing.Point(593, 43);
			this.buttonBrowseDestination.Name = "buttonBrowseDestination";
			this.buttonBrowseDestination.Size = new System.Drawing.Size(75, 23);
			this.buttonBrowseDestination.TabIndex = 4;
			this.buttonBrowseDestination.Text = "Browse...";
			this.buttonBrowseDestination.UseVisualStyleBackColor = true;
			this.buttonBrowseDestination.Click += new System.EventHandler(this.buttonBrowseDestination_Click);
			// 
			// openFileDialog
			// 
			this.openFileDialog.RestoreDirectory = true;
			this.openFileDialog.SupportMultiDottedExtensions = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.textSource);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.buttonBrowseSource);
			this.groupBox1.Controls.Add(this.textDestination);
			this.groupBox1.Controls.Add(this.buttonBrowseDestination);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(674, 74);
			this.groupBox1.TabIndex = 7;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Directories";
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.Location = new System.Drawing.Point(12, 92);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.buttonConvert);
			this.splitContainer1.Panel1.Controls.Add(this.listFiles);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.pictureBox);
			this.splitContainer1.Size = new System.Drawing.Size(674, 380);
			this.splitContainer1.SplitterDistance = 224;
			this.splitContainer1.TabIndex = 8;
			// 
			// pictureBox
			// 
			this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pictureBox.Location = new System.Drawing.Point(0, 0);
			this.pictureBox.Name = "pictureBox";
			this.pictureBox.Size = new System.Drawing.Size(446, 380);
			this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox.TabIndex = 0;
			this.pictureBox.TabStop = false;
			// 
			// buttonConvert
			// 
			this.buttonConvert.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.buttonConvert.Location = new System.Drawing.Point(0, 357);
			this.buttonConvert.Name = "buttonConvert";
			this.buttonConvert.Size = new System.Drawing.Size(224, 23);
			this.buttonConvert.TabIndex = 3;
			this.buttonConvert.Text = "Convert all";
			this.buttonConvert.UseVisualStyleBackColor = true;
			this.buttonConvert.Click += new System.EventHandler(this.buttonConvert_Click);
			// 
			// FormMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(698, 484);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.groupBox1);
			this.Name = "FormMain";
			this.Text = "Converter";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button buttonBrowseSource;
		private System.Windows.Forms.TextBox textSource;
		private System.Windows.Forms.ListBox listFiles;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textDestination;
		private System.Windows.Forms.Button buttonBrowseDestination;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.PictureBox pictureBox;
		private System.Windows.Forms.Button buttonConvert;
	}
}

