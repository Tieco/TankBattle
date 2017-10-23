namespace TankBattle
{
    partial class GameForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameForm));
            this.displayPanel = new System.Windows.Forms.Panel();
            this.controlPanel = new System.Windows.Forms.Panel();
            this.btnFire = new System.Windows.Forms.Button();
            this.lblPowerValue = new System.Windows.Forms.Label();
            this.tbPower = new System.Windows.Forms.TrackBar();
            this.lblPower = new System.Windows.Forms.Label();
            this.numUpDownAngle = new System.Windows.Forms.NumericUpDown();
            this.lblAngle = new System.Windows.Forms.Label();
            this.cmbWeapon = new System.Windows.Forms.ComboBox();
            this.lblWeapon = new System.Windows.Forms.Label();
            this.lblWindValue = new System.Windows.Forms.Label();
            this.lblWind = new System.Windows.Forms.Label();
            this.lblPlayerName = new System.Windows.Forms.Label();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.controlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbPower)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownAngle)).BeginInit();
            this.SuspendLayout();
            // 
            // displayPanel
            // 
            this.displayPanel.Location = new System.Drawing.Point(0, 39);
            this.displayPanel.Margin = new System.Windows.Forms.Padding(4);
            this.displayPanel.Name = "displayPanel";
            this.displayPanel.Size = new System.Drawing.Size(1067, 738);
            this.displayPanel.TabIndex = 0;
            this.displayPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.displayPanel_Paint);
            // 
            // controlPanel
            // 
            this.controlPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.controlPanel.BackColor = System.Drawing.Color.OrangeRed;
            this.controlPanel.Controls.Add(this.btnFire);
            this.controlPanel.Controls.Add(this.lblPowerValue);
            this.controlPanel.Controls.Add(this.tbPower);
            this.controlPanel.Controls.Add(this.lblPower);
            this.controlPanel.Controls.Add(this.numUpDownAngle);
            this.controlPanel.Controls.Add(this.lblAngle);
            this.controlPanel.Controls.Add(this.cmbWeapon);
            this.controlPanel.Controls.Add(this.lblWeapon);
            this.controlPanel.Controls.Add(this.lblWindValue);
            this.controlPanel.Controls.Add(this.lblWind);
            this.controlPanel.Controls.Add(this.lblPlayerName);
            this.controlPanel.Enabled = false;
            this.controlPanel.Location = new System.Drawing.Point(0, 0);
            this.controlPanel.Margin = new System.Windows.Forms.Padding(4);
            this.controlPanel.Name = "controlPanel";
            this.controlPanel.Size = new System.Drawing.Size(1067, 39);
            this.controlPanel.TabIndex = 1;
            // 
            // btnFire
            // 
            this.btnFire.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFire.Location = new System.Drawing.Point(967, 5);
            this.btnFire.Margin = new System.Windows.Forms.Padding(4);
            this.btnFire.Name = "btnFire";
            this.btnFire.Size = new System.Drawing.Size(96, 28);
            this.btnFire.TabIndex = 10;
            this.btnFire.Text = "Fire!";
            this.btnFire.UseVisualStyleBackColor = true;
            this.btnFire.Click += new System.EventHandler(this.btnFire_Click);
            // 
            // lblPowerValue
            // 
            this.lblPowerValue.AutoSize = true;
            this.lblPowerValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPowerValue.Location = new System.Drawing.Point(923, 9);
            this.lblPowerValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPowerValue.Name = "lblPowerValue";
            this.lblPowerValue.Size = new System.Drawing.Size(34, 25);
            this.lblPowerValue.TabIndex = 9;
            this.lblPowerValue.Text = "20";
            // 
            // tbPower
            // 
            this.tbPower.LargeChange = 10;
            this.tbPower.Location = new System.Drawing.Point(799, -16);
            this.tbPower.Margin = new System.Windows.Forms.Padding(4);
            this.tbPower.Maximum = 100;
            this.tbPower.Minimum = 5;
            this.tbPower.Name = "tbPower";
            this.tbPower.Size = new System.Drawing.Size(115, 56);
            this.tbPower.TabIndex = 8;
            this.tbPower.Value = 5;
            this.tbPower.ValueChanged += new System.EventHandler(this.tbPower_ValueChanged);
            // 
            // lblPower
            // 
            this.lblPower.AutoSize = true;
            this.lblPower.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPower.Location = new System.Drawing.Point(709, 7);
            this.lblPower.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPower.Name = "lblPower";
            this.lblPower.Size = new System.Drawing.Size(78, 25);
            this.lblPower.TabIndex = 7;
            this.lblPower.Text = "Power :";
            // 
            // numUpDownAngle
            // 
            this.numUpDownAngle.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numUpDownAngle.Location = new System.Drawing.Point(613, 7);
            this.numUpDownAngle.Margin = new System.Windows.Forms.Padding(4);
            this.numUpDownAngle.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.numUpDownAngle.Minimum = new decimal(new int[] {
            90,
            0,
            0,
            -2147483648});
            this.numUpDownAngle.Name = "numUpDownAngle";
            this.numUpDownAngle.Size = new System.Drawing.Size(64, 22);
            this.numUpDownAngle.TabIndex = 6;
            this.numUpDownAngle.ValueChanged += new System.EventHandler(this.numUpDownAngle_ValueChanged);
            // 
            // lblAngle
            // 
            this.lblAngle.AutoSize = true;
            this.lblAngle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAngle.Location = new System.Drawing.Point(533, 7);
            this.lblAngle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAngle.Name = "lblAngle";
            this.lblAngle.Size = new System.Drawing.Size(74, 25);
            this.lblAngle.TabIndex = 5;
            this.lblAngle.Text = "Angle :";
            // 
            // cmbWeapon
            // 
            this.cmbWeapon.FormattingEnabled = true;
            this.cmbWeapon.Location = new System.Drawing.Point(344, 6);
            this.cmbWeapon.Margin = new System.Windows.Forms.Padding(4);
            this.cmbWeapon.Name = "cmbWeapon";
            this.cmbWeapon.Size = new System.Drawing.Size(160, 24);
            this.cmbWeapon.TabIndex = 4;
            this.cmbWeapon.SelectedIndexChanged += new System.EventHandler(this.cmbWeapon_SelectedIndexChanged);
            // 
            // lblWeapon
            // 
            this.lblWeapon.AutoSize = true;
            this.lblWeapon.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWeapon.Location = new System.Drawing.Point(239, 7);
            this.lblWeapon.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWeapon.Name = "lblWeapon";
            this.lblWeapon.Size = new System.Drawing.Size(98, 25);
            this.lblWeapon.TabIndex = 3;
            this.lblWeapon.Text = "Weapon :";
            // 
            // lblWindValue
            // 
            this.lblWindValue.AutoSize = true;
            this.lblWindValue.Location = new System.Drawing.Point(164, 20);
            this.lblWindValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWindValue.Name = "lblWindValue";
            this.lblWindValue.Size = new System.Drawing.Size(33, 17);
            this.lblWindValue.TabIndex = 2;
            this.lblWindValue.Text = "0 W";
            // 
            // lblWind
            // 
            this.lblWind.AutoSize = true;
            this.lblWind.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWind.Location = new System.Drawing.Point(159, 2);
            this.lblWind.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWind.Name = "lblWind";
            this.lblWind.Size = new System.Drawing.Size(44, 17);
            this.lblWind.TabIndex = 1;
            this.lblWind.Text = "Wind";
            // 
            // lblPlayerName
            // 
            this.lblPlayerName.AutoSize = true;
            this.lblPlayerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPlayerName.Location = new System.Drawing.Point(16, 11);
            this.lblPlayerName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPlayerName.Name = "lblPlayerName";
            this.lblPlayerName.Size = new System.Drawing.Size(110, 20);
            this.lblPlayerName.TabIndex = 0;
            this.lblPlayerName.Text = "PlayerName";
            // 
            // timer
            // 
            this.timer.Interval = 20;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 774);
            this.Controls.Add(this.controlPanel);
            this.Controls.Add(this.displayPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "GameForm";
            this.Text = "Form1";
            this.controlPanel.ResumeLayout(false);
            this.controlPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbPower)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownAngle)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel displayPanel;
        private System.Windows.Forms.Panel controlPanel;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Label lblWeapon;
        private System.Windows.Forms.Label lblWindValue;
        private System.Windows.Forms.Label lblWind;
        private System.Windows.Forms.Label lblPlayerName;
        private System.Windows.Forms.Label lblPowerValue;
        private System.Windows.Forms.TrackBar tbPower;
        private System.Windows.Forms.Label lblPower;
        private System.Windows.Forms.NumericUpDown numUpDownAngle;
        private System.Windows.Forms.Label lblAngle;
        private System.Windows.Forms.ComboBox cmbWeapon;
        private System.Windows.Forms.Button btnFire;
    }
}

