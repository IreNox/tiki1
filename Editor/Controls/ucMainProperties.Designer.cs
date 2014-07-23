namespace TikiEngine.Editor.Controls
{
    partial class ucMainProperties
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.propertys = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // propertys
            // 
            this.propertys.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertys.HelpVisible = false;
            this.propertys.Location = new System.Drawing.Point(2, 2);
            this.propertys.Name = "propertys";
            this.propertys.Size = new System.Drawing.Size(198, 404);
            this.propertys.TabIndex = 1;
            this.propertys.ToolbarVisible = false;
            // 
            // ucLevelPropertys
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.propertys);
            this.Name = "ucLevelPropertys";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Size = new System.Drawing.Size(202, 408);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propertys;
    }
}
