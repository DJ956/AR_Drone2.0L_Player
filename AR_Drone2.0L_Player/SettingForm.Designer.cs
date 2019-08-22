namespace AR_Drone2._0L_Player
{
    partial class SettingForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingForm));
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.label_Up = new System.Windows.Forms.Label();
            this.trackBar_Up = new System.Windows.Forms.TrackBar();
            this.trackBar_Down = new System.Windows.Forms.TrackBar();
            this.label_Down = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.trackBar_Left = new System.Windows.Forms.TrackBar();
            this.trackBar_Right = new System.Windows.Forms.TrackBar();
            this.label_Left = new System.Windows.Forms.Label();
            this.label_Right = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.trackBar_Forward = new System.Windows.Forms.TrackBar();
            this.trackBar_Back = new System.Windows.Forms.TrackBar();
            this.label_Forward = new System.Windows.Forms.Label();
            this.label_Back = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.trackBar_TurnLeft = new System.Windows.Forms.TrackBar();
            this.trackBar_TurnRight = new System.Windows.Forms.TrackBar();
            this.label_TurnLeft = new System.Windows.Forms.Label();
            this.label_TurnRight = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Up)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Down)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Left)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Right)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Forward)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Back)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_TurnLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_TurnRight)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(541, 222);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 0;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.ButtonSave_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(639, 222);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 1;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // label_Up
            // 
            this.label_Up.AutoSize = true;
            this.label_Up.Location = new System.Drawing.Point(7, 29);
            this.label_Up.Name = "label_Up";
            this.label_Up.Size = new System.Drawing.Size(21, 12);
            this.label_Up.TabIndex = 2;
            this.label_Up.Text = "Up:";
            // 
            // trackBar_Up
            // 
            this.trackBar_Up.Location = new System.Drawing.Point(6, 44);
            this.trackBar_Up.Maximum = 100;
            this.trackBar_Up.Minimum = -100;
            this.trackBar_Up.Name = "trackBar_Up";
            this.trackBar_Up.Size = new System.Drawing.Size(159, 45);
            this.trackBar_Up.TabIndex = 1;
            this.trackBar_Up.TickFrequency = 10;
            this.trackBar_Up.ValueChanged += new System.EventHandler(this.trackBarValueChange);
            // 
            // trackBar_Down
            // 
            this.trackBar_Down.Location = new System.Drawing.Point(185, 44);
            this.trackBar_Down.Maximum = 100;
            this.trackBar_Down.Minimum = -100;
            this.trackBar_Down.Name = "trackBar_Down";
            this.trackBar_Down.Size = new System.Drawing.Size(159, 45);
            this.trackBar_Down.TabIndex = 5;
            this.trackBar_Down.TickFrequency = 10;
            this.trackBar_Down.ValueChanged += new System.EventHandler(this.trackBarValueChange);
            // 
            // label_Down
            // 
            this.label_Down.AutoSize = true;
            this.label_Down.Location = new System.Drawing.Point(186, 29);
            this.label_Down.Name = "label_Down";
            this.label_Down.Size = new System.Drawing.Size(35, 12);
            this.label_Down.TabIndex = 4;
            this.label_Down.Text = "Down:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.trackBar_Up);
            this.groupBox1.Controls.Add(this.trackBar_Down);
            this.groupBox1.Controls.Add(this.label_Up);
            this.groupBox1.Controls.Add(this.label_Down);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(352, 99);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Up-Down";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.trackBar_Left);
            this.groupBox2.Controls.Add(this.trackBar_Right);
            this.groupBox2.Controls.Add(this.label_Left);
            this.groupBox2.Controls.Add(this.label_Right);
            this.groupBox2.Location = new System.Drawing.Point(370, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(352, 99);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Left-Right";
            // 
            // trackBar_Left
            // 
            this.trackBar_Left.Location = new System.Drawing.Point(6, 44);
            this.trackBar_Left.Maximum = 100;
            this.trackBar_Left.Minimum = -100;
            this.trackBar_Left.Name = "trackBar_Left";
            this.trackBar_Left.Size = new System.Drawing.Size(159, 45);
            this.trackBar_Left.TabIndex = 3;
            this.trackBar_Left.TickFrequency = 10;
            this.trackBar_Left.ValueChanged += new System.EventHandler(this.trackBarValueChange);
            // 
            // trackBar_Right
            // 
            this.trackBar_Right.Location = new System.Drawing.Point(185, 44);
            this.trackBar_Right.Maximum = 100;
            this.trackBar_Right.Minimum = -100;
            this.trackBar_Right.Name = "trackBar_Right";
            this.trackBar_Right.Size = new System.Drawing.Size(159, 45);
            this.trackBar_Right.TabIndex = 5;
            this.trackBar_Right.TickFrequency = 10;
            this.trackBar_Right.ValueChanged += new System.EventHandler(this.trackBarValueChange);
            // 
            // label_Left
            // 
            this.label_Left.AutoSize = true;
            this.label_Left.Location = new System.Drawing.Point(7, 29);
            this.label_Left.Name = "label_Left";
            this.label_Left.Size = new System.Drawing.Size(27, 12);
            this.label_Left.TabIndex = 2;
            this.label_Left.Text = "Left:";
            // 
            // label_Right
            // 
            this.label_Right.AutoSize = true;
            this.label_Right.Location = new System.Drawing.Point(186, 29);
            this.label_Right.Name = "label_Right";
            this.label_Right.Size = new System.Drawing.Size(34, 12);
            this.label_Right.TabIndex = 4;
            this.label_Right.Text = "Right:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.trackBar_Forward);
            this.groupBox3.Controls.Add(this.trackBar_Back);
            this.groupBox3.Controls.Add(this.label_Forward);
            this.groupBox3.Controls.Add(this.label_Back);
            this.groupBox3.Location = new System.Drawing.Point(12, 117);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(352, 99);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Forward-Back";
            // 
            // trackBar_Forward
            // 
            this.trackBar_Forward.Location = new System.Drawing.Point(6, 44);
            this.trackBar_Forward.Maximum = 100;
            this.trackBar_Forward.Minimum = -100;
            this.trackBar_Forward.Name = "trackBar_Forward";
            this.trackBar_Forward.Size = new System.Drawing.Size(159, 45);
            this.trackBar_Forward.TabIndex = 3;
            this.trackBar_Forward.TickFrequency = 10;
            this.trackBar_Forward.ValueChanged += new System.EventHandler(this.trackBarValueChange);
            // 
            // trackBar_Back
            // 
            this.trackBar_Back.Location = new System.Drawing.Point(185, 44);
            this.trackBar_Back.Maximum = 100;
            this.trackBar_Back.Minimum = -100;
            this.trackBar_Back.Name = "trackBar_Back";
            this.trackBar_Back.Size = new System.Drawing.Size(159, 45);
            this.trackBar_Back.TabIndex = 5;
            this.trackBar_Back.TickFrequency = 10;
            this.trackBar_Back.ValueChanged += new System.EventHandler(this.trackBarValueChange);
            // 
            // label_Forward
            // 
            this.label_Forward.AutoSize = true;
            this.label_Forward.Location = new System.Drawing.Point(7, 29);
            this.label_Forward.Name = "label_Forward";
            this.label_Forward.Size = new System.Drawing.Size(48, 12);
            this.label_Forward.TabIndex = 2;
            this.label_Forward.Text = "Forward:";
            // 
            // label_Back
            // 
            this.label_Back.AutoSize = true;
            this.label_Back.Location = new System.Drawing.Point(186, 29);
            this.label_Back.Name = "label_Back";
            this.label_Back.Size = new System.Drawing.Size(33, 12);
            this.label_Back.TabIndex = 4;
            this.label_Back.Text = "Back:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.trackBar_TurnLeft);
            this.groupBox4.Controls.Add(this.trackBar_TurnRight);
            this.groupBox4.Controls.Add(this.label_TurnLeft);
            this.groupBox4.Controls.Add(this.label_TurnRight);
            this.groupBox4.Location = new System.Drawing.Point(370, 117);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(352, 99);
            this.groupBox4.TabIndex = 8;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Turn Left-Right";
            // 
            // trackBar_TurnLeft
            // 
            this.trackBar_TurnLeft.Location = new System.Drawing.Point(6, 44);
            this.trackBar_TurnLeft.Maximum = 100;
            this.trackBar_TurnLeft.Minimum = -100;
            this.trackBar_TurnLeft.Name = "trackBar_TurnLeft";
            this.trackBar_TurnLeft.Size = new System.Drawing.Size(159, 45);
            this.trackBar_TurnLeft.TabIndex = 3;
            this.trackBar_TurnLeft.TickFrequency = 10;
            this.trackBar_TurnLeft.ValueChanged += new System.EventHandler(this.trackBarValueChange);
            // 
            // trackBar_TurnRight
            // 
            this.trackBar_TurnRight.Location = new System.Drawing.Point(185, 44);
            this.trackBar_TurnRight.Maximum = 100;
            this.trackBar_TurnRight.Minimum = -100;
            this.trackBar_TurnRight.Name = "trackBar_TurnRight";
            this.trackBar_TurnRight.Size = new System.Drawing.Size(159, 45);
            this.trackBar_TurnRight.TabIndex = 5;
            this.trackBar_TurnRight.TickFrequency = 10;
            this.trackBar_TurnRight.ValueChanged += new System.EventHandler(this.trackBarValueChange);
            // 
            // label_TurnLeft
            // 
            this.label_TurnLeft.AutoSize = true;
            this.label_TurnLeft.Location = new System.Drawing.Point(7, 29);
            this.label_TurnLeft.Name = "label_TurnLeft";
            this.label_TurnLeft.Size = new System.Drawing.Size(54, 12);
            this.label_TurnLeft.TabIndex = 2;
            this.label_TurnLeft.Text = "Turn Left:";
            // 
            // label_TurnRight
            // 
            this.label_TurnRight.AutoSize = true;
            this.label_TurnRight.Location = new System.Drawing.Point(186, 29);
            this.label_TurnRight.Name = "label_TurnRight";
            this.label_TurnRight.Size = new System.Drawing.Size(61, 12);
            this.label_TurnRight.TabIndex = 4;
            this.label_TurnRight.Text = "Turn Right:";
            // 
            // SettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(739, 260);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonSave);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingForm";
            this.Text = "Drone Setting";
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Up)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Down)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Left)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Right)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Forward)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Back)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_TurnLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_TurnRight)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Label label_Up;
        private System.Windows.Forms.TrackBar trackBar_Up;
        private System.Windows.Forms.TrackBar trackBar_Down;
        private System.Windows.Forms.Label label_Down;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TrackBar trackBar_Left;
        private System.Windows.Forms.TrackBar trackBar_Right;
        private System.Windows.Forms.Label label_Left;
        private System.Windows.Forms.Label label_Right;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TrackBar trackBar_Forward;
        private System.Windows.Forms.TrackBar trackBar_Back;
        private System.Windows.Forms.Label label_Forward;
        private System.Windows.Forms.Label label_Back;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TrackBar trackBar_TurnLeft;
        private System.Windows.Forms.TrackBar trackBar_TurnRight;
        private System.Windows.Forms.Label label_TurnLeft;
        private System.Windows.Forms.Label label_TurnRight;
    }
}