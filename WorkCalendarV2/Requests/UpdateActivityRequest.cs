using LogicLayer.Entitys;

namespace WorkCalendarV2.Requests
{
    public class UpdateActivityRequest
    {
        public int ActivityId { get; set; }
        public string position { get; set; }
        public string begintime { get; set; }
        public string endtime { get; set; }
        public string date { get; set; }
        public string employer_name { get; set; }
        public string employer_email { get; set; }

    }
}
