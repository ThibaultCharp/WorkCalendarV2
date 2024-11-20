namespace WorkCalendarV2.Requests
{
    public class RegisterRequest
    {
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public int roleId { get; set; }
    }
}
