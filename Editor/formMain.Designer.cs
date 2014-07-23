namespace TikiEngine.Editor
{
    partial class formMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formMain));
            this.imagesTiles = new System.Windows.Forms.ImageList(this.components);
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panelViewport = new System.Windows.Forms.Panel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.menuLevelEditor = new System.Windows.Forms.ToolStripButton();
            this.menuAnimationEditor = new System.Windows.Forms.ToolStripButton();
            this.menuObjectEditor = new System.Windows.Forms.ToolStripButton();
            this.menuBreakableEditor = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.menuLevelNew = new System.Windows.Forms.ToolStripButton();
            this.menuLevelOpen = new System.Windows.Forms.ToolStripButton();
            this.menuLevelSave = new System.Windows.Forms.ToolStripButton();
            this.menuSaveAs = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuPreferences = new System.Windows.Forms.ToolStripButton();
            this.menuCameraReset = new System.Windows.Forms.ToolStripButton();
            this.tabFiles = new System.Windows.Forms.TabPage();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imagesTiles
            // 
            this.imagesTiles.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imagesTiles.ImageSize = new System.Drawing.Size(16, 16);
            this.imagesTiles.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(723, 438);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(723, 488);
            this.toolStripContainer1.TabIndex = 2;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip2);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panelViewport);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl);
            this.splitContainer1.Size = new System.Drawing.Size(723, 438);
            this.splitContainer1.SplitterDistance = 447;
            this.splitContainer1.TabIndex = 0;
            // 
            // panelViewport
            // 
            this.panelViewport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelViewport.Location = new System.Drawing.Point(0, 0);
            this.panelViewport.Name = "panelViewport";
            this.panelViewport.Size = new System.Drawing.Size(447, 438);
            this.panelViewport.TabIndex = 0;
            this.panelViewport.SizeChanged += new System.EventHandler(this.panelViewport_SizeChanged);
            // 
            // tabControl
            // 
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(272, 438);
            this.tabControl.TabIndex = 1;
            // 
            // toolStrip2
            // 
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuLevelEditor,
            this.menuAnimationEditor,
            this.menuObjectEditor,
            this.menuBreakableEditor});
            this.toolStrip2.Location = new System.Drawing.Point(46, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(104, 25);
            this.toolStrip2.TabIndex = 1;
            // 
            // menuLevelEditor
            // 
            this.menuLevelEditor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.menuLevelEditor.Image = global::TikiEngine.Editor.Properties.Resources.map;
            this.menuLevelEditor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuLevelEditor.Name = "menuLevelEditor";
            this.menuLevelEditor.Size = new System.Drawing.Size(23, 22);
            this.menuLevelEditor.ToolTipText = "Level-Editor";
            this.menuLevelEditor.Click += new System.EventHandler(this.menuLevelEditor_Click);
            // 
            // menuAnimationEditor
            // 
            this.menuAnimationEditor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.menuAnimationEditor.Image = global::TikiEngine.Editor.Properties.Resources.multimedia2;
            this.menuAnimationEditor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuAnimationEditor.Name = "menuAnimationEditor";
            this.menuAnimationEditor.Size = new System.Drawing.Size(23, 22);
            this.menuAnimationEditor.Text = "Animation-Editor";
            this.menuAnimationEditor.Click += new System.EventHandler(this.menuAnimationEditor_Click);
            // 
            // menuObjectEditor
            // 
            this.menuObjectEditor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.menuObjectEditor.Image = global::TikiEngine.Editor.Properties.Resources.objects;
            this.menuObjectEditor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuObjectEditor.Name = "menuObjectEditor";
            this.menuObjectEditor.Size = new System.Drawing.Size(23, 22);
            this.menuObjectEditor.ToolTipText = "Object-Editor";
            this.menuObjectEditor.Click += new System.EventHandler(this.menuObjectEditor_Click);
            // 
            // menuBreakableEditor
            // 
            this.menuBreakableEditor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.menuBreakableEditor.Image = global::TikiEngine.Editor.Properties.Resources.breakable;
            this.menuBreakableEditor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuBreakableEditor.Name = "menuBreakableEditor";
            this.menuBreakableEditor.Size = new System.Drawing.Size(23, 22);
            this.menuBreakableEditor.Text = "Breakable";
            this.menuBreakableEditor.Click += new System.EventHandler(this.menuBreakableEditor_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuLevelNew,
            this.menuLevelOpen,
            this.menuLevelSave,
            this.menuSaveAs,
            this.toolStripSeparator1,
            this.menuPreferences,
            this.menuCameraReset});
            this.toolStrip1.Location = new System.Drawing.Point(3, 25);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(156, 25);
            this.toolStrip1.TabIndex = 0;
            // 
            // menuLevelNew
            // 
            this.menuLevelNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.menuLevelNew.Image = global::TikiEngine.Editor.Properties.Resources.filenew;
            this.menuLevelNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuLevelNew.Name = "menuLevelNew";
            this.menuLevelNew.Size = new System.Drawing.Size(23, 22);
            this.menuLevelNew.Text = "New Level";
            this.menuLevelNew.Click += new System.EventHandler(this.menuLevelNew_Click);
            // 
            // menuLevelOpen
            // 
            this.menuLevelOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.menuLevelOpen.Image = global::TikiEngine.Editor.Properties.Resources.fileopen;
            this.menuLevelOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuLevelOpen.Name = "menuLevelOpen";
            this.menuLevelOpen.Size = new System.Drawing.Size(23, 22);
            this.menuLevelOpen.Text = "Open Level";
            this.menuLevelOpen.Click += new System.EventHandler(this.menuLevelOpen_Click);
            // 
            // menuLevelSave
            // 
            this.menuLevelSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.menuLevelSave.Image = global::TikiEngine.Editor.Properties.Resources.filesave;
            this.menuLevelSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuLevelSave.Name = "menuLevelSave";
            this.menuLevelSave.Size = new System.Drawing.Size(23, 22);
            this.menuLevelSave.Text = "Save Level";
            this.menuLevelSave.Click += new System.EventHandler(this.menuLevelSave_Click);
            // 
            // menuSaveAs
            // 
            this.menuSaveAs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.menuSaveAs.Image = global::TikiEngine.Editor.Properties.Resources.filesaveas;
            this.menuSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuSaveAs.Name = "menuSaveAs";
            this.menuSaveAs.Size = new System.Drawing.Size(23, 22);
            this.menuSaveAs.ToolTipText = "Save as";
            this.menuSaveAs.Click += new System.EventHandler(this.menuSaveAs_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // menuPreferences
            // 
            this.menuPreferences.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.menuPreferences.Image = global::TikiEngine.Editor.Properties.Resources.configure;
            this.menuPreferences.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuPreferences.Name = "menuPreferences";
            this.menuPreferences.Size = new System.Drawing.Size(23, 22);
            this.menuPreferences.Text = "Level propertys";
            this.menuPreferences.Click += new System.EventHandler(this.menuLevelPropertys_Click);
            // 
            // menuCameraReset
            // 
            this.menuCameraReset.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.menuCameraReset.Image = global::TikiEngine.Editor.Properties.Resources.camera;
            this.menuCameraReset.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuCameraReset.Name = "menuCameraReset";
            this.menuCameraReset.Size = new System.Drawing.Size(23, 22);
            this.menuCameraReset.Text = "reset Camera";
            this.menuCameraReset.Click += new System.EventHandler(this.menuCameraReset_Click);
            // 
            // tabFiles
            // 
            this.tabFiles.Location = new System.Drawing.Point(4, 22);
            this.tabFiles.Name = "tabFiles";
            this.tabFiles.Padding = new System.Windows.Forms.Padding(3);
            this.tabFiles.Size = new System.Drawing.Size(264, 412);
            this.tabFiles.TabIndex = 2;
            this.tabFiles.Text = "Animations";
            this.tabFiles.UseVisualStyleBackColor = true;
            // 
            // formMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(723, 488);
            this.Controls.Add(this.toolStripContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "formMain";
            this.Text = "Editor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.formMain_FormClosed);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton menuLevelNew;
        private System.Windows.Forms.ToolStripButton menuLevelOpen;
        private System.Windows.Forms.ToolStripButton menuLevelSave;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton menuAnimationEditor;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton menuPreferences;
        private System.Windows.Forms.ImageList imagesTiles;
        private System.Windows.Forms.TabPage tabFiles;
        private System.Windows.Forms.ToolStripButton menuObjectEditor;
        private System.Windows.Forms.ToolStripButton menuLevelEditor;
        private System.Windows.Forms.ToolStripButton menuSaveAs;
        private System.Windows.Forms.ToolStripButton menuBreakableEditor;
        private System.Windows.Forms.ToolStripButton menuCameraReset;
        internal System.Windows.Forms.Panel panelViewport;
    }
}
