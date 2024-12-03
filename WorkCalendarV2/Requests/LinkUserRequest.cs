namespace WorkCalendarV2.Requests
{
    public class LinkUserRequest
    {
        public string LoggedInUserEmail { get; set; }
        public string TargetUserEmail { get; set; }
    }
}
