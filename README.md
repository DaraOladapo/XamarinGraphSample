# Xamarin Graph Sample
## Working with Microsoft Graph SDK in Xamarin.Forms

This code sample shows how to use Microsoft Graph SDK with your Xamarin.Forms application.

Original Docs Link: https://docs.microsoft.com/en-us/graph/tutorials/xamarin

## Dependencies
- Microsoft.Graph
- Microsoft.Graph.Core
- Microsoft.Identity.Client

## Prerequisites
A personal Microsoft Account or an Office (Microsoft) 365 Subscription.

- Sign up for a personal Microsoft Account [here](https://signup.live.com/signup?wa=wsignin1.0&rpsnv=12&ct=1454618383&rver=6.4.6456.0&wp=MBI_SSL_SHARED&wreply=https://mail.live.com/default.aspx&id=64855&cbcxt=mai&bk=1454618383&uiflavor=web&uaid=b213a65b4fdc484382b6622b3ecaa547&mkt=E-US&lc=1033&lic=1).
- Sign up for the Office 365 Developer Program [here](https://developer.microsoft.com/office/dev-program).


## TODO: Create OAuthSettings.cs file
To save my keys and ID, I had to add that OAuthSettings.cs to my .gitignore file.
No worries, it's simple.
In the Models Folder, create a C Sharp file called OAuthSettings.cs
This is what it will look like. Be sure to replace `<yourAppID>` and `<yourAppBundleID>` with the Application (Client ID) of your Azure AD Application and the ID of your app bundle respectively.

    public static class OAuthSettings
    {
        public const string ApplicationId = "<yourAppID>";
        public const string Scopes = "User.Read Calendars.Read";
        public const string RedirectUri = "msauth://<yourAppBundleID>";
    }

Also, be sure to go into your `AndroidManifest.xml` file to change the `package` value to what you would like it to be (it should be the value you used when setting up the Azure AD Application). Do the same for your iOS Project-that would be the `Info.plist` file (Bundle Identifier).

From here on, all things should be good.

## Changes I made to my code

I followed the official documentation up till here then I realized the user information was not showing after I finished the sign-in process (I might have missed some steps) so I modified my `InitializeGraphClientAsync()` method to initialize sign in if no account is found.

    var interactiveRequest = PCA.AcquireTokenInteractive(Scopes);

    if (AuthUIParent != null)
    {
        interactiveRequest = interactiveRequest
            .WithParentActivityOrWindow(AuthUIParent);
    }

    var interactiveAuthResult = await interactiveRequest.ExecuteAsync();
    //recursive call to the same function
    await InitializeGraphClientAsync();

## Scopes
So that I am able to get my graph API calls to work fine, I created an `ApplicationScopes` class that contains fields of strings. It looks something like the image below. This I will be using for my various scenarios.

![Application Scopes](/Images/ApplicationScopes.png)

For more information on scopes and permission, visit this [link](https://docs.microsoft.com/en-us/graph/permissions-reference#mail-permissions).

## Issues
Running on an iOS emulator for now gives me an error telling me to enable KeyChain access. It works fine deploying to a phone.
Trying to fix with this [doc](https://docs.microsoft.com/en-us/azure/active-directory/develop/msal-net-xamarin-ios-considerations) and [this](https://damienaicheh.github.io/azure/xamarin/xamarin.forms/2019/07/01/sign-in-with-microsoft-account-with-xamarin-en.html).
This part of the former link solved it for me.
![Xamarin iOS Entitlement](/Images/xamarin-ios-plist.png)
