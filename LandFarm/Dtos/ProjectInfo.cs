namespace LandFarm.Dtos
{
    public class ProjectInfo
    {
        public int ID { get; set; }
        public string ProjectNameAr { get; set; }
        public string ProjectNameEn { get; set; }
        public int Pro_Type { get; set; }
        public int Pro_Place { get; set; }
        public DateTime St_Date { get; set; }
        public int Customer_ID { get; set; }
        public string St_Budget { get; set; }
        public int Gov_COD { get; set; }
        public string Pro_Address { get; set; }
        public string Pro_LocationDetail { get; set; }
        public string Pro_Mail { get; set; }
    }
}