using System.Text.Json;

namespace AIG.Models
{
    public class InstallationTokenServiceContext
    {
        public string ClientId { get; set; }
        public int[] Repos { get; set; }
        public JsonElement Permissions { get; set; }
        public string TenantId { get; set; }
        public string SubscriptionId { get; set; }
    }
}
