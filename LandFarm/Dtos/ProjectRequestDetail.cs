namespace LandFarm.Dtos
{
    public class ProjectRequestDetail
    {
        public int ID { get; set; }
        public int Pro_Request_Items_ID { get; set; }
        public int Org_ID { get; set; }
        public string Expert_ID { get; set; }
        public string St_Cost { get; set; }
        public DateTime DurationDate { get; set; }
    }
}
