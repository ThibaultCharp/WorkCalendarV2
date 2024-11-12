namespace WorkCalendarV2.Models
{
    public class CreateActivityRequest
    {
        public int position_id { get; set; }
        public string begintime { get; set; }
        public string endtime { get; set; }
        public string date { get; set; }
        public int employee_id { get; set; }
    }
}
