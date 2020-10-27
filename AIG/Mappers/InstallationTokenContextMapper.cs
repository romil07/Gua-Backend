using AIG.Contracts;
using AIG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AIG.Mappers
{
    public class InstallationTokenContextMapper
    {
		/// <summary>
		/// Maps installation contract to service contract
		/// </summary>
		/// <param name="installationTokenContext"></param>
		/// <returns>InstallationTokenServiceContext</returns>
		public InstallationTokenServiceContext MapToInstallationTokenServiceContext(
			InstallationTokenContext installationTokenContext)
		{
			InstallationTokenServiceContext installationTokenServiceContext = new InstallationTokenServiceContext();
			installationTokenServiceContext.ClientId = ""; //TODO: fill this
			installationTokenContext.Repos = installationTokenContext.Repos;
			installationTokenServiceContext.Permissions = installationTokenContext.Permissions;
			installationTokenServiceContext.TenantId = ""; //TODO: fill this
			installationTokenServiceContext.SubscriptionId = installationTokenContext.SubscriptionId;
			return installationTokenServiceContext;
		}
	}
}
