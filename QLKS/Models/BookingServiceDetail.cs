namespace QLKS.Models
{
    [Table("CHITIETPHIEUDV")]
    public class BookingServiceDetail
    {
        [Column("MAPHIEUDV", true)]
        public int BookingService { get; set; }
        [Column("MADV", true)]
        public int Service { get; set; }
        [Column("DONGIA")]
        public decimal Price { get; set; }
        [Column("SOLUONG")]
        public int Quantity { get; set; }

        public BookingServiceDetail() { }
    }
}
