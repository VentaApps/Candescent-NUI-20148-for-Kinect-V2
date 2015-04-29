namespace CCT.NUI.Samples
{
    partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonClose = new System.Windows.Forms.ToolStripButton();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.propertyGridClustering = new System.Windows.Forms.PropertyGrid();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.propertyGridShape = new System.Windows.Forms.PropertyGrid();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.propertyGridHandDetection = new System.Windows.Forms.PropertyGrid();
            this.toolStrip1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonClose});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(335, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonClose
            // 
            this.toolStripButtonClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonClose.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonClose.Image")));
            this.toolStripButtonClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonClose.Name = "toolStripButtonClose";
            this.toolStripButtonClose.Size = new System.Drawing.Size(40, 22);
            this.toolStripButtonClose.Text = "Close";
            this.toolStripButtonClose.Click += new System.EventHandler(this.toolStripButtonClose_Click);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Controls.Add(this.tabPage3);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 25);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(335, 576);
            this.tabControl.TabIndex = 14;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.propertyGridClustering);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(327, 550);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Clustering";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // propertyGridClustering
            // 
            this.propertyGridClustering.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridClustering.Location = new System.Drawing.Point(3, 3);
            this.propertyGridClustering.Name = "propertyGridClustering";
            this.propertyGridClustering.Size = new System.Drawing.Size(321, 544);
            this.propertyGridClustering.TabIndex = 13;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.propertyGridShape);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(327, 550);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Shape";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // propertyGridShape
            // 
            this.propertyGridShape.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridShape.Location = new System.Drawing.Point(3, 3);
            this.propertyGridShape.Name = "propertyGridShape";
            this.propertyGridShape.Size = new System.Drawing.Size(321, 544);
            this.propertyGridShape.TabIndex = 15;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.propertyGridHandDetection);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(327, 550);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Hand";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // propertyGridHandDetection
            // 
            this.propertyGridHandDetection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridHandDetection.Location = new System.Drawing.Point(0, 0);
            this.propertyGridHandDetection.Name = "propertyGridHandDetection";
            this.propertyGridHandDetection.Size = new System.Drawing.Size(327, 550);
            this.propertyGridHandDetection.TabIndex = 15;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 601);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonClose;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.PropertyGrid propertyGridClustering;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.PropertyGrid propertyGridShape;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.PropertyGrid propertyGridHandDetection;
    }
}