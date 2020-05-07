# Xamarin Graph Sample
## Working with Microsoft Graph SDK in Xamarin.Forms

Ths code sample shows how to use Microsoft Graph SDK with your Xamarin.Forms application.

Original Docs Link: https://docs.microsoft.com/en-us/graph/tutorials/xamarin

## Dependencies
- Microsoft.Graph
- Microsoft.Graph.Core
- Microsoft.Identity.Client
- Newtonsoft.Json

## TODO: Create OAuthSettings.cs file
To save my keys and ID, I had to add that OAuthSettings.cs to my .gitignore file.
No worries, it's simple.
In the Models Folder, create a C Sharp file called OAuthSettings.cs
This is what it will look like. Be sure to replace `<yourAppID>` and `<youappbundleid>` with the Application (Client ID) of your Azure AD Application and the ID of your app bundle respectively.

     public static class OAuthSettings
        {
            public const string ApplicationId = "<yourAppID>";
            public const string Scopes = "User.Read Calendars.Read";
            public const string RedirectUri = "msauth://<youappbundleid>";
        }