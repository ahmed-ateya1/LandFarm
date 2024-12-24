namespace LandFarm.Dtos
{
    public class ProjectExpertDetail
    {
        public int ID { get; set; }
        public int Project_ID { get; set; }
        public int Organization_ID { get; set; }
        public string Pro_Request_Items { get; set; }
        public int Expert_ID { get; set; }
        public DateTime Request_Date { get; set; }
    }
}
