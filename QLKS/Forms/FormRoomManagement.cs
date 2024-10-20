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
    public partial class FormRoomManagement : Form
    {
        
        int disBefore = 0;
        void CreateRoomUI(int index, LoadingListRoom loading, int indexType)
        {
            Panel panelRoom = new Panel();
            if (loading.Status == "Đã đặt")
                panelRoom.BackColor = System.Drawing.Color.DimGray;
            else
                panelRoom.BackColor = System.Drawing.Color.SeaGreen;
            Panel panelChild = new Panel();
            panelChild.BackColor = System.Drawing.Color.Gainsboro;
            Label dayStart = new Label();
            dayStart.AutoSize = true;
            dayStart.ForeColor = System.Drawing.Color.Black;
            dayStart.Location = new System.Drawing.Point(3, 7);
            dayStart.Name = "lbl_dayStart";
            dayStart.Size = new System.Drawing.Size(52, 25);
            dayStart.TabIndex = 0;
            if (loading.Status == "Đã đặt")
                dayStart.Text = loading.ArrivedDate;
            else
                dayStart.Text = "Trống";
            Label dayEnd = new Label();
            dayEnd.AutoSize = true;
            dayEnd.ForeColor = System.Drawing.Color.Black;
            dayEnd.Location = new System.Drawing.Point(120, 7);
            dayEnd.Name = "lbl_dayEnd";
            dayEnd.Size = new System.Drawing.Size(52, 25);
            dayEnd.TabIndex = 0;
            if (loading.Status == "Đã đặt")
                dayEnd.Text = loading.ExpectedDate;
            else
                dayEnd.Text = "Trống";
            panelChild.Controls.Add(dayStart);
            panelChild.Controls.Add(dayEnd);
            panelChild.Dock = System.Windows.Forms.DockStyle.Bottom;
            panelChild.Location = new System.Drawing.Point(0, 88);
            panelChild.Name = "panelChild";
            panelChild.Size = new System.Drawing.Size(271, 37);
            panelRoom.Controls.Add(panelChild);
            PictureBox pictureBoxStatus = new PictureBox();
            if (loading.Status == "Đã đặt")
                pictureBoxStatus.Image = ((System.Drawing.Image)(Properties.Resources.user));
            else
                pictureBoxStatus.Image = ((System.Drawing.Image)(Properties.Resources.check));
            pictureBoxStatus.Location = new System.Drawing.Point(29, 53);
            pictureBoxStatus.Name = "pictureBox2";
            pictureBoxStatus.Size = new System.Drawing.Size(24, 24);
            pictureBoxStatus.TabStop = false;
            panelRoom.Controls.Add(pictureBoxStatus);
            Label roomStatus = new Label();
            roomStatus.AutoSize = true;
            roomStatus.Location = new System.Drawing.Point(118, 9);
            roomStatus.Name = "label2";
            roomStatus.Size = new System.Drawing.Size(120, 25);
            roomStatus.TabIndex = 0;
            if (loading.Status == "Đã đặt")
                roomStatus.Text = "Phòng đã đặt";
            else
                roomStatus.Text = "Phòng trống";
            panelRoom.Controls.Add(roomStatus);
            Label nameCustomer = new Label();
            nameCustomer.AutoSize = true;
            nameCustomer.Location = new System.Drawing.Point(81, 53);
            nameCustomer.Name = "label4";
            nameCustomer.Size = new System.Drawing.Size(131, 25);
            nameCustomer.TabIndex = 0;
            if (loading.Status == "Đã đặt")
                nameCustomer.Text = loading.CustomerName;
            else
                nameCustomer.Text = "Phòng trống";
            panelRoom.Controls.Add(nameCustomer);
            Label nameRoom = new Label();
            nameRoom.AutoSize = true;
            nameRoom.Location = new System.Drawing.Point(14, 9);
            nameRoom.Name = "label1";
            nameRoom.Size = new System.Drawing.Size(52, 25);
            nameRoom.TabIndex = 0;
            nameRoom.Text = loading.Number;
            panelRoom.Controls.Add(nameRoom);

            panelRoom.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            panelRoom.ForeColor = System.Drawing.Color.White;
            int dis = 0;
            if (index >= 4)
            {
                dis = index / 4;
            }
            if ((index / 4) > 0)
                panelRoom.Location = new System.Drawing.Point(33 + (index % 4) * 315, 120 + disBefore * 140 + indexType * 180 + dis * 140);
            else
                panelRoom.Location = new System.Drawing.Point(33 + index * 315, 120 + disBefore * 140 + indexType * 180 + dis * 140);
            panelRoom.Name = "panelRoom";
            panelRoom.Size = new System.Drawing.Size(220, 125);
            panelMain.Controls.Add(panelRoom);
        }
        void LoadRoomType(int indexType, string roomTypeName)
        {
            Label roomType = new Label();
            roomType.AutoSize = true;
            roomType.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            roomType.Location = new System.Drawing.Point(30, 95 + disBefore * 140 + indexType * 178);
            roomType.Name = "lblRoom";
            roomType.Size = new System.Drawing.Size(109, 28);
            roomType.Text = roomTypeName;
            panelMain.Controls.Add(roomType);
        }
        DbContext db = new DbContext(DbContext.ConnectionType.ConfigurationManager, "DefaultConnection");
        void LoadListRoom()
        {
            List<IEnumerable<Room>> rooms = new List<IEnumerable<Room>>();
            IEnumerable<LoadingListRoom> listRoom = LoadingListRoom.GetRooms(db);
            IEnumerable<RoomType> roomTypes = db.GetTable<RoomType>();
            List<RoomType> roomTypesCopy = new List<RoomType>();
            foreach (RoomType roomType in roomTypes)
            {
                rooms.Add(db.GetTable<Room>(t => t.RoomType == roomType.Id));
                roomTypesCopy.Add(roomType);
            }
            int indexType = 0;
            int countSum = 0;
            int countBefore = 0;
            foreach (IEnumerable<Room> lst in rooms)
            {
                int index = 0;
                foreach (Room room in lst)
                {
                    LoadingListRoom loading = new LoadingListRoom(room, db);
                    if (room.Status == "Đã đặt")
                        loading = listRoom.Where(t => t.IdRoom == room.Id).First();
                    CreateRoomUI(index, loading, indexType);
                    index++;
                    countSum++;
                }
                LoadRoomType(indexType, roomTypesCopy[indexType].Name);
                indexType++;
                countBefore = lst.Count() - 1;
                disBefore += (lst.Count() - 1) / 4;
            }
        }
        public FormRoomManagement()
        {
            InitializeComponent();
        }

        private void FormRoomManagement_Load(object sender, EventArgs e)
        {
            LoadListRoom();
        }
    }
}
