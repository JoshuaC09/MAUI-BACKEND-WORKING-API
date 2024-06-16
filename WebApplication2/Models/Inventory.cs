namespace WebApplication2.Models
{
    public class Inventory
    {
        public int ItemId { get; set; }
        public string ItemNo { get; set; } = string.Empty;
        public string ItemDescription { get; set; } = string.Empty;
        public string ItemUom { get; set; } = string.Empty;
        public string ItemLotNumber { get; set; } = string.Empty;
        public string ItemExpiry { get; set; } = string.Empty;
        public int ItemQuantity { get; set; }
        public string ItemDateLog { get; set; } = string.Empty;
        public string ItemCouCode { get; set; } = string.Empty;
        public string ItemEmpCode { get; set; } = string.Empty;
    }
}
