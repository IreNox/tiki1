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
			this.buttonIslandBrowseSource = new System.Windows.Forms.Button();
			this.textIslandSource = new System.Windows.Forms.TextBox();
			this.listIslands = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.textIslandDestination = new System.Windows.Forms.TextBox();
			this.buttonIslandBrowseDestination = new System.Windows.Forms.Button();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.buttonIslandConvert = new System.Windows.Forms.Button();
			this.pictureIsland = new System.Windows.Forms.PictureBox();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabBreakables = new System.Windows.Forms.TabPage();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.buttonBreakableConvert = new System.Windows.Forms.Button();
			this.listBreakables = new System.Windows.Forms.ListBox();
			this.pictureBreakable = new System.Windows.Forms.PictureBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.textBreakableSource = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.buttonBreakableBrowseSource = new System.Windows.Forms.Button();
			this.textBreakableDestination = new System.Windows.Forms.TextBox();
			this.buttonBreakableBrowseDestination = new System.Windows.Forms.Button();
			this.tabIslands = new System.Windows.Forms.TabPage();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureIsland)).BeginInit();
			this.tabControl.SuspendLayout();
			this.tabBreakables.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBreakable)).BeginInit();
			this.groupBox2.SuspendLayout();
			this.tabIslands.SuspendLayout();
			this.SuspendLayout();
			// 
			// buttonIslandBrowseSource
			// 
			this.buttonIslandBrowseSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonIslandBrowseSource.Location = new System.Drawing.Point(961, 17);
			this.buttonIslandBrowseSource.Name = "buttonIslandBrowseSource";
			this.buttonIslandBrowseSource.Size = new System.Drawing.Size(75, 23);
			this.buttonIslandBrowseSource.TabIndex = 0;
			this.buttonIslandBrowseSource.Text = "Browse...";
			this.buttonIslandBrowseSource.UseVisualStyleBackColor = true;
			this.buttonIslandBrowseSource.Click += new System.EventHandler(this.buttonIslandBrowseSource_Click);
			// 
			// textIslandSource
			// 
			this.textIslandSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textIslandSource.Location = new System.Drawing.Point(75, 19);
			this.textIslandSource.Name = "textIslandSource";
			this.textIslandSource.Size = new System.Drawing.Size(880, 20);
			this.textIslandSource.TabIndex = 1;
			this.textIslandSource.Text = "E:\\Development\\tiki1\\GameMainContent\\Islands";
			// 
			// listIslands
			// 
			this.listIslands.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listIslands.FormattingEnabled = true;
			this.listIslands.IntegralHeight = false;
			this.listIslands.Location = new System.Drawing.Point(0, 0);
			this.listIslands.Name = "listIslands";
			this.listIslands.Size = new System.Drawing.Size(346, 506);
			this.listIslands.TabIndex = 2;
			this.listIslands.SelectedIndexChanged += new System.EventHandler(this.listIslands_SelectedIndexChanged);
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
			// textIslandDestination
			// 
			this.textIslandDestination.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textIslandDestination.Location = new System.Drawing.Point(75, 45);
			this.textIslandDestination.Name = "textIslandDestination";
			this.textIslandDestination.Size = new System.Drawing.Size(880, 20);
			this.textIslandDestination.TabIndex = 5;
			this.textIslandDestination.Text = "E:\\Development\\mechanica\\content\\mechanica";
			// 
			// buttonIslandBrowseDestination
			// 
			this.buttonIslandBrowseDestination.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonIslandBrowseDestination.Location = new System.Drawing.Point(961, 43);
			this.buttonIslandBrowseDestination.Name = "buttonIslandBrowseDestination";
			this.buttonIslandBrowseDestination.Size = new System.Drawing.Size(75, 23);
			this.buttonIslandBrowseDestination.TabIndex = 4;
			this.buttonIslandBrowseDestination.Text = "Browse...";
			this.buttonIslandBrowseDestination.UseVisualStyleBackColor = true;
			this.buttonIslandBrowseDestination.Click += new System.EventHandler(this.buttonIslandBrowseDestination_Click);
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
			this.groupBox1.Controls.Add(this.textIslandSource);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.buttonIslandBrowseSource);
			this.groupBox1.Controls.Add(this.textIslandDestination);
			this.groupBox1.Controls.Add(this.buttonIslandBrowseDestination);
			this.groupBox1.Location = new System.Drawing.Point(6, 6);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(1042, 74);
			this.groupBox1.TabIndex = 7;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Directories";
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.Location = new System.Drawing.Point(6, 86);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.buttonIslandConvert);
			this.splitContainer1.Panel1.Controls.Add(this.listIslands);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.pictureIsland);
			this.splitContainer1.Size = new System.Drawing.Size(1042, 535);
			this.splitContainer1.SplitterDistance = 346;
			this.splitContainer1.TabIndex = 8;
			// 
			// buttonIslandConvert
			// 
			this.buttonIslandConvert.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.buttonIslandConvert.Location = new System.Drawing.Point(0, 512);
			this.buttonIslandConvert.Name = "buttonIslandConvert";
			this.buttonIslandConvert.Size = new System.Drawing.Size(346, 23);
			this.buttonIslandConvert.TabIndex = 3;
			this.buttonIslandConvert.Text = "Convert all";
			this.buttonIslandConvert.UseVisualStyleBackColor = true;
			this.buttonIslandConvert.Click += new System.EventHandler(this.buttonIslandConvert_Click);
			// 
			// pictureIsland
			// 
			this.pictureIsland.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pictureIsland.Location = new System.Drawing.Point(0, 0);
			this.pictureIsland.Name = "pictureIsland";
			this.pictureIsland.Size = new System.Drawing.Size(692, 535);
			this.pictureIsland.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureIsland.TabIndex = 0;
			this.pictureIsland.TabStop = false;
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add(this.tabBreakables);
			this.tabControl.Controls.Add(this.tabIslands);
			this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl.Location = new System.Drawing.Point(4, 4);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(1062, 653);
			this.tabControl.TabIndex = 9;
			this.tabControl.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl_Selected);
			// 
			// tabBreakables
			// 
			this.tabBreakables.Controls.Add(this.splitContainer2);
			this.tabBreakables.Controls.Add(this.groupBox2);
			this.tabBreakables.Location = new System.Drawing.Point(4, 22);
			this.tabBreakables.Name = "tabBreakables";
			this.tabBreakables.Padding = new System.Windows.Forms.Padding(3);
			this.tabBreakables.Size = new System.Drawing.Size(1054, 627);
			this.tabBreakables.TabIndex = 1;
			this.tabBreakables.Text = "Breakables";
			this.tabBreakables.UseVisualStyleBackColor = true;
			// 
			// splitContainer2
			// 
			this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer2.Location = new System.Drawing.Point(6, 86);
			this.splitContainer2.Name = "splitContainer2";
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.buttonBreakableConvert);
			this.splitContainer2.Panel1.Controls.Add(this.listBreakables);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.pictureBreakable);
			this.splitContainer2.Size = new System.Drawing.Size(1042, 535);
			this.splitContainer2.SplitterDistance = 346;
			this.splitContainer2.TabIndex = 9;
			// 
			// buttonBreakableConvert
			// 
			this.buttonBreakableConvert.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.buttonBreakableConvert.Location = new System.Drawing.Point(0, 512);
			this.buttonBreakableConvert.Name = "buttonBreakableConvert";
			this.buttonBreakableConvert.Size = new System.Drawing.Size(346, 23);
			this.buttonBreakableConvert.TabIndex = 3;
			this.buttonBreakableConvert.Text = "Convert all";
			this.buttonBreakableConvert.UseVisualStyleBackColor = true;
			this.buttonBreakableConvert.Click += new System.EventHandler(this.buttonBreakableConvert_Click);
			// 
			// listBreakables
			// 
			this.listBreakables.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listBreakables.FormattingEnabled = true;
			this.listBreakables.IntegralHeight = false;
			this.listBreakables.Location = new System.Drawing.Point(0, 0);
			this.listBreakables.Name = "listBreakables";
			this.listBreakables.Size = new System.Drawing.Size(346, 506);
			this.listBreakables.TabIndex = 2;
			this.listBreakables.SelectedIndexChanged += new System.EventHandler(this.listBreakables_SelectedIndexChanged);
			// 
			// pictureBreakable
			// 
			this.pictureBreakable.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pictureBreakable.Location = new System.Drawing.Point(0, 0);
			this.pictureBreakable.Name = "pictureBreakable";
			this.pictureBreakable.Size = new System.Drawing.Size(692, 535);
			this.pictureBreakable.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBreakable.TabIndex = 0;
			this.pictureBreakable.TabStop = false;
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.textBreakableSource);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.buttonBreakableBrowseSource);
			this.groupBox2.Controls.Add(this.textBreakableDestination);
			this.groupBox2.Controls.Add(this.buttonBreakableBrowseDestination);
			this.groupBox2.Location = new System.Drawing.Point(6, 6);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(1042, 74);
			this.groupBox2.TabIndex = 8;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Directories";
			// 
			// textBreakableSource
			// 
			this.textBreakableSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBreakableSource.Location = new System.Drawing.Point(75, 19);
			this.textBreakableSource.Name = "textBreakableSource";
			this.textBreakableSource.Size = new System.Drawing.Size(880, 20);
			this.textBreakableSource.TabIndex = 1;
			this.textBreakableSource.Text = "E:\\Development\\tiki1\\GameMainContent\\Islands";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(6, 48);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(63, 13);
			this.label3.TabIndex = 6;
			this.label3.Text = "Destination:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(6, 22);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(44, 13);
			this.label4.TabIndex = 3;
			this.label4.Text = "Source:";
			// 
			// buttonBreakableBrowseSource
			// 
			this.buttonBreakableBrowseSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonBreakableBrowseSource.Location = new System.Drawing.Point(961, 17);
			this.buttonBreakableBrowseSource.Name = "buttonBreakableBrowseSource";
			this.buttonBreakableBrowseSource.Size = new System.Drawing.Size(75, 23);
			this.buttonBreakableBrowseSource.TabIndex = 0;
			this.buttonBreakableBrowseSource.Text = "Browse...";
			this.buttonBreakableBrowseSource.UseVisualStyleBackColor = true;
			this.buttonBreakableBrowseSource.Click += new System.EventHandler(this.buttonBreakableBrowseSource_Click);
			// 
			// textBreakableDestination
			// 
			this.textBreakableDestination.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBreakableDestination.Location = new System.Drawing.Point(75, 45);
			this.textBreakableDestination.Name = "textBreakableDestination";
			this.textBreakableDestination.Size = new System.Drawing.Size(880, 20);
			this.textBreakableDestination.TabIndex = 5;
			this.textBreakableDestination.Text = "E:\\Development\\mechanica\\content\\mechanica";
			// 
			// buttonBreakableBrowseDestination
			// 
			this.buttonBreakableBrowseDestination.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonBreakableBrowseDestination.Location = new System.Drawing.Point(961, 43);
			this.buttonBreakableBrowseDestination.Name = "buttonBreakableBrowseDestination";
			this.buttonBreakableBrowseDestination.Size = new System.Drawing.Size(75, 23);
			this.buttonBreakableBrowseDestination.TabIndex = 4;
			this.buttonBreakableBrowseDestination.Text = "Browse...";
			this.buttonBreakableBrowseDestination.UseVisualStyleBackColor = true;
			this.buttonBreakableBrowseDestination.Click += new System.EventHandler(this.buttonBreakableBrowseDestination_Click);
			// 
			// tabIslands
			// 
			this.tabIslands.Controls.Add(this.groupBox1);
			this.tabIslands.Controls.Add(this.splitContainer1);
			this.tabIslands.Location = new System.Drawing.Point(4, 22);
			this.tabIslands.Name = "tabIslands";
			this.tabIslands.Padding = new System.Windows.Forms.Padding(3);
			this.tabIslands.Size = new System.Drawing.Size(1054, 627);
			this.tabIslands.TabIndex = 0;
			this.tabIslands.Text = "Islands";
			this.tabIslands.UseVisualStyleBackColor = true;
			// 
			// FormMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1070, 661);
			this.Controls.Add(this.tabControl);
			this.Name = "FormMain";
			this.Padding = new System.Windows.Forms.Padding(4);
			this.Text = "Converter";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureIsland)).EndInit();
			this.tabControl.ResumeLayout(false);
			this.tabBreakables.ResumeLayout(false);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
			this.splitContainer2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBreakable)).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.tabIslands.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button buttonIslandBrowseSource;
		private System.Windows.Forms.TextBox textIslandSource;
		private System.Windows.Forms.ListBox listIslands;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textIslandDestination;
		private System.Windows.Forms.Button buttonIslandBrowseDestination;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.PictureBox pictureIsland;
		private System.Windows.Forms.Button buttonIslandConvert;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabBreakables;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.Button buttonBreakableConvert;
		private System.Windows.Forms.ListBox listBreakables;
		private System.Windows.Forms.PictureBox pictureBreakable;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TextBox textBreakableSource;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button buttonBreakableBrowseSource;
		private System.Windows.Forms.TextBox textBreakableDestination;
		private System.Windows.Forms.Button buttonBreakableBrowseDestination;
		private System.Windows.Forms.TabPage tabIslands;
	}
}

