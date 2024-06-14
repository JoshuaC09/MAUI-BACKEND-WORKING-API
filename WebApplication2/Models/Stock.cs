namespace WebApplication2.Models
{
    public class Stock
    {
        public string StkSCode { get; set; }
        public string StkItemNo { get; set; }
        public string StkLocation { get; set; }
        public string StkSubLocation { get; set; }
        public string StkSourceUOM { get; set; }
        public string StkLotNo { get; set; }
        public DateTime? StkExpiration { get; set; } // Nullable DateTime
        public int StkAvailableQty { get; set; }
        public int StkActualQty { get; set; }
        public DateTime? StkLogDateTime { get; set; } // Nullable DateTime
    }
}
