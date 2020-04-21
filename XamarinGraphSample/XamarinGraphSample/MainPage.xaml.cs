using Microsoft.Graph;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinGraphSample
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            SignInButton.IsVisible = true;
            UserPanel.IsVisible = false;
            SignOutButton.IsVisible = false;
            DashboardButton.IsVisible = false;
            SetTheStage();
        }
        private async void SetTheStage()
        {
            var accounts = await App.PCA.GetAccountsAsync();
            if (accounts.Count() > 0)
            {
                var silentAuthResult = await App.PCA
                       .AcquireTokenSilent(App.Scopes, accounts.FirstOrDefault())
                       .ExecuteAsync();
                App.User = await GetUserInfo();
                SignInButton.IsVisible = false;
                UserPanel.IsVisible = true;
                SignOutButton.IsVisible = true;
                DashboardButton.IsVisible = true;
            }
            else
            {
                SignInButton.IsVisible = true;
                UserPanel.IsVisible = false;
                SignOutButton.IsVisible = false;
                DashboardButton.IsVisible = false;
            }
        }
        private async void OnSignIn(object sender, EventArgs e)
        {

            // First, attempt silent sign in
            // If the user's information is already in the app's cache,
            // they won't have to sign in again.
            string accessToken = string.Empty;
            try
            {
                var accounts = await App.PCA.GetAccountsAsync();
                if (accounts.Count() > 0)
                {
                    var silentAuthResult = await App.PCA
                        .AcquireTokenSilent(App.Scopes, accounts.FirstOrDefault())
                        .ExecuteAsync();

                    Debug.WriteLine("User already signed in.");
                    Debug.WriteLine($"Access token: {silentAuthResult.AccessToken}");
                    accessToken = silentAuthResult.AccessToken;
                }
            }
            catch (MsalUiRequiredException)
            {
                // This exception is thrown when an interactive sign-in is required.
                Debug.WriteLine("Silent token request failed, user needs to sign-in");
            }

            if (string.IsNullOrEmpty(accessToken))
            {
                // Prompt the user to sign-in
                var interactiveRequest = App.PCA.AcquireTokenInteractive(App.Scopes);

                if (App.AuthUIParent != null)
                {
                    interactiveRequest = interactiveRequest
                        .WithParentActivityOrWindow(App.AuthUIParent);
                }

                var authResult = await interactiveRequest.ExecuteAsync();
                Debug.WriteLine($"Access Token: {authResult.AccessToken}");

            }
            App.User = await GetUserInfo();
            SignInButton.IsVisible = false;
            UserPanel.IsVisible = true;
            SignOutButton.IsVisible = true;
            DashboardButton.IsVisible = true;
        }

        private async Task<User> GetUserInfo()
        {
            var GraphClient = new GraphServiceClient(new DelegateAuthenticationProvider(
                async (requestMessage) =>
                {
                    var accounts = await App.PCA.GetAccountsAsync();

                    var result = await App.PCA.AcquireTokenSilent(App.Scopes, accounts.FirstOrDefault())
                        .ExecuteAsync();

                    requestMessage.Headers.Authorization =
                        new AuthenticationHeaderValue("Bearer", result.AccessToken);
                }));
            App.User = await GraphClient.Me.Request().GetAsync();
            //var profilePhoto = GraphClient.Me.Photo.Request().GetAsync();
            //UserProfileImage.Source = user.Photo.ODataType;
            //string UserPhoto = await GetUserPhoto();
            UserNameLabel.Text = App.User.DisplayName;
            App.UserPhotoStream = await GetUserPhoto();
            UserProfileImage.Source = ImageSource.FromStream(() => App.UserPhotoStream);
            //UserName = user.DisplayName;
            //UserEmail = string.IsNullOrEmpty(user.Mail) ? user.UserPrincipalName : user.Mail;
            //App.User = user;
            return App.User;
        }

        private async void OnSignOut(object sender, EventArgs e)
        {
            var accounts = await App.PCA.GetAccountsAsync();
            while (accounts.Any())
            {
                // Remove the account info from the cache
                await App.PCA.RemoveAsync(accounts.First());
                accounts = await App.PCA.GetAccountsAsync();
            }
            SignInButton.IsVisible = true;
            UserPanel.IsVisible = false;
            SignOutButton.IsVisible = false;
            DashboardButton.IsVisible = false;
        }
        private async void OnDashboard(object sender, EventArgs e)
        { }
        private async Task<Stream> GetUserPhoto()
        {
            // Return the default photo
            //return Assembly.GetExecutingAssembly().GetManifestResourceStream("GraphTutorial.no-profile-pic.png");
            //ProfilePhoto UserPhoto = await GraphClient.Me.Photo.Request().GetAsync();

            //return UserPhoto;
            var accounts = await App.PCA.GetAccountsAsync();

            var result = await App.PCA.AcquireTokenSilent(App.Scopes, accounts.FirstOrDefault())
                .ExecuteAsync();

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var httpResponse = await httpClient.GetAsync($"https://graph.microsoft.com/v1.0/me/photo/$value");
                var httpResponseBody = await httpResponse.Content.ReadAsStreamAsync();
                return httpResponseBody;
            }
        }
    }
}
