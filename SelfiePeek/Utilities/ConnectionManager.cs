using Windows.Networking.Connectivity;

namespace SelfiePeek.Utilities
{
    public class ConnectionManager
    {
        public static bool CheckInternetAccess()
        {
            var ConnectionProfile = NetworkInformation.GetInternetConnectionProfile();
            return (ConnectionProfile != null &&
                ConnectionProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess);
        }
    }
}
