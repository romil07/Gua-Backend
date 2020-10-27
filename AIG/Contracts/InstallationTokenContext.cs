using System.Text.Json;

namespace AIG.Contracts
{
    public class InstallationTokenContext
    {
        public int[] Repos { get; set; }
        public JsonElement Permissions { get; set; }
        public string SubscriptionId { get; set; }
    }
}
