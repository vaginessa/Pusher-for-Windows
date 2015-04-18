using System;
using System.Collections.Generic;
using System.Text;

namespace Pusher.Pusher
{
    class PusherUtils
    {
        public static readonly string CLIENT_ID = "IfgaX7cNfg0bdIcXgoLmROL6xFlT9dgq";
        public static readonly string CLIENT_SECRET = "w6QmD8gtBtz9gcT6RcjJ9JtIwgP5KVRx";
        public static readonly string REDIRECT_URI = "http://andreapivetta.altervista.org";
        public static readonly string LOGIN_KEY = "isloggedin";

        public static bool isUserLoggedIn()
        {
            if (Windows.Storage.ApplicationData.Current.LocalSettings.Values.ContainsKey(LOGIN_KEY))
                return (bool) Windows.Storage.ApplicationData.Current.LocalSettings.Values[LOGIN_KEY];

            return false;
        }

        public static string getPushbulletLoginURL()
        {
            return "https://www.pushbullet.com/authorize?client_id=" + PusherUtils.CLIENT_ID +
                "&redirect_uri=" + Uri.EscapeUriString(PusherUtils.REDIRECT_URI) + "&response_type=token";
        }

    }
}
