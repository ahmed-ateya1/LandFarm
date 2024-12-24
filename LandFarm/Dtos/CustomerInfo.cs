namespace LandFarm.Dtos
{
    public class CustomerInfo
    {
        public int ID { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public int Type { get; set; }
        public string Mail_text { get; set; }
        public string Address { get; set; }
        public int? Gov_COD { get; set; } 
        public DateTime ST_Date { get; set; }
    }
}
