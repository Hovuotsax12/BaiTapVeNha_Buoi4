using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NguyenHoangVuong__5951071124
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GetStudentsRecord();
        }
        SqlConnection con = new SqlConnection(@"Data Source=LAPTOP-DQ9LSA83\SQLEXPRESS;Initial Catalog=DemoCRUD;Integrated Security=True");
        private void GetStudentsRecord()
        {
            SqlCommand cmd = new SqlCommand("select * from StudentsTb",con);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();
            dgvSinhvien.DataSource = dt;
        }
        private bool IsValidData()
        {
            if(txthosinhvien.Text == string.Empty || txttensinhvien.Text == string.Empty 
                || txtdiachi.Text == string.Empty || string.IsNullOrEmpty(txtsdt.Text)
                || string.IsNullOrEmpty(txtsbd.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (IsValidData())
            {
                SqlCommand cmd = new SqlCommand("insert into StudentsTb values (@Name,@FatherName,@RollNumber,@Address,@Mobile)", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Name", txthosinhvien.Text);
                cmd.Parameters.AddWithValue("@FatherName", txttensinhvien.Text);
                cmd.Parameters.AddWithValue("@RollNumber", txtsbd.Text);
                cmd.Parameters.AddWithValue("@Address", txtdiachi.Text);
                cmd.Parameters.AddWithValue("@Mobile", txtsdt.Text);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                GetStudentsRecord();
            }
        }
        public int StudentID;
        private void dgvSinhvien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = new DataGridViewRow();
            row = dgvSinhvien.Rows[e.RowIndex];
            StudentID = Convert.ToInt32(row.Cells[0].Value);
            txthosinhvien.Text = row.Cells[1].Value.ToString();
            txttensinhvien.Text = row.Cells[2].Value.ToString();
            txtsbd.Text = row.Cells[3].Value.ToString();
            txtdiachi.Text = row.Cells[4].Value.ToString();
            txtsdt.Text = row.Cells[5].Value.ToString();
        }

        private void btnCapnhat_Click(object sender, EventArgs e)
        {
            if(StudentID > 0)
            {
                SqlCommand cmd = new SqlCommand("update StudentsTb set Name = @Name,FatherName = @FatherName,RollNumber = @RollNumber,Address = @Address,Mobile=@Mobile where StudentID = @ID",con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Name", txthosinhvien.Text);
                cmd.Parameters.AddWithValue("@FatherName", txttensinhvien.Text);
                cmd.Parameters.AddWithValue("@RollNumber", txtsbd.Text);
                cmd.Parameters.AddWithValue("@Address", txtdiachi.Text);
                cmd.Parameters.AddWithValue("@Mobile", txtsdt.Text);
                cmd.Parameters.AddWithValue("@ID", this.StudentID);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                GetStudentsRecord();
                ResetData();
            }
            else
            {
                MessageBox.Show("Cập nhật bị lỗi", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetData()
        {
            txthosinhvien.Text = txttensinhvien.Text = txtsdt.Text = txtsbd.Text = txtdiachi.Text = "";
            StudentID = 0;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if(StudentID > 0)
            {
                if (MessageBox.Show("Bạn có chắc chắn muốn xóa StudentID " + StudentID + "?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SqlCommand cmd = new SqlCommand("delete from StudentsTb where StudentID = @ID", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@ID", this.StudentID);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    GetStudentsRecord();
                    ResetData();
                }
                else
                {
                    MessageBox.Show("Xóa thất bại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn student cần xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
           
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn thoát ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Dispose();
            }
        }

        private void btnXacLap_Click(object sender, EventArgs e)
        {
            ResetData();
        }

        private void txtsdt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }
    }
}
