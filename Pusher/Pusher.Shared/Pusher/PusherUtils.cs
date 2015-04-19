using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Pusher.Pusher
{
    class PusherUtils
    {
        public static readonly string CLIENT_ID = "IfgaX7cNfg0bdIcXgoLmROL6xFlT9dgq";
        public static readonly string CLIENT_SECRET = "w6QmD8gtBtz9gcT6RcjJ9JtIwgP5KVRx";
        public static readonly string REDIRECT_URI = "http://andreapivetta.altervista.org";
        public static readonly string LOGIN_KEY = "isloggedin";
        public static readonly string ACCESS_TOKEN_KEY = "token";

        public static bool IsUserLoggedIn()
        {
            if (Windows.Storage.ApplicationData.Current.LocalSettings.Values.ContainsKey(LOGIN_KEY))
                return (bool)Windows.Storage.ApplicationData.Current.LocalSettings.Values[LOGIN_KEY];

            return false;
        }

        public static string GetPushbulletLoginURL()
        {
            return "https://www.pushbullet.com/authorize?client_id=" + PusherUtils.CLIENT_ID +
                "&redirect_uri=" + Uri.EscapeUriString(PusherUtils.REDIRECT_URI) + "&response_type=token";
        }

        public static void StoreAccessToken(string redirect)
        {
            Windows.Storage.ApplicationData.Current.LocalSettings.Values[ACCESS_TOKEN_KEY] =
                redirect.Substring(redirect.IndexOf('=') + 1);
            Windows.Storage.ApplicationData.Current.LocalSettings.Values[LOGIN_KEY] = true;
        }

        public async static void pushNote(string message, string title = "", string device = "")
        {
            var values = new Dictionary<string, string>
            {
               { "type", "note" },
               { "title", title },
               { "body", message },
               { "device_iden", device}
            };

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                (string)Windows.Storage.ApplicationData.Current.LocalSettings.Values[ACCESS_TOKEN_KEY]);
            var response = await client.PostAsync(
                "https://api.pushbullet.com/v2/pushes", new FormUrlEncodedContent(values));
            var responseString = await response.Content.ReadAsStringAsync();

            System.Diagnostics.Debug.WriteLine(responseString);
        }
    }
}
