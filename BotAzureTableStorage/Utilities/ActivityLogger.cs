using Microsoft.Bot.Connector;
using Microsoft.WindowsAzure.Storage.Table;
using static Microsoft.Bot.Builder.Azure.TableLogger;

namespace BotAzureTableStorage.Utilities
{
    public class ActivityLogger : ActivityEntity
    {
        public string _message { set; get; }

        public string _partionKey { set; get; }

        public string _rowKey { set; get; }

        public string from { set; get; }

        public string receipt { set; get; }

        public ActivityLogger(IActivity activity) : base(activity)
        {
            _message = activity.AsMessageActivity().Text;
        }
    }

}