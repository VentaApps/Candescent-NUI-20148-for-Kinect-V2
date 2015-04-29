namespace CCT.NUI.Samples
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.buttonClustering = new System.Windows.Forms.Button();
            this.buttonDepth = new System.Windows.Forms.Button();
            this.buttonRGB = new System.Windows.Forms.Button();
            this.buttonExit = new System.Windows.Forms.Button();
            this.labelTBC = new System.Windows.Forms.Label();
            this.linkLabelBlog = new System.Windows.Forms.LinkLabel();
            this.linkLabelSource = new System.Windows.Forms.LinkLabel();
            this.buttonHandAndFinger = new System.Windows.Forms.Button();
            this.buttonSettings = new System.Windows.Forms.Button();
            this.radioButtonOpenNI = new System.Windows.Forms.RadioButton();
            this.radioButtonSDK = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonImageManipulation = new System.Windows.Forms.Button();
            this.videoControl = new CCT.NUI.Visual.VideoControl();
            this.buttonHandDataFactory = new System.Windows.Forms.Button();
            this.buttonHandTracking = new System.Windows.Forms.Button();
            this.radioOpenNINite = new System.Windows.Forms.RadioButton();
            this.radioButtonKinectWONear = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // buttonClustering
            // 
            this.buttonClustering.Enabled = false;
            this.buttonClustering.Location = new System.Drawing.Point(12, 229);
            this.buttonClustering.Name = "buttonClustering";
            this.buttonClustering.Size = new System.Drawing.Size(246, 43);
            this.buttonClustering.TabIndex = 6;
            this.buttonClustering.Text = "Clustering";
            this.buttonClustering.UseVisualStyleBackColor = true;
            this.buttonClustering.Click += new System.EventHandler(this.buttonClustering_Click);
            // 
            // buttonDepth
            // 
            this.buttonDepth.Enabled = false;
            this.buttonDepth.Image = ((System.Drawing.Image)(resources.GetObject("buttonDepth.Image")));
            this.buttonDepth.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonDepth.Location = new System.Drawing.Point(12, 180);
            this.buttonDepth.Name = "buttonDepth";
            this.buttonDepth.Size = new System.Drawing.Size(246, 43);
            this.buttonDepth.TabIndex = 5;
            this.buttonDepth.Text = "Depth Image";
            this.buttonDepth.UseVisualStyleBackColor = true;
            this.buttonDepth.Click += new System.EventHandler(this.buttonDepth_Click);
            // 
            // buttonRGB
            // 
            this.buttonRGB.Enabled = false;
            this.buttonRGB.Image = ((System.Drawing.Image)(resources.GetObject("buttonRGB.Image")));
            this.buttonRGB.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonRGB.Location = new System.Drawing.Point(12, 131);
            this.buttonRGB.Name = "buttonRGB";
            this.buttonRGB.Size = new System.Drawing.Size(246, 43);
            this.buttonRGB.TabIndex = 4;
            this.buttonRGB.Text = "RGB Image";
            this.buttonRGB.UseVisualStyleBackColor = true;
            this.buttonRGB.Click += new System.EventHandler(this.buttonRGB_Click);
            // 
            // buttonExit
            // 
            this.buttonExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonExit.Location = new System.Drawing.Point(9, 617);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(246, 43);
            this.buttonExit.TabIndex = 11;
            this.buttonExit.Text = "Exit";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // labelTBC
            // 
            this.labelTBC.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTBC.Location = new System.Drawing.Point(9, 485);
            this.labelTBC.Name = "labelTBC";
            this.labelTBC.Size = new System.Drawing.Size(246, 22);
            this.labelTBC.TabIndex = 5;
            this.labelTBC.Text = "...";
            this.labelTBC.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // linkLabelBlog
            // 
            this.linkLabelBlog.AutoSize = true;
            this.linkLabelBlog.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabelBlog.Location = new System.Drawing.Point(12, 37);
            this.linkLabelBlog.Name = "linkLabelBlog";
            this.linkLabelBlog.Size = new System.Drawing.Size(167, 15);
            this.linkLabelBlog.TabIndex = 1;
            this.linkLabelBlog.TabStop = true;
            this.linkLabelBlog.Text = "http://blog.candescent.ch";
            this.linkLabelBlog.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelBlog_LinkClicked);
            // 
            // linkLabelSource
            // 
            this.linkLabelSource.AutoSize = true;
            this.linkLabelSource.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabelSource.Location = new System.Drawing.Point(12, 12);
            this.linkLabelSource.Name = "linkLabelSource";
            this.linkLabelSource.Size = new System.Drawing.Size(233, 15);
            this.linkLabelSource.TabIndex = 0;
            this.linkLabelSource.TabStop = true;
            this.linkLabelSource.Text = "http://candescentnui.codeplex.com/";
            this.linkLabelSource.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelSource_LinkClicked);
            // 
            // buttonHandAndFinger
            // 
            this.buttonHandAndFinger.Enabled = false;
            this.buttonHandAndFinger.Location = new System.Drawing.Point(12, 278);
            this.buttonHandAndFinger.Name = "buttonHandAndFinger";
            this.buttonHandAndFinger.Size = new System.Drawing.Size(246, 43);
            this.buttonHandAndFinger.TabIndex = 7;
            this.buttonHandAndFinger.Text = "Hand and Finger Detection";
            this.buttonHandAndFinger.UseVisualStyleBackColor = true;
            this.buttonHandAndFinger.Click += new System.EventHandler(this.buttonHandAndFinger_Click);
            // 
            // buttonSettings
            // 
            this.buttonSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSettings.Location = new System.Drawing.Point(9, 568);
            this.buttonSettings.Name = "buttonSettings";
            this.buttonSettings.Size = new System.Drawing.Size(246, 43);
            this.buttonSettings.TabIndex = 10;
            this.buttonSettings.Text = "Settings";
            this.buttonSettings.UseVisualStyleBackColor = true;
            this.buttonSettings.Click += new System.EventHandler(this.buttonSettings_Click);
            // 
            // radioButtonOpenNI
            // 
            this.radioButtonOpenNI.AutoSize = true;
            this.radioButtonOpenNI.Location = new System.Drawing.Point(15, 85);
            this.radioButtonOpenNI.Name = "radioButtonOpenNI";
            this.radioButtonOpenNI.Size = new System.Drawing.Size(62, 17);
            this.radioButtonOpenNI.TabIndex = 2;
            this.radioButtonOpenNI.Text = "OpenNI";
            this.radioButtonOpenNI.UseVisualStyleBackColor = true;
            this.radioButtonOpenNI.CheckedChanged += new System.EventHandler(this.radioButtonOpenNI_CheckedChanged);
            // 
            // radioButtonSDK
            // 
            this.radioButtonSDK.AutoSize = true;
            this.radioButtonSDK.Location = new System.Drawing.Point(15, 108);
            this.radioButtonSDK.Name = "radioButtonSDK";
            this.radioButtonSDK.Size = new System.Drawing.Size(80, 17);
            this.radioButtonSDK.TabIndex = 3;
            this.radioButtonSDK.Text = "Kinect SDK";
            this.radioButtonSDK.UseVisualStyleBackColor = true;
            this.radioButtonSDK.CheckedChanged += new System.EventHandler(this.radioButtonSDK_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Please select your framework:";
            // 
            // buttonImageManipulation
            // 
            this.buttonImageManipulation.Enabled = false;
            this.buttonImageManipulation.Location = new System.Drawing.Point(12, 327);
            this.buttonImageManipulation.Name = "buttonImageManipulation";
            this.buttonImageManipulation.Size = new System.Drawing.Size(246, 43);
            this.buttonImageManipulation.TabIndex = 8;
            this.buttonImageManipulation.Text = "Image Manipulation";
            this.buttonImageManipulation.UseVisualStyleBackColor = true;
            this.buttonImageManipulation.Click += new System.EventHandler(this.buttonImageManipulation_Click);
            // 
            // videoControl
            // 
            this.videoControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.videoControl.BackColor = System.Drawing.Color.Black;
            this.videoControl.Location = new System.Drawing.Point(264, 12);
            this.videoControl.Name = "videoControl";
            this.videoControl.Size = new System.Drawing.Size(640, 652);
            this.videoControl.Stretch = false;
            this.videoControl.TabIndex = 8;
            // 
            // buttonHandDataFactory
            // 
            this.buttonHandDataFactory.Enabled = false;
            this.buttonHandDataFactory.Location = new System.Drawing.Point(12, 376);
            this.buttonHandDataFactory.Name = "buttonHandDataFactory";
            this.buttonHandDataFactory.Size = new System.Drawing.Size(246, 43);
            this.buttonHandDataFactory.TabIndex = 9;
            this.buttonHandDataFactory.Text = "Hand Data Factory";
            this.buttonHandDataFactory.UseVisualStyleBackColor = true;
            this.buttonHandDataFactory.Click += new System.EventHandler(this.buttonHandDataFactory_Click);
            // 
            // buttonHandTracking
            // 
            this.buttonHandTracking.Enabled = false;
            this.buttonHandTracking.Location = new System.Drawing.Point(12, 425);
            this.buttonHandTracking.Name = "buttonHandTracking";
            this.buttonHandTracking.Size = new System.Drawing.Size(246, 43);
            this.buttonHandTracking.TabIndex = 12;
            this.buttonHandTracking.Text = "Hand Tracking (NITE)";
            this.buttonHandTracking.UseVisualStyleBackColor = true;
            this.buttonHandTracking.Click += new System.EventHandler(this.buttonHandTracking_Click);
            // 
            // radioOpenNINite
            // 
            this.radioOpenNINite.AutoSize = true;
            this.radioOpenNINite.Location = new System.Drawing.Point(80, 85);
            this.radioOpenNINite.Name = "radioOpenNINite";
            this.radioOpenNINite.Size = new System.Drawing.Size(99, 17);
            this.radioOpenNINite.TabIndex = 13;
            this.radioOpenNINite.Text = "OpenNI + NITE";
            this.radioOpenNINite.UseVisualStyleBackColor = true;
            this.radioOpenNINite.CheckedChanged += new System.EventHandler(this.radioOpenNINite_CheckedChanged);
            // 
            // radioButtonKinectWONear
            // 
            this.radioButtonKinectWONear.AutoSize = true;
            this.radioButtonKinectWONear.Location = new System.Drawing.Point(101, 108);
            this.radioButtonKinectWONear.Name = "radioButtonKinectWONear";
            this.radioButtonKinectWONear.Size = new System.Drawing.Size(155, 17);
            this.radioButtonKinectWONear.TabIndex = 14;
            this.radioButtonKinectWONear.Text = "Kinect SDK w/o near mode";
            this.radioButtonKinectWONear.UseVisualStyleBackColor = true;
            this.radioButtonKinectWONear.CheckedChanged += new System.EventHandler(this.radioButtonKinectWONear_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(914, 672);
            this.Controls.Add(this.radioButtonKinectWONear);
            this.Controls.Add(this.radioOpenNINite);
            this.Controls.Add(this.buttonHandTracking);
            this.Controls.Add(this.buttonHandDataFactory);
            this.Controls.Add(this.buttonImageManipulation);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.radioButtonSDK);
            this.Controls.Add(this.radioButtonOpenNI);
            this.Controls.Add(this.buttonSettings);
            this.Controls.Add(this.buttonHandAndFinger);
            this.Controls.Add(this.linkLabelSource);
            this.Controls.Add(this.linkLabelBlog);
            this.Controls.Add(this.labelTBC);
            this.Controls.Add(this.videoControl);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.buttonRGB);
            this.Controls.Add(this.buttonDepth);
            this.Controls.Add(this.buttonClustering);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Candescent NUI Samples";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonClustering;
        private System.Windows.Forms.Button buttonDepth;
        private System.Windows.Forms.Button buttonRGB;
        private System.Windows.Forms.Button buttonExit;
        private CCT.NUI.Visual.VideoControl videoControl;
        private System.Windows.Forms.Label labelTBC;
        private System.Windows.Forms.LinkLabel linkLabelBlog;
        private System.Windows.Forms.LinkLabel linkLabelSource;
        private System.Windows.Forms.Button buttonHandAndFinger;
        private System.Windows.Forms.Button buttonSettings;
        private System.Windows.Forms.RadioButton radioButtonOpenNI;
        private System.Windows.Forms.RadioButton radioButtonSDK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonImageManipulation;
        private System.Windows.Forms.Button buttonHandDataFactory;
        private System.Windows.Forms.Button buttonHandTracking;
        private System.Windows.Forms.RadioButton radioOpenNINite;
        private System.Windows.Forms.RadioButton radioButtonKinectWONear;
    }
}

