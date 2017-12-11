using System;
using System.Linq;
using System.Collections.Generic;
using System.Timers;
using SimpleFeedReader;
using System.Threading.Tasks;

namespace MarvinSWI.Controllers
{ 
        // Define a class to hold custom event info
     class BuildEventArgs : EventArgs
    {
        public BuildEventArgs(string s)
        {
            message = s;
        }
        private string message;

        public string Message
        {
            get { return message; }
            set { message = value; }
        }
    }

    class BuildReportController
    {
        public event EventHandler<BuildEventArgs> RaiseBuildEvent;

        FeedReader reader;
        IEnumerable<FeedItem> oldItems;
     
        public BuildReportController()
        {
            reader = new FeedReader();
        }

        public async void GetBuilds(string uri)
        {

            // var items = await Task.Run(() => reader.RetrieveFeed(uri));
            var items = reader.RetrieveFeed(uri);

            if (oldItems != null)
            {
                var newFails = from newItem in items
                               where !(from oldItem in oldItems
                                       select oldItem.Title)
                                       .Contains(newItem.Title)
                               select newItem;

                foreach (var i in newFails)
                {
                    var newBuildEvent = new BuildEventArgs("hue");
                    OnRaiseBuildEvent(newBuildEvent, i.Title);
                }
            }

            oldItems = items;
        }

        protected virtual void OnRaiseBuildEvent(BuildEventArgs e, string buildMsg)
        {
            EventHandler<BuildEventArgs> handler = RaiseBuildEvent;

            if (handler != null)
            {
                e.Message = buildMsg;
                handler(this, e);
            }
        }
    }
}