using QLKS.Models;
using QLKS.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLKS.Forms
{
    public partial class FormRoom : Form
    {
        static DbContext db = new DbContext(DbContext.ConnectionType.ConfigurationManager, "DefaultConnection");
        public FormRoom()
        {
            InitializeComponent();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            FormRoomType frmRoomType=new FormRoomType();
            frmRoomType.ShowDialog();
        }
        IEnumerable<RoomViewModel> View = RoomViewModel.GetRooms(db);
        

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(lblMaxPeople.Text))
            {
                MessageBox.Show("Vui lòng nhập vào thông tin để tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            RoomViewModel room = null;
            foreach (RoomViewModel view in View)
            {
                if(view.Number==txtSearch.Text)
                {
                    room = view;
                    break;
                }    
            }
            if (room == null)
            {
                MessageBox.Show("Không tìm thấy phòng này", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            txtNumber.Text = room.Number;
            cboStatus.Text = room.Status;
            cboTypeId.Text = room.TypeId.ToString();
            txtTypeName.Text = room.Type;
            txtMaxPeople.Text=room.MaxPeople.ToString();
            txtPrice.Text = room.Price.ToString();
        }

        private void FormRoom_Load(object sender, EventArgs e)
        {
            LoadDataSource();
        }
        void LoadDataSource()
        {
            List<RoomViewModel> list = new List<RoomViewModel>();
            foreach (RoomViewModel view in View)
            {
                list.Add(view);
            }
            dtgvRoom.DataSource = list;
        }

        private void dtgvRoom_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex>=0)
            {
                DataGridViewRow row=dtgvRoom.Rows[e.RowIndex];
                txtNumber.Text = row.Cells["Number"].Value?.ToString();
                cboStatus.Text = row.Cells["Status"].Value?.ToString();
                cboTypeId.Text = row.Cells["TypeId"].Value?.ToString();
                txtTypeName.Text = row.Cells["Type"].Value?.ToString(); ;
                txtMaxPeople.Text = row.Cells["MaxPeople"].Value?.ToString(); ;
                txtPrice.Text = row.Cells["Price"].Value?.ToString();
            }    
        }
        string ErrorMessage()
        {
            if (string.IsNullOrEmpty(txtNumber.Text))
                return "Vui lòng nhập vào số phòng";
            if (string.IsNullOrEmpty(cboTypeId.Text))
                return "Vui lòng nhập vào mã loại phòng";
            if (string.IsNullOrEmpty(txtTypeName.Text))
                return "Vui lòng nhập vào tên loại phòng";
            if (string.IsNullOrEmpty(txtPrice.Text))
                return "Vui lòng nhập vào giá phòng";
            if (string.IsNullOrEmpty(cboStatus.Text))
                return "Vui lòng nhập vào trạng thái phòng";
            if (string.IsNullOrEmpty(txtMaxPeople.Text))
                return "Vui lòng nhập vào số lượng người tối đa";
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
            Room room = new Room();
            room.Name=txtNumber.Text;
            room.Status=cboStatus.Text;
            room.RoomType =int.Parse(cboTypeId.Text);
            db.AddRow(room);
            LoadDataSource();
        }
    }
}
