using System;
using System.Collections.Generic;
using System.Text;

namespace Pusher.DataModel
{
    class PushLink : Push
    {
        public Uri url { get; set; }

        public PushLink(string Iden, TYPES Type, string Title, long Created, long Modified, string url) 
            : base(Iden, Type, Title, Created, Modified)
        {
            this.url = new Uri(url);
        }
    }
}
