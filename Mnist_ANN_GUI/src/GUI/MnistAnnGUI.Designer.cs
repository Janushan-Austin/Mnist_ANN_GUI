namespace Mnist_ANN_GUI
{
    partial class MnistAnnGUI
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
            this.MnistPictureBox = new System.Windows.Forms.PictureBox();
            this.MnistImageLabel = new System.Windows.Forms.Label();
            this.TrainButton = new System.Windows.Forms.Button();
            this.TestImageButton = new System.Windows.Forms.Button();
            this.ImageSelectionTextBox = new System.Windows.Forms.TextBox();
            this.ImageSelectionlabel = new System.Windows.Forms.Label();
            this.UseTrainingSetCheckBox = new System.Windows.Forms.CheckBox();
            this.InvalidImageLabel = new System.Windows.Forms.Label();
            this.PredictionLabel = new System.Windows.Forms.Label();
            this.TrainingProgressLabel = new System.Windows.Forms.Label();
            this.TrainingProgressBar = new System.Windows.Forms.ProgressBar();
            this.TrainingSensLabel = new System.Windows.Forms.Label();
            this.TestingSensLabel = new System.Windows.Forms.Label();
            this.TotalEpochLabel = new System.Windows.Forms.Label();
            this.NumEpochsComboBox = new System.Windows.Forms.ComboBox();
            this.NumEpochsLabel = new System.Windows.Forms.Label();
            this.CancelNetworkButton = new System.Windows.Forms.Button();
            this.ResetNetworkButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.MnistPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // MnistPictureBox
            // 
            this.MnistPictureBox.BackColor = System.Drawing.Color.Black;
            this.MnistPictureBox.Location = new System.Drawing.Point(189, 66);
            this.MnistPictureBox.Name = "MnistPictureBox";
            this.MnistPictureBox.Size = new System.Drawing.Size(28, 28);
            this.MnistPictureBox.TabIndex = 0;
            this.MnistPictureBox.TabStop = false;
            // 
            // MnistImageLabel
            // 
            this.MnistImageLabel.AutoSize = true;
            this.MnistImageLabel.Location = new System.Drawing.Point(223, 81);
            this.MnistImageLabel.Name = "MnistImageLabel";
            this.MnistImageLabel.Size = new System.Drawing.Size(77, 13);
            this.MnistImageLabel.TabIndex = 1;
            this.MnistImageLabel.Text = "Image Label: 1";
            // 
            // TrainButton
            // 
            this.TrainButton.Location = new System.Drawing.Point(12, 159);
            this.TrainButton.Name = "TrainButton";
            this.TrainButton.Size = new System.Drawing.Size(122, 45);
            this.TrainButton.TabIndex = 2;
            this.TrainButton.Text = "Train 1 Epoch";
            this.TrainButton.UseVisualStyleBackColor = true;
            this.TrainButton.Click += new System.EventHandler(this.TrainButton_Click);
            // 
            // TestImageButton
            // 
            this.TestImageButton.Location = new System.Drawing.Point(189, 113);
            this.TestImageButton.Name = "TestImageButton";
            this.TestImageButton.Size = new System.Drawing.Size(122, 23);
            this.TestImageButton.TabIndex = 3;
            this.TestImageButton.Text = "Test on Image";
            this.TestImageButton.UseVisualStyleBackColor = true;
            this.TestImageButton.Click += new System.EventHandler(this.TestImageButton_Click);
            // 
            // ImageSelectionTextBox
            // 
            this.ImageSelectionTextBox.Location = new System.Drawing.Point(189, 40);
            this.ImageSelectionTextBox.Name = "ImageSelectionTextBox";
            this.ImageSelectionTextBox.Size = new System.Drawing.Size(100, 20);
            this.ImageSelectionTextBox.TabIndex = 4;
            this.ImageSelectionTextBox.TextChanged += new System.EventHandler(this.ImageSelectionTextBox_TextChanged);
            // 
            // ImageSelectionlabel
            // 
            this.ImageSelectionlabel.AutoSize = true;
            this.ImageSelectionlabel.Location = new System.Drawing.Point(186, 24);
            this.ImageSelectionlabel.Name = "ImageSelectionlabel";
            this.ImageSelectionlabel.Size = new System.Drawing.Size(148, 13);
            this.ImageSelectionlabel.TabIndex = 5;
            this.ImageSelectionlabel.Text = "Choose image to test (0-9999)";
            // 
            // UseTrainingSetCheckBox
            // 
            this.UseTrainingSetCheckBox.AutoSize = true;
            this.UseTrainingSetCheckBox.Location = new System.Drawing.Point(189, 4);
            this.UseTrainingSetCheckBox.Name = "UseTrainingSetCheckBox";
            this.UseTrainingSetCheckBox.Size = new System.Drawing.Size(164, 17);
            this.UseTrainingSetCheckBox.TabIndex = 6;
            this.UseTrainingSetCheckBox.Text = "Use images from training set?";
            this.UseTrainingSetCheckBox.UseVisualStyleBackColor = true;
            this.UseTrainingSetCheckBox.CheckedChanged += new System.EventHandler(this.UseTrainingSetCheckBox_CheckedChanged);
            // 
            // InvalidImageLabel
            // 
            this.InvalidImageLabel.AutoSize = true;
            this.InvalidImageLabel.ForeColor = System.Drawing.Color.Red;
            this.InvalidImageLabel.Location = new System.Drawing.Point(295, 43);
            this.InvalidImageLabel.Name = "InvalidImageLabel";
            this.InvalidImageLabel.Size = new System.Drawing.Size(38, 13);
            this.InvalidImageLabel.TabIndex = 7;
            this.InvalidImageLabel.Text = "Invalid";
            this.InvalidImageLabel.Visible = false;
            // 
            // PredictionLabel
            // 
            this.PredictionLabel.AutoSize = true;
            this.PredictionLabel.Location = new System.Drawing.Point(185, 97);
            this.PredictionLabel.Name = "PredictionLabel";
            this.PredictionLabel.Size = new System.Drawing.Size(152, 13);
            this.PredictionLabel.TabIndex = 8;
            this.PredictionLabel.Text = "Network Label Prediction: N/A";
            // 
            // TrainingProgressLabel
            // 
            this.TrainingProgressLabel.AutoSize = true;
            this.TrainingProgressLabel.Location = new System.Drawing.Point(13, 48);
            this.TrainingProgressLabel.Name = "TrainingProgressLabel";
            this.TrainingProgressLabel.Size = new System.Drawing.Size(111, 13);
            this.TrainingProgressLabel.TabIndex = 9;
            this.TrainingProgressLabel.Text = "Network Progress: 0%";
            // 
            // TrainingProgressBar
            // 
            this.TrainingProgressBar.Location = new System.Drawing.Point(13, 64);
            this.TrainingProgressBar.Name = "TrainingProgressBar";
            this.TrainingProgressBar.Size = new System.Drawing.Size(122, 23);
            this.TrainingProgressBar.TabIndex = 10;
            // 
            // TrainingSensLabel
            // 
            this.TrainingSensLabel.AutoSize = true;
            this.TrainingSensLabel.Location = new System.Drawing.Point(13, 94);
            this.TrainingSensLabel.Name = "TrainingSensLabel";
            this.TrainingSensLabel.Size = new System.Drawing.Size(121, 13);
            this.TrainingSensLabel.TabIndex = 12;
            this.TrainingSensLabel.Text = "Training Sensitivity: N/A";
            // 
            // TestingSensLabel
            // 
            this.TestingSensLabel.AutoSize = true;
            this.TestingSensLabel.Location = new System.Drawing.Point(13, 121);
            this.TestingSensLabel.Name = "TestingSensLabel";
            this.TestingSensLabel.Size = new System.Drawing.Size(118, 13);
            this.TestingSensLabel.TabIndex = 13;
            this.TestingSensLabel.Text = "Testing Sensitivity: N/A";
            // 
            // TotalEpochLabel
            // 
            this.TotalEpochLabel.AutoSize = true;
            this.TotalEpochLabel.Location = new System.Drawing.Point(14, 143);
            this.TotalEpochLabel.Name = "TotalEpochLabel";
            this.TotalEpochLabel.Size = new System.Drawing.Size(121, 13);
            this.TotalEpochLabel.TabIndex = 14;
            this.TotalEpochLabel.Text = "Total Trained Epochs: 0";
            // 
            // NumEpochsComboBox
            // 
            this.NumEpochsComboBox.FormattingEnabled = true;
            this.NumEpochsComboBox.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "10"});
            this.NumEpochsComboBox.Location = new System.Drawing.Point(17, 24);
            this.NumEpochsComboBox.Name = "NumEpochsComboBox";
            this.NumEpochsComboBox.Size = new System.Drawing.Size(121, 21);
            this.NumEpochsComboBox.TabIndex = 15;
            this.NumEpochsComboBox.SelectedIndexChanged += new System.EventHandler(this.NumEpochsComboBox_SelectedIndexChanged);
            // 
            // NumEpochsLabel
            // 
            this.NumEpochsLabel.AutoSize = true;
            this.NumEpochsLabel.Location = new System.Drawing.Point(16, 5);
            this.NumEpochsLabel.Name = "NumEpochsLabel";
            this.NumEpochsLabel.Size = new System.Drawing.Size(150, 13);
            this.NumEpochsLabel.TabIndex = 16;
            this.NumEpochsLabel.Text = "Number of Epochs per training";
            // 
            // CancelNetworkButton
            // 
            this.CancelNetworkButton.Location = new System.Drawing.Point(13, 211);
            this.CancelNetworkButton.Name = "CancelNetworkButton";
            this.CancelNetworkButton.Size = new System.Drawing.Size(121, 23);
            this.CancelNetworkButton.TabIndex = 17;
            this.CancelNetworkButton.Text = "Cancel";
            this.CancelNetworkButton.UseVisualStyleBackColor = true;
            this.CancelNetworkButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // ResetNetworkButton
            // 
            this.ResetNetworkButton.Location = new System.Drawing.Point(188, 211);
            this.ResetNetworkButton.Name = "ResetNetworkButton";
            this.ResetNetworkButton.Size = new System.Drawing.Size(123, 23);
            this.ResetNetworkButton.TabIndex = 18;
            this.ResetNetworkButton.Text = "Reset Network";
            this.ResetNetworkButton.UseVisualStyleBackColor = true;
            this.ResetNetworkButton.Click += new System.EventHandler(this.ResetNetworkButton_Click);
            // 
            // MnistAnnGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 255);
            this.Controls.Add(this.ResetNetworkButton);
            this.Controls.Add(this.CancelNetworkButton);
            this.Controls.Add(this.NumEpochsLabel);
            this.Controls.Add(this.NumEpochsComboBox);
            this.Controls.Add(this.TotalEpochLabel);
            this.Controls.Add(this.TestingSensLabel);
            this.Controls.Add(this.TrainingSensLabel);
            this.Controls.Add(this.TrainingProgressBar);
            this.Controls.Add(this.TrainingProgressLabel);
            this.Controls.Add(this.PredictionLabel);
            this.Controls.Add(this.InvalidImageLabel);
            this.Controls.Add(this.UseTrainingSetCheckBox);
            this.Controls.Add(this.ImageSelectionlabel);
            this.Controls.Add(this.ImageSelectionTextBox);
            this.Controls.Add(this.TestImageButton);
            this.Controls.Add(this.TrainButton);
            this.Controls.Add(this.MnistImageLabel);
            this.Controls.Add(this.MnistPictureBox);
            this.Name = "MnistAnnGUI";
            this.Text = "MNIST ANN";
            ((System.ComponentModel.ISupportInitialize)(this.MnistPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox MnistPictureBox;
        private System.Windows.Forms.Label MnistImageLabel;
        private System.Windows.Forms.Button TrainButton;
        private System.Windows.Forms.Button TestImageButton;
        private System.Windows.Forms.TextBox ImageSelectionTextBox;
        private System.Windows.Forms.Label ImageSelectionlabel;
        private System.Windows.Forms.CheckBox UseTrainingSetCheckBox;
        private System.Windows.Forms.Label InvalidImageLabel;
        private System.Windows.Forms.Label PredictionLabel;
        private System.Windows.Forms.Label TrainingProgressLabel;
        private System.Windows.Forms.ProgressBar TrainingProgressBar;
        private System.Windows.Forms.Label TrainingSensLabel;
        private System.Windows.Forms.Label TestingSensLabel;
        private System.Windows.Forms.Label TotalEpochLabel;
        private System.Windows.Forms.ComboBox NumEpochsComboBox;
        private System.Windows.Forms.Label NumEpochsLabel;
        private System.Windows.Forms.Button CancelNetworkButton;
        private System.Windows.Forms.Button ResetNetworkButton;
    }
}

