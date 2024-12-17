using LogicLayer.Entitys;

namespace WorkCalendarV2.Requests
{
    public class UpdateActivityRequest
    {
        public int ActivityId { get; set; }
        public int position_id { get; set; }
        public string begintime { get; set; }
        public string endtime { get; set; }
        public string date { get; set; }
    }
}
