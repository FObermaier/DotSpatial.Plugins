namespace DotSpatial.Plugins.BruTileLayer.Configuration.Forms
{
    partial class WmsConnection
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
            this.gbConnectionDetails = new System.Windows.Forms.GroupBox();
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.lblUrl = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtReferrer = new System.Windows.Forms.TextBox();
            this.lblReferrer = new System.Windows.Forms.Label();
            this.wmsConnectionErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.gbConnectionDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.wmsConnectionErrorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // gbConnectionDetails
            // 
            this.gbConnectionDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbConnectionDetails.Controls.Add(this.txtReferrer);
            this.gbConnectionDetails.Controls.Add(this.lblReferrer);
            this.gbConnectionDetails.Controls.Add(this.txtPassword);
            this.gbConnectionDetails.Controls.Add(this.lblPassword);
            this.gbConnectionDetails.Controls.Add(this.txtUsername);
            this.gbConnectionDetails.Controls.Add(this.lblUsername);
            this.gbConnectionDetails.Controls.Add(this.label1);
            this.gbConnectionDetails.Controls.Add(this.txtUrl);
            this.gbConnectionDetails.Controls.Add(this.lblUrl);
            this.gbConnectionDetails.Controls.Add(this.txtName);
            this.gbConnectionDetails.Controls.Add(this.lblName);
            this.gbConnectionDetails.Location = new System.Drawing.Point(12, 12);
            this.gbConnectionDetails.Name = "gbConnectionDetails";
            this.gbConnectionDetails.Size = new System.Drawing.Size(380, 196);
            this.gbConnectionDetails.TabIndex = 0;
            this.gbConnectionDetails.TabStop = false;
            this.gbConnectionDetails.Text = "Connection details";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(6, 27);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(35, 13);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(73, 24);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(301, 20);
            this.txtName.TabIndex = 1;
            // 
            // txtUrl
            // 
            this.txtUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUrl.Location = new System.Drawing.Point(73, 50);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(301, 20);
            this.txtUrl.TabIndex = 3;
            // 
            // lblUrl
            // 
            this.lblUrl.AutoSize = true;
            this.lblUrl.Location = new System.Drawing.Point(6, 53);
            this.lblUrl.Name = "lblUrl";
            this.lblUrl.Size = new System.Drawing.Size(20, 13);
            this.lblUrl.TabIndex = 2;
            this.lblUrl.Text = "Url";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(317, 215);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(236, 215);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 83);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(350, 33);
            this.label1.TabIndex = 5;
            this.label1.Text = "Enter username and password if the service requires basic authentification";
            // 
            // txtPassword
            // 
            this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPassword.Location = new System.Drawing.Point(73, 142);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(301, 20);
            this.txtPassword.TabIndex = 9;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(6, 145);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(53, 13);
            this.lblPassword.TabIndex = 8;
            this.lblPassword.Text = "Password";
            // 
            // txtUsername
            // 
            this.txtUsername.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUsername.Location = new System.Drawing.Point(73, 116);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(301, 20);
            this.txtUsername.TabIndex = 7;
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(6, 119);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(55, 13);
            this.lblUsername.TabIndex = 6;
            this.lblUsername.Text = "Username";
            // 
            // txtReferrer
            // 
            this.txtReferrer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtReferrer.Location = new System.Drawing.Point(73, 168);
            this.txtReferrer.Name = "txtReferrer";
            this.txtReferrer.Size = new System.Drawing.Size(301, 20);
            this.txtReferrer.TabIndex = 11;
            // 
            // lblReferrer
            // 
            this.lblReferrer.AutoSize = true;
            this.lblReferrer.Location = new System.Drawing.Point(6, 171);
            this.lblReferrer.Name = "lblReferrer";
            this.lblReferrer.Size = new System.Drawing.Size(45, 13);
            this.lblReferrer.TabIndex = 10;
            this.lblReferrer.Text = "Referrer";
            // 
            // wmsConnectionErrorProvider
            // 
            this.wmsConnectionErrorProvider.ContainerControl = this;
            // 
            // WmsConnection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 250);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.gbConnectionDetails);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(420, 288);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(420, 288);
            this.Name = "WmsConnection";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Wm(t)s Connection settings";
            this.gbConnectionDetails.ResumeLayout(false);
            this.gbConnectionDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.wmsConnectionErrorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbConnectionDetails;
        private System.Windows.Forms.TextBox txtReferrer;
        private System.Windows.Forms.Label lblReferrer;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.Label lblUrl;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ErrorProvider wmsConnectionErrorProvider;
    }
}