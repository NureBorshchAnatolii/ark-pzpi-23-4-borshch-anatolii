namespace CareLink.Api.Models.Requests
{
    public class UpdateMessageRequest
    {
        public long MessageId { get; set; }
        public string NewContent { get; set; }
    }
}