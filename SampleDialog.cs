namespace QuickReplies
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;
    using Models;
    using Newtonsoft.Json.Linq;

    [Serializable]
    public class SampleDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            await context.PostAsync("Hi! I'm sample bot that will show you how to use Facebook's Quick Replies with BotFramework.");

            var reply = context.MakeMessage();
            reply.Text = "What's your favorite color?";

            if (reply.ChannelId.Equals("facebook", StringComparison.InvariantCultureIgnoreCase))
            {
                //var channelData = JObject.FromObject(new
                //{
                //    quick_replies = new dynamic[]
                //    {
                //        new
                //        {
                //            content_type = "text",
                //            title = "Blue",
                //            payload = "DEFINED_PAYLOAD_FOR_PICKING_BLUE",
                //            image_url = "https://cdn3.iconfinder.com/data/icons/developperss/PNG/Blue%20Ball.png"
                //        },
                //        new
                //        {
                //            content_type = "text",
                //            title = "Green",
                //            payload = "DEFINED_PAYLOAD_FOR_PICKING_GREEN",
                //            image_url = "https://cdn3.iconfinder.com/data/icons/developperss/PNG/Green%20Ball.png"
                //        },
                //        new
                //        {
                //            content_type = "text",
                //            title = "Red",
                //            payload = "DEFINED_PAYLOAD_FOR_PICKING_RED",
                //        }
                //    }
                //});

                var channelData = new FacebookChannelData
                {
                    QuickReplies = new[]
                    {
                        new FacebookTextQuickReply("Blue", "DEFINED_PAYLOAD_FOR_PICKING_BLUE", "https://cdn3.iconfinder.com/data/icons/developperss/PNG/Blue%20Ball.png"),
                        new FacebookTextQuickReply("Green", "DEFINED_PAYLOAD_FOR_PICKING_GREEN", "https://cdn3.iconfinder.com/data/icons/developperss/PNG/Green%20Ball.png"),
                        new FacebookTextQuickReply("Red", "DEFINED_PAYLOAD_FOR_PICKING_RED")
                    }
                };

                reply.ChannelData = channelData;
            }

            await context.PostAsync(reply);

            context.Wait(this.OnColorPicked);
        }

        private async Task OnColorPicked(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var colorMessage = await result;

            var message = $"Color picked: {colorMessage.Text}.";

            if (colorMessage.ChannelId.Equals("facebook", StringComparison.InvariantCultureIgnoreCase))
            {
                var quickReplyResponse = colorMessage.ChannelData.message.quick_reply;

                if (quickReplyResponse != null)
                {
                    message += $" The payload for the quick reply clicked is: {quickReplyResponse.payload}";
                }
                else
                {
                    message += " It seems you didn't click on a quick reply and you just typed the color.";
                }
            }

            await context.PostAsync(message);

            context.Wait(this.MessageReceivedAsync);
        }
    }
}