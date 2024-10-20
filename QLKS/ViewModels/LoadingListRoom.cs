using QLKS.Models;
using System.Collections.Generic;
using System.Linq;

namespace QLKS.ViewModels
{
    public class LoadingListRoom
    {
        BookingRoom bookingRoom;
        BookingRoomDetail bookingRoomDetail;
        Customer customer;
        Room room;

        public BookingRoom BookingRoom { get => bookingRoom; set => bookingRoom = value; }
        public Customer Customer { get => customer; set => customer = value; }
        public Room Room { get => room; set => room = value; }
        public BookingRoomDetail BookingRoomDetail { get => bookingRoomDetail; set => bookingRoomDetail = value; }
        public string Number { get; set; }
        public string Status {  get; set; }
        public string CustomerName {  get; set; }
        public string ArrivedDate { get; set; }
        public string ExpectedDate { get; set; }
        public int Id {  get; set; }
        public int IdRoom {  get; set; }
        

        public LoadingListRoom() 
        { 
        }
        public LoadingListRoom(Room room, DbContext db)
        {
            this.room = room;
            if(room.Status=="Đã đặt")
            {
                bookingRoomDetail = db.GetTable<BookingRoomDetail>(t => t.Room == room.Id).FirstOrDefault();
                if (bookingRoomDetail != null)
                {
                    Id = bookingRoomDetail.BookingRoom;
                    bookingRoom = db.GetTable<BookingRoom>(t => t.Id == Id).First();
                    customer = db.GetTable<Customer>(t => t.Id == bookingRoom.Customer).First();
                    CustomerName = customer.Name;
                    ArrivedDate = bookingRoom.ArrivedDate.ToString("dd/MM/yyyy");
                    ExpectedDate = bookingRoom.ExpectedDate.ToString("dd/MM/yyyy");
                    IdRoom = room.Id;
                }
            }
            Number = room.Name;
            Status = room.Status;

        }
        public static IEnumerable<LoadingListRoom> GetRooms(DbContext context)
        {
            foreach (Room room in context.GetTable<Room>())
            {
                yield return new LoadingListRoom(room, context);
            }
        }

    }
}
