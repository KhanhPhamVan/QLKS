using QLKS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace QLKS.Forms
{
    public partial class FormCustomer : Form
    {
        static DbContext db = new DbContext(DbContext.ConnectionType.ConfigurationManager, "DefaultConnection");
        public FormCustomer()
        {
            InitializeComponent();
        }

        private void FormCustomer_Load(object sender, EventArgs e)
        {
            cboTypeSearch.SelectedIndex = 0;
            cboCountry.DataSource = Helpers.Countries;
            cboGender.DataSource = new List<string> { "Nam", "Nữ" };
            dtgvCustomer.DataSource = db.GetTable<Customer>();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                MessageBox.Show("Vui lòng nhập vào thông tin để tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Customer customer=new Customer();
            if (cboTypeSearch.SelectedIndex == 0) 
                customer = db.GetTable<Customer>(t => t.UniqueNumber == txtSearch.Text).FirstOrDefault();
            else
                customer = db.GetTable<Customer>(t => t.Phone == txtSearch.Text).FirstOrDefault();
            if (customer == null)
            {
                MessageBox.Show("Không tìm thấy khách hàng này", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            txtCustomerIdShow.Text = customer.UniqueNumber.ToString();
            txtCustomerName.Text = customer.Name.ToString();
            dtpDoB.Text = customer.DoB.ToString();
            txtPhoneNumber.Text = customer.Phone.ToString();
            cboGender.Text = customer.Gender.ToString();
            cboCountry.Text = customer.Country.ToString();
        }
        string ErrorMessage()
        {
            if (string.IsNullOrEmpty(txtCustomerName.Text))
                return "Vui lòng nhập vào tên khách hàng";
            if (string.IsNullOrEmpty(txtCustomerIdShow.Text))
                return "Vui lòng nhập vào mã định danh khách hàng";
            if (string.IsNullOrEmpty(txtPhoneNumber.Text))
                return "Vui lòng nhập vào số điện thoại khách hàng";
            return null;
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string error = ErrorMessage();
            if (error != null)
            {
                MessageBox.Show(error, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Customer customer = new Customer();
            customer.Name = txtCustomerName.Text;
            customer.DoB=dtpDoB.Value;
            customer.Gender=cboGender.Text;
            customer.Country=cboCountry.Text;
            customer.Phone=txtPhoneNumber.Text;
            customer.UniqueNumber=txtCustomerIdShow.Text;
            db.AddRow(customer);
            dtgvCustomer.DataSource = db.GetTable<Customer>();
        }

        private void dtgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex>=0)
            {
                DataGridViewRow row=dtgvCustomer.Rows[e.RowIndex];
                txtCustomerName.Text = row.Cells["NameCustomer"].Value?.ToString();
                txtPhoneNumber.Text = row.Cells["Phone"].Value?.ToString();
                DateTime date = DateTime.ParseExact(row.Cells["DoB"].Value?.ToString(), "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                dtpDoB.Value = date.Date;
                cboCountry.Text = row.Cells["Country"].Value?.ToString();
                cboGender.Text = row.Cells["Gender"].Value?.ToString();
                txtCustomerIdShow.Text = row.Cells["UniqueNumber"].Value?.ToString();
            }    
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string error = ErrorMessage();
            if (error != null)
            {
                MessageBox.Show(error, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Customer customer = new Customer();
            customer.Name = txtCustomerName.Text;
            customer.DoB = dtpDoB.Value;
            customer.Gender = cboGender.Text;
            customer.Country = cboCountry.Text;
            customer.Phone = txtPhoneNumber.Text;
            customer.UniqueNumber = txtCustomerIdShow.Text;
            customer.Id = db.GetTable<Customer>(t => t.UniqueNumber ==customer.UniqueNumber).First().Id;
            db.UpdateRow(customer);
            dtgvCustomer.DataSource = db.GetTable<Customer>();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string error = ErrorMessage();
            if (error != null)
            {
                MessageBox.Show(error, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Bạn có chắc muốn xóa khách hàng này?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            Customer customer = new Customer();
            customer.Name = txtCustomerName.Text;
            customer.DoB = dtpDoB.Value;
            customer.Gender = cboGender.Text;
            customer.Country = cboCountry.Text;
            customer.Phone = txtPhoneNumber.Text;
            customer.UniqueNumber = txtCustomerIdShow.Text;
            customer.Id = db.GetTable<Customer>(t => t.UniqueNumber == customer.UniqueNumber).First().Id;
            Func<Customer, bool> predicate =p=>p.Id == customer.Id;
            db.DeleteRows(predicate);
            dtgvCustomer.DataSource = db.GetTable<Customer>();

        }
        void ClearControl(Control control)
        {
            foreach (Control control1 in control.Controls)
            {
                if (control1 is TextBox)
                {
                    TextBox textBox = (TextBox)control1;
                    textBox.Clear();
                }
                else if (control1 is ComboBox)
                {
                    ComboBox comboBox = (ComboBox)control1;
                    comboBox.SelectedIndex = 0;
                }
                else if (control1 is DateTimePicker)
                {
                    DateTimePicker dateTimePicker = (DateTimePicker)control1;
                    dateTimePicker.Value=DateTime.Now;
                }
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            ClearControl(grbCustomer);
        }
    }
}
