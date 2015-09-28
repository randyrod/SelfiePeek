
namespace SelfiePeek.Utilities
{
    /// <summary>
    /// This class is used as holder for the strings that are being used throught the application.
    /// </summary>
    public class SharedStrings
    {
        public static string AccessTokenKeyValue = "appAccessToken";
        public static string AuthenticationURI = @"https://instagram.com/oauth/authorize/?client_id=c79a7451cfc64faf8b081dbea8ac365a&redirect_uri=http://instagram.com&response_type=token";
        public static string InstagramAPIBaseURI = @"https://api.instagram.com/v1/tags/selfie/media/recent?access_token=";
        public static string CurrentToken { get; set; }
    }
}
