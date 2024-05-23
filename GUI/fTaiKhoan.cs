﻿using BUS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace GUI
{
    public partial class fTaiKhoan : Form
    {
        private static fTaiKhoan _instance;
        public static fTaiKhoan Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new fTaiKhoan();
                }
                return _instance;
            }
        }
        public fTaiKhoan()
        {
            InitializeComponent();
            LoadAllAccount();
        }
        public string type = fSignIn.accountType;
        public string username = fSignIn.userNameLogin;

        private void fTaiKhoan_Load(object sender, EventArgs e)
        {
            fHome.Instance.PictureBoxLogo.Enabled = false;

        }

        void LoadAllAccount()
        {
            if (type.ToLower() == "user")
            {
                BUSAccount.Instance.GetAccountByUsername(dataGridViewContent,username);
            }
            else
            {
                BUSAccount.Instance.GetAllAccount(dataGridViewContent);
            }

            //dataGridViewContent.DataBindingComplete += (sender, e) =>
            //{
            //    DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
            //    column.HeaderText = "STT";
            //    dataGridViewContent.Columns.Insert(0, column);

            //    for (int i = 0; i < dataGridViewContent.Rows.Count; i++)
            //    {
            //        dataGridViewContent.Rows[i].Cells[0].Value = (i + 1).ToString();
            //    }
            //    dataGridViewContent.Columns.RemoveAt(1);
            //};
            foreach (DataGridViewColumn column in dataGridViewContent.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

              
        }


        private void dataGridViewContent_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void ButtonUpdate_Click(object sender, EventArgs e)
        {
            if (type.ToLower() == "user")
            {
                MessageBox.Show("Tài Khoản Cùa Bạn Không Thể Sử Dụng Chức Năng Này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else
            {
                string usernameTK = textBoxUsername.Text;
                string accountType = comboBoxLoaiTk.Text;
                if (comboBoxLoaiTk.SelectedIndex != -1)
                {
                    accountType = comboBoxLoaiTk.SelectedItem.ToString();
                }

                if (string.IsNullOrEmpty(usernameTK))
                {
                    MessageBox.Show("Vui Lòng Nhập Tài Khoản Cần Cấp Quyền.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if(username.ToLower() != "admin")
                    {
                        MessageBox.Show("Tài Khoản Cùa Bạn Không Thể Sử Dụng Chức Năng Này???.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    }
                    string message = BUSAccount.Instance.GrantPermission(usernameTK, accountType);
                    if (message == "")
                    {
                        MessageBox.Show($"Cập Quyền Cho Tài Khoản: {usernameTK} Với Quyền: {accountType} Thành Công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadAllAccount();
                    }
                    else
                    {
                        MessageBox.Show($"Thất Bại. {message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    }
                }
            }
        }

        private void ButtonDel_Click(object sender, EventArgs e)
        {
            if (type.ToLower() == "user")
            {
                MessageBox.Show("Tài Khoản Cùa Bạn Không Thể Sử Dụng Chức Năng Này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else
            {
                DialogResult result = MessageBox.Show($"Bạn Có Chắc Chắn Muốn Xóa Username: {textBoxUsername.Text}", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    string message = BUSAccount.Instance.DeleteAccount(textBoxUsername.Text);
                    if (message == "")
                    {
                        MessageBox.Show($"Xóa Thành Công Username: {textBoxUsername.Text}", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadAllAccount();
                    }
                    else
                    {
                        MessageBox.Show($"Thất Bại. {message}", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    }
                }
            }
        }


        private Button tempButton;
        private void dataGridViewContent_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Lấy vị trí của ô được click
                Rectangle cellRect = dataGridViewContent.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);

                // Tạo Button tạm thời
                tempButton = new Button();
                tempButton.Text = "Button";
                tempButton.Size = new Size(70, 20);
                tempButton.Location = new Point(cellRect.X + (cellRect.Width - tempButton.Width) / 2, cellRect.Y + (cellRect.Height - tempButton.Height) / 2);
                tempButton.Click += TempButton_Click;

                // Thêm Button vào Form
                Controls.Add(tempButton);
                tempButton.BringToFront();
            }
        }
        private void TempButton_Click(object sender, EventArgs e)
        {
            // Xử lý sự kiện khi Button tạm thời được click
            MessageBox.Show("Right-clicked Button!");
        }

        private void dataGridViewContent_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex != -1)
            {
                textBoxUsername.Text = dataGridViewContent.Rows[e.RowIndex].Cells[1].Value.ToString();
                textBoxSDT.Text = dataGridViewContent.Rows[e.RowIndex].Cells[2].Value.ToString();
                textBoxEmail.Text = dataGridViewContent.Rows[e.RowIndex].Cells[3].Value.ToString();
                comboBoxLoaiTk.Text = dataGridViewContent.Rows[e.RowIndex].Cells[4].Value.ToString();
            }
        }
        Color color;

        private void dataGridViewContent_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Đổi màu chữ của hàng khi di chuột ra
                dataGridViewContent.Rows[e.RowIndex].DefaultCellStyle.BackColor = color;
            }
        }

        private void dataGridViewContent_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
         
            if (e.RowIndex >= 0)
            {
                color = dataGridViewContent.Rows[e.RowIndex].DefaultCellStyle.BackColor;
                dataGridViewContent.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightSkyBlue;

            }
        }
    }
}
