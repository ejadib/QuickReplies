namespace QuickReplies.Models
{
    public class FacebookChannelData
    {
        [Newtonsoft.Json.JsonProperty("quick_replies")]
        public FacebookQuickReply[] QuickReplies { get; set; }
    }
}