using QLKS.Forms;
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

namespace QLKS
{
    
    public partial class FormReservation : Form
    {
        public class ListRoomBooked
        {
            public string RoomNumber { get; set; }
            public string ArrivedDate { get; set; }
            public string ExpectedDate { get; set; }
            public ListRoomBooked(string roomnumber, string arriveddate, string expecteddate)
            {
                RoomNumber = roomnumber;
                ArrivedDate = arriveddate;
                ExpectedDate = expecteddate;
            }
        }

        static DbContext db = new DbContext(DbContext.ConnectionType.ConfigurationManager, "DefaultConnection");
        public FormReservation()
        {
            InitializeComponent();
        }
        IEnumerable<ReservationRoomStatus> viewModel = ReservationRoomStatus.GetRooms(db);
        public void LoadingList()
        {
            
            List<string> roomBooked=new List<string>();
            List<string> roomEmpty = new List<string>();
            foreach (ReservationRoomStatus room in viewModel)
            {
                if(room.Status=="Đã đặt")
                {
                    roomBooked.Add(room.Number);
                }   
                else if (room.Status == "Phòng trống")
                    roomEmpty.Add(room.Number);
            }
            foreach (string room in roomBooked)
            {
                lsvRoomBooked.Items.Add(room);
            }
            foreach (string room in roomEmpty)
            {
                lsvRoomEmpty.Items.Add(room);
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            FormChangeRoom form = new FormChangeRoom();
            form.ShowDialog();
        }

        private void FormReservation_Load(object sender, EventArgs e)
        {
            LoadingList();
            cboCountry.DataSource = Helpers.Countries;
            cboGender.DataSource=new List<string>{"Nam","Nữ" };
        }

        private void lsvRoomEmpty_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            ListView listView = (ListView)sender;
            if (listView.SelectedItems.Count > 0)
            {
                foreach (ReservationRoomStatus room in viewModel)
                {
                    if (room.Number == listView.SelectedItems[0].Text)
                    {
                        txtRoomTypeName.Text = room.NameRoomType;
                        txtPrice.Text = room.Price.ToString();
                        txtMaxPeople.Text = room.MaxPeople.ToString();
                    }
                }
            }
        }

        private void btnSearchCustomer_Click(object sender, EventArgs e)
        {
            Customer customer = db.GetTable<Customer>(t => t.UniqueNumber == txtCustomerId.Text).FirstOrDefault();
            if(customer==null)
            {
                MessageBox.Show("Không tìm thấy khách hàng này", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            } 
            txtCustomerIdShow.Text=customer.UniqueNumber.ToString();
            txtCustomerName.Text=customer.Name.ToString();
            dtpDoB.Text=customer.DoB.ToString();
            txtPhoneNumber.Text=customer.Phone.ToString();
            cboGender.Text=customer.Gender.ToString();
            cboCountry.Text=customer.Country.ToString();
        }
        string ErrorMessage()
        {
            if (string.IsNullOrEmpty(txtRoomNumber.Text))
                return "Vui lòng nhập vào số phòng";
            if(string.IsNullOrEmpty (txtCustomerName.Text)) 
                return "Vui lòng nhập vào tên khách hàng";
            if (string.IsNullOrEmpty(txtCustomerIdShow.Text))
                return "Vui lòng nhập vào mã định danh khách hàng" ;
            if(string.IsNullOrEmpty(txtPhoneNumber.Text))
                return "Vui lòng nhập vào số điện thoại khách hàng";
            return null;
        }
        List<ListRoomBooked> listRooms = new List<ListRoomBooked>();

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string error=ErrorMessage();
            if(error!=null)
            {
                MessageBox.Show(error, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            
            listRooms.Add(new ListRoomBooked(txtRoomNumber.Text,dtpArrivedDate.Text,dtpExpectedRoom.Text));
            dtgvListBookedRoom.DataSource = null;
            dtgvListBookedRoom.DataSource = listRooms;
            dtgvListBookedRoom.Refresh();
            foreach (ListViewItem item in lsvRoomEmpty.Items)
            {
                if (item.Text == txtRoomNumber.Text)
                {
                    lsvRoomEmpty.Items.Remove(item);
                    lsvRoomBooked.Items.Add(item);
                    lsvRoomBooked.Refresh();
                    lsvRoomEmpty.Refresh();
                    break;
                }
            }
        }
    }
}
