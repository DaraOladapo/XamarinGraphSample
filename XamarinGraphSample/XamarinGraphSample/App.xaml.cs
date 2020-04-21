using Microsoft.Graph;
using Microsoft.Identity.Client;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Application = Xamarin.Forms.Application;

namespace XamarinGraphSample
{
    public partial class App : Application
    {
        // UIParent used by Android version of the app
        public static object AuthUIParent = null;

        // Keychain security group used by iOS version of the app
        public static string iOSKeychainSecurityGroup = "com.daraoladapo.xamaringraphsample";

        // Microsoft Authentication client for native/mobile apps
        public static IPublicClientApplication PCA;

        // Microsoft Graph client
        public static GraphServiceClient GraphClient;
        internal static User User;
        internal static Stream UserPhotoStream;

        public static string TenantID = "fef59609-f695-49c1-8349-fc659e5549ae";
        public static string ClientID = "9e01a77f-8fc5-45c0-94a2-6f4e66894e04";
        public static string RedirectUri = "msal" + ClientID + "://auth";
        public static string[] Scopes = { "User.Read", "Mail.Send", "Files.ReadWrite" };
        public App()
        {
            InitializeComponent();
            User = new User();
            var builder = PublicClientApplicationBuilder
                .Create(ClientID);

            if (!string.IsNullOrEmpty(iOSKeychainSecurityGroup))
            {
                builder = builder.WithIosKeychainSecurityGroup(iOSKeychainSecurityGroup);
            }

            PCA = builder.Build();
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
