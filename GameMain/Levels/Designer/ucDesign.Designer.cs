#if DESIGNER
namespace TikiEngine.Levels.Designer
{
    partial class ucDesign
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkReady = new System.Windows.Forms.CheckBox();
            this.labelName = new System.Windows.Forms.Label();
            this.labelType = new System.Windows.Forms.Label();
            this.picElement = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabsProperties = new System.Windows.Forms.TabControl();
            this.tabProperties = new System.Windows.Forms.TabPage();
            this.propertiesControl = new TikiEngine.Levels.Designer.ucProperties();
            this.tabAttached = new System.Windows.Forms.TabPage();
            this.buttonAttachedSelect = new System.Windows.Forms.Button();
            this.listAttached = new System.Windows.Forms.ListBox();
            this.buttonAttachedAdd = new System.Windows.Forms.Button();
            this.buttonAttachedRemove = new System.Windows.Forms.Button();
            this.buttonNew = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picElement)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabsProperties.SuspendLayout();
            this.tabProperties.SuspendLayout();
            this.tabAttached.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkReady);
            this.groupBox1.Controls.Add(this.labelName);
            this.groupBox1.Controls.Add(this.labelType);
            this.groupBox1.Controls.Add(this.picElement);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(1, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(348, 173);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Current Element";
            // 
            // checkReady
            // 
            this.checkReady.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkReady.AutoSize = true;
            this.checkReady.Enabled = false;
            this.checkReady.Location = new System.Drawing.Point(288, 145);
            this.checkReady.Name = "checkReady";
            this.checkReady.Size = new System.Drawing.Size(57, 17);
            this.checkReady.TabIndex = 3;
            this.checkReady.Text = "Ready";
            this.checkReady.UseVisualStyleBackColor = true;
            // 
            // labelName
            // 
            this.labelName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(6, 149);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(35, 13);
            this.labelName.TabIndex = 2;
            this.labelName.Text = "Name";
            // 
            // labelType
            // 
            this.labelType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelType.AutoSize = true;
            this.labelType.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelType.Location = new System.Drawing.Point(5, 129);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(43, 20);
            this.labelType.TabIndex = 1;
            this.labelType.Text = "Type";
            // 
            // picElement
            // 
            this.picElement.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.picElement.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.picElement.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picElement.Location = new System.Drawing.Point(6, 19);
            this.picElement.Name = "picElement";
            this.picElement.Size = new System.Drawing.Size(336, 107);
            this.picElement.TabIndex = 0;
            this.picElement.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonRemove);
            this.panel1.Controls.Add(this.buttonSave);
            this.panel1.Controls.Add(this.splitContainer1);
            this.panel1.Controls.Add(this.buttonNew);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(1);
            this.panel1.Size = new System.Drawing.Size(358, 571);
            this.panel1.TabIndex = 2;
            // 
            // buttonRemove
            // 
            this.buttonRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonRemove.Enabled = false;
            this.buttonRemove.Location = new System.Drawing.Point(98, 544);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(88, 23);
            this.buttonRemove.TabIndex = 5;
            this.buttonRemove.Text = "Remove";
            this.buttonRemove.UseVisualStyleBackColor = true;
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonSave.Location = new System.Drawing.Point(192, 544);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(88, 23);
            this.buttonSave.TabIndex = 2;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(4, 4);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabsProperties);
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(1);
            this.splitContainer1.Size = new System.Drawing.Size(350, 534);
            this.splitContainer1.SplitterDistance = 175;
            this.splitContainer1.TabIndex = 4;
            // 
            // tabsProperties
            // 
            this.tabsProperties.Controls.Add(this.tabProperties);
            this.tabsProperties.Controls.Add(this.tabAttached);
            this.tabsProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabsProperties.Enabled = false;
            this.tabsProperties.Location = new System.Drawing.Point(1, 1);
            this.tabsProperties.Name = "tabsProperties";
            this.tabsProperties.SelectedIndex = 0;
            this.tabsProperties.Size = new System.Drawing.Size(348, 353);
            this.tabsProperties.TabIndex = 4;
            // 
            // tabProperties
            // 
            this.tabProperties.Controls.Add(this.propertiesControl);
            this.tabProperties.Location = new System.Drawing.Point(4, 22);
            this.tabProperties.Name = "tabProperties";
            this.tabProperties.Padding = new System.Windows.Forms.Padding(3);
            this.tabProperties.Size = new System.Drawing.Size(340, 327);
            this.tabProperties.TabIndex = 0;
            this.tabProperties.Text = "Properties";
            this.tabProperties.UseVisualStyleBackColor = true;
            // 
            // propertiesControl
            // 
            this.propertiesControl.AutoScroll = true;
            this.propertiesControl.CurrentObject = null;
            this.propertiesControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertiesControl.Location = new System.Drawing.Point(3, 3);
            this.propertiesControl.Name = "propertiesControl";
            this.propertiesControl.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.propertiesControl.SettingsChanged = false;
            this.propertiesControl.Size = new System.Drawing.Size(334, 321);
            this.propertiesControl.TabIndex = 0;
            // 
            // tabAttached
            // 
            this.tabAttached.Controls.Add(this.buttonAttachedSelect);
            this.tabAttached.Controls.Add(this.listAttached);
            this.tabAttached.Controls.Add(this.buttonAttachedAdd);
            this.tabAttached.Controls.Add(this.buttonAttachedRemove);
            this.tabAttached.Location = new System.Drawing.Point(4, 22);
            this.tabAttached.Name = "tabAttached";
            this.tabAttached.Padding = new System.Windows.Forms.Padding(3);
            this.tabAttached.Size = new System.Drawing.Size(340, 327);
            this.tabAttached.TabIndex = 1;
            this.tabAttached.Text = "Attached";
            this.tabAttached.UseVisualStyleBackColor = true;
            // 
            // buttonAttachedSelect
            // 
            this.buttonAttachedSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAttachedSelect.Location = new System.Drawing.Point(212, 298);
            this.buttonAttachedSelect.Name = "buttonAttachedSelect";
            this.buttonAttachedSelect.Size = new System.Drawing.Size(58, 23);
            this.buttonAttachedSelect.TabIndex = 9;
            this.buttonAttachedSelect.Text = "Select";
            this.buttonAttachedSelect.UseVisualStyleBackColor = true;
            this.buttonAttachedSelect.Click += new System.EventHandler(this.buttonAttachedSelect_Click);
            // 
            // listAttached
            // 
            this.listAttached.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listAttached.FormattingEnabled = true;
            this.listAttached.IntegralHeight = false;
            this.listAttached.Location = new System.Drawing.Point(6, 6);
            this.listAttached.Name = "listAttached";
            this.listAttached.Size = new System.Drawing.Size(328, 286);
            this.listAttached.TabIndex = 8;
            this.listAttached.DoubleClick += new System.EventHandler(this.buttonAttachedSelect_Click);
            // 
            // buttonAttachedAdd
            // 
            this.buttonAttachedAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAttachedAdd.Location = new System.Drawing.Point(148, 298);
            this.buttonAttachedAdd.Name = "buttonAttachedAdd";
            this.buttonAttachedAdd.Size = new System.Drawing.Size(58, 23);
            this.buttonAttachedAdd.TabIndex = 7;
            this.buttonAttachedAdd.Text = "Add";
            this.buttonAttachedAdd.UseVisualStyleBackColor = true;
            this.buttonAttachedAdd.Click += new System.EventHandler(this.buttonAttachedAdd_Click);
            // 
            // buttonAttachedRemove
            // 
            this.buttonAttachedRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAttachedRemove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonAttachedRemove.Location = new System.Drawing.Point(276, 298);
            this.buttonAttachedRemove.Name = "buttonAttachedRemove";
            this.buttonAttachedRemove.Size = new System.Drawing.Size(58, 23);
            this.buttonAttachedRemove.TabIndex = 6;
            this.buttonAttachedRemove.Text = "Remove";
            this.buttonAttachedRemove.UseVisualStyleBackColor = true;
            this.buttonAttachedRemove.Click += new System.EventHandler(this.buttonAttachedRemove_Click);
            // 
            // buttonNew
            // 
            this.buttonNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonNew.Location = new System.Drawing.Point(4, 544);
            this.buttonNew.Name = "buttonNew";
            this.buttonNew.Size = new System.Drawing.Size(88, 23);
            this.buttonNew.TabIndex = 3;
            this.buttonNew.Text = "New Element";
            this.buttonNew.UseVisualStyleBackColor = true;
            this.buttonNew.Click += new System.EventHandler(this.buttonNew_Click);
            // 
            // ucDesign
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Location = new System.Drawing.Point(1300, 100);
            this.Name = "ucDesign";
            this.Size = new System.Drawing.Size(358, 571);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picElement)).EndInit();
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabsProperties.ResumeLayout(false);
            this.tabProperties.ResumeLayout(false);
            this.tabAttached.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label labelType;
        private System.Windows.Forms.PictureBox picElement;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonNew;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button buttonRemove;
        private ucProperties propertiesControl;
        private System.Windows.Forms.CheckBox checkReady;
        private System.Windows.Forms.Button buttonAttachedRemove;
        private System.Windows.Forms.TabControl tabsProperties;
        private System.Windows.Forms.TabPage tabProperties;
        private System.Windows.Forms.TabPage tabAttached;
        private System.Windows.Forms.ListBox listAttached;
        private System.Windows.Forms.Button buttonAttachedAdd;
        private System.Windows.Forms.Button buttonAttachedSelect;
    }
}
#endif