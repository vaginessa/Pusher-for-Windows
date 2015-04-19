﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Pusher.Data;
using Pusher.Common;
using Pusher.Pusher;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.Security.Authentication.Web;
using System.Threading.Tasks;

namespace Pusher
{

    public sealed partial class HubPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        public HubPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;

            if (!PusherUtils.IsUserLoggedIn())
            {
                CreateLoginDialogAsync();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("I'm in");
                SetupAsync();
            }
        }

        private async void CreateLoginDialogAsync()
        {
            var dialog = new CoreWindowDialog("Welcome to Pusher");
            dialog.Commands.Add(new UICommand("Login with Pushbullet", PerformLoginAsync));
            await dialog.ShowAsync();
        }

        private async void PerformLoginAsync(IUICommand command)
        {
            try
            {
                System.Uri StartUri = new Uri(PusherUtils.GetPushbulletLoginURL());
                System.Uri EndUri = new Uri(PusherUtils.REDIRECT_URI);

                WebAuthenticationResult WebAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(
                                                        WebAuthenticationOptions.None, StartUri, EndUri);
                if (WebAuthenticationResult.ResponseStatus == WebAuthenticationStatus.Success)
                {
                    PusherUtils.StoreAccessToken(WebAuthenticationResult.ResponseData.ToString());
                }
                else if (WebAuthenticationResult.ResponseStatus == WebAuthenticationStatus.ErrorHttp)
                {
                    System.Diagnostics.Debug.WriteLine("HTTP Error returned by AuthenticateAsync() : " + 
                        WebAuthenticationResult.ResponseErrorDetail.ToString());
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Error returned by AuthenticateAsync() : " + 
                        WebAuthenticationResult.ResponseStatus.ToString());
                }
            }
            catch (Exception Error)
            {
                System.Diagnostics.Debug.WriteLine(Error.ToString());
            }

            SetupAsync();
        }

        private async void SetupAsync()
        {
            Dictionary<string, string> result = await PusherUtils.GetUserInfoAsync();
        }

        #region Autogenerated Stuff
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            var sampleDataGroup = await SampleDataSource.GetGroupAsync("Group-4");
            this.DefaultViewModel["Section3Items"] = sampleDataGroup;
        }

        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var itemId = ((SampleDataItem)e.ClickedItem).UniqueId;
            this.Frame.Navigate(typeof(ItemPage), itemId);
        }
        #endregion
        #region Registrazione di NavigationHelper

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private async void SendSimplePushButton_Click(object sender, RoutedEventArgs e)
        {
            TextBox simplePushTextBox = FindChildControl<TextBox>(this, "SimplePushTextBox") as TextBox;
            string message = simplePushTextBox.Text;

            if(message.Length > 0) 
            {
                PusherUtils.PushNoteAsync(message);
                simplePushTextBox.Text = "";
            }
            else
            {
                var messageDialog = new Windows.UI.Popups.MessageDialog("Hey, at least one character!");
                messageDialog.DefaultCommandIndex = 1;
                await messageDialog.ShowAsync();
            }
        }

        private DependencyObject FindChildControl<T>(DependencyObject control, string ctrlName)
        {
            int childNumber = VisualTreeHelper.GetChildrenCount(control);
            for (int i = 0; i < childNumber; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(control, i);
                FrameworkElement fe = child as FrameworkElement;
                // Not a framework element or is null
                if (fe == null) return null;

                if (child is T && fe.Name == ctrlName)
                {
                    // Found the control so return
                    return child;
                }
                else
                {
                    // Not found it - search children
                    DependencyObject nextLevel = FindChildControl<T>(child, ctrlName);
                    if (nextLevel != null)
                        return nextLevel;
                }
            }
            return null;
        }
    }
}
