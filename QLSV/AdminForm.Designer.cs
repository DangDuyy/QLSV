namespace QLSV
{
    partial class AdminForm
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
            this.searchButton = new System.Windows.Forms.Button();
            this.searchtxt = new System.Windows.Forms.TextBox();
            this.AllRbtn = new System.Windows.Forms.Button();
            this.AllAbtn = new System.Windows.Forms.Button();
            this.ButtonRefresh = new System.Windows.Forms.Button();
            this.RejectButton = new System.Windows.Forms.Button();
            this.ApproveButton = new System.Windows.Forms.Button();
            this.TotalStudentlbl = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ChoiceBox = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.unAprrovestdBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unAprrovestdBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // searchButton
            // 
            this.searchButton.BackColor = System.Drawing.Color.Silver;
            this.searchButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.searchButton.Location = new System.Drawing.Point(560, 278);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(106, 31);
            this.searchButton.TabIndex = 52;
            this.searchButton.Text = "Search";
            this.searchButton.UseVisualStyleBackColor = false;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // searchtxt
            // 
            this.searchtxt.Location = new System.Drawing.Point(296, 280);
            this.searchtxt.Name = "searchtxt";
            this.searchtxt.Size = new System.Drawing.Size(258, 22);
            this.searchtxt.TabIndex = 50;
            // 
            // AllRbtn
            // 
            this.AllRbtn.AccessibleName = "";
            this.AllRbtn.BackColor = System.Drawing.Color.Tomato;
            this.AllRbtn.Location = new System.Drawing.Point(9, 342);
            this.AllRbtn.Name = "AllRbtn";
            this.AllRbtn.Size = new System.Drawing.Size(104, 26);
            this.AllRbtn.TabIndex = 49;
            this.AllRbtn.Text = "Reject All";
            this.AllRbtn.UseVisualStyleBackColor = false;
            this.AllRbtn.Click += new System.EventHandler(this.AllRbtn_Click);
            // 
            // AllAbtn
            // 
            this.AllAbtn.AccessibleName = "";
            this.AllAbtn.BackColor = System.Drawing.Color.LimeGreen;
            this.AllAbtn.Location = new System.Drawing.Point(9, 310);
            this.AllAbtn.Name = "AllAbtn";
            this.AllAbtn.Size = new System.Drawing.Size(104, 26);
            this.AllAbtn.TabIndex = 48;
            this.AllAbtn.Text = "Approve All";
            this.AllAbtn.UseVisualStyleBackColor = false;
            this.AllAbtn.Click += new System.EventHandler(this.AllAbtn_Click);
            // 
            // ButtonRefresh
            // 
            this.ButtonRefresh.AccessibleName = "";
            this.ButtonRefresh.BackColor = System.Drawing.Color.LightBlue;
            this.ButtonRefresh.Location = new System.Drawing.Point(9, 278);
            this.ButtonRefresh.Name = "ButtonRefresh";
            this.ButtonRefresh.Size = new System.Drawing.Size(104, 26);
            this.ButtonRefresh.TabIndex = 47;
            this.ButtonRefresh.Text = "Refresh";
            this.ButtonRefresh.UseVisualStyleBackColor = false;
            this.ButtonRefresh.Click += new System.EventHandler(this.ButtonRefresh_Click);
            // 
            // RejectButton
            // 
            this.RejectButton.BackColor = System.Drawing.Color.Red;
            this.RejectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.RejectButton.Location = new System.Drawing.Point(311, 317);
            this.RejectButton.Name = "RejectButton";
            this.RejectButton.Size = new System.Drawing.Size(166, 38);
            this.RejectButton.TabIndex = 46;
            this.RejectButton.Text = "Reject";
            this.RejectButton.UseVisualStyleBackColor = false;
            this.RejectButton.Click += new System.EventHandler(this.RejectButton_Click);
            // 
            // ApproveButton
            // 
            this.ApproveButton.BackColor = System.Drawing.Color.MediumSpringGreen;
            this.ApproveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.ApproveButton.Location = new System.Drawing.Point(139, 317);
            this.ApproveButton.Name = "ApproveButton";
            this.ApproveButton.Size = new System.Drawing.Size(166, 38);
            this.ApproveButton.TabIndex = 45;
            this.ApproveButton.Text = "Approve";
            this.ApproveButton.UseVisualStyleBackColor = false;
            this.ApproveButton.Click += new System.EventHandler(this.ApproveButton_Click);
            // 
            // TotalStudentlbl
            // 
            this.TotalStudentlbl.AutoSize = true;
            this.TotalStudentlbl.BackColor = System.Drawing.Color.Gainsboro;
            this.TotalStudentlbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.TotalStudentlbl.Location = new System.Drawing.Point(601, 13);
            this.TotalStudentlbl.Name = "TotalStudentlbl";
            this.TotalStudentlbl.Size = new System.Drawing.Size(125, 20);
            this.TotalStudentlbl.TabIndex = 36;
            this.TotalStudentlbl.Text = "Total Account";
            this.TotalStudentlbl.Click += new System.EventHandler(this.TotalStudentlbl_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 10.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label2.Location = new System.Drawing.Point(11, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(330, 25);
            this.label2.TabIndex = 6;
            this.label2.Text = "Danh Sách Tài Khoản Chưa Được Duyệt";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.MediumSpringGreen;
            this.panel1.Controls.Add(this.TotalStudentlbl);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(12, 8);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(813, 48);
            this.panel1.TabIndex = 44;
            // 
            // ChoiceBox
            // 
            this.ChoiceBox.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.ChoiceBox.FormattingEnabled = true;
            this.ChoiceBox.Items.AddRange(new object[] {
            "MSSV",
            "FName",
            "LName",
            "Address",
            "Phone",
            "Email"});
            this.ChoiceBox.Location = new System.Drawing.Point(139, 280);
            this.ChoiceBox.Name = "ChoiceBox";
            this.ChoiceBox.Size = new System.Drawing.Size(151, 31);
            this.ChoiceBox.TabIndex = 51;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 62);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(813, 212);
            this.dataGridView1.TabIndex = 43;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentDoubleClick);
            // 
            // unAprrovestdBindingSource
            // 
            this.unAprrovestdBindingSource.DataMember = "UnAprrovestd";
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToOrderColumns = true;
            this.dataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(12, 428);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.ReadOnly = true;
            this.dataGridView2.RowHeadersWidth = 51;
            this.dataGridView2.RowTemplate.Height = 50;
            this.dataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.Size = new System.Drawing.Size(816, 242);
            this.dataGridView2.TabIndex = 53;
            this.dataGridView2.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellContentClick);
            this.dataGridView2.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellContentClick);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.MediumSpringGreen;
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Location = new System.Drawing.Point(12, 374);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(813, 48);
            this.panel2.TabIndex = 54;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Gainsboro;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label1.Location = new System.Drawing.Point(601, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 20);
            this.label1.TabIndex = 36;
            this.label1.Text = "Total Account";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 10.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label3.Location = new System.Drawing.Point(11, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(263, 25);
            this.label3.TabIndex = 6;
            this.label3.Text = "Danh Sách Tài Khoản Đã Duyệt";
            // 
            // AdminForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 715);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.searchtxt);
            this.Controls.Add(this.AllRbtn);
            this.Controls.Add(this.AllAbtn);
            this.Controls.Add(this.ButtonRefresh);
            this.Controls.Add(this.RejectButton);
            this.Controls.Add(this.ApproveButton);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ChoiceBox);
            this.Controls.Add(this.dataGridView1);
            this.Name = "AdminForm";
            this.Text = "AdminForm";
            this.Load += new System.EventHandler(this.AdminApproveStudent_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unAprrovestdBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.TextBox searchtxt;
        private System.Windows.Forms.Button AllRbtn;
        private System.Windows.Forms.Button AllAbtn;
        private System.Windows.Forms.Button ButtonRefresh;
        private System.Windows.Forms.Button RejectButton;
        private System.Windows.Forms.Button ApproveButton;
        private System.Windows.Forms.Label TotalStudentlbl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.BindingSource unAprrovestdBindingSource;
        private System.Windows.Forms.ComboBox ChoiceBox;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
    }
}