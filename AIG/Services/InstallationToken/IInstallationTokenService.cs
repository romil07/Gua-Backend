using AIG.Contracts;
using AIG.Models;
using System.Collections.Generic;

namespace AIG.Services.InstallationToken
{
    /// <summary>
    /// Interface capturing methods of installation token service.
    /// </summary>
    public interface IInstallationTokenService
    {
        /// <summary>
        /// Fetched installation token for given context.
        /// </summary>
        /// <param name="installationTokenServiceContext">Installation token context.</param>
        /// <returns>InstallationTokenResponse</returns>
        public IEnumerable<InstallationTokenResponse> FetchInstallationToken(
            InstallationTokenServiceContext installationTokenServiceContext);
    }
}
