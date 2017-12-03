using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Builder.History;
using Microsoft.Bot.Connector;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BotAzureTableStorage.Utilities
{
    public class CustomTableLogger : IActivityLogger
    {
        Task IActivityLogger.LogAsync(IActivity activity)
        {
            var store = new TableBotDataStore(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);

            CloudTable _table;

            _table = store.Table;

            if (!activity.Timestamp.HasValue)
            {
                activity.Timestamp = DateTime.UtcNow;
            }

            return Write(_table, activity);
        }

        private static Task Write(CloudTable table, IActivity activity, int retriesLeft = 5)
        {
            var insert = TableOperation.Insert(new ActivityLogger(activity));
            return table.ExecuteAsync(insert).ContinueWith(t =>
            {
                if (--retriesLeft > 0 && t.IsFaulted)
                {
                    var response = ((t.Exception.InnerException as StorageException)?.InnerException as System.Net.WebException)?.Response as System.Net.HttpWebResponse;
                    if (response != null && response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        activity.Timestamp = activity.Timestamp.Value.AddTicks(1);

                        return CustomTableLogger.Write(table, activity, retriesLeft);
                    }
                }
                t.Wait();
                return t;
            }).Unwrap();
        }
    }
}