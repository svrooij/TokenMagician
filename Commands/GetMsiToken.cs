using Azure.Identity;
using Microsoft.Extensions.Logging;
using System.Management.Automation;
using Svrooij.PowerShell.DI;
using Azure.Core;

namespace TokenMagician;
/// <summary>
/// <para type="synopsis">Get a token using managed identity</para>
/// <para type="description">Microsoft just released support for using managed identities as federated credentials, this library helps you get tokens.</para>
/// </summary>
/// <example>
/// <para type="name">Get a token for Graph using system-assigned identity</para>
/// <para type="description">This will use the system assigned managed identity to get a token</para>
/// <code>Get-TmMsiToken -TenantId "svrooij.io" -ClientId "81386206-d7fb-402a-b9dd-a4ed5bc3ef71" -Scope https://graph.microsoft.com/.default</code>
/// </example>
/// <example>
/// <para type="name">Get a token for Graph using user-assigned identity</para>
/// <para type="description">This will use the user assigned managed identity to get a token</para>
/// <code>Get-TmMsiToken -TenantId "svrooij.io" -ClientId "81386206-d7fb-402a-b9dd-a4ed5bc3ef71" -Scope https://graph.microsoft.com/.default -UserAssignedIdentity "a9f67427-921a-4aab-8c20-5a8a0acdd2a6"</code>
/// </example>
[Cmdlet(VerbsCommon.Get, "TmMsiToken")] // You can specify the name of the cmdlet
[OutputType(typeof(string))] // You can specify an output type as you're used to.
[GenerateBindings] // If your class is partial, and you want to go for max speed, add this attribute to have it generate the bindings at compile time.
[Alias("Get-MsiToken")]
public partial class GetMsiToken : DependencyCmdlet<Startup>
{
    /// <summary>
    /// Specifies the tenant id for which you want a token
    /// </summary>
    [Parameter(
        Mandatory = true,
        Position = 0,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true)]
    public string? TenantId { get; set; }

    /// <summary>
    /// Specify the client id of your multi-tenant application
    /// </summary>
    [Parameter(
        Mandatory = true,
        Position = 1,
        ValueFromPipelineByPropertyName = true)]
    public string? ClientId { get; set; }

    /// <summary>
    /// Specify the scope for which you want a token (ending with `/.default`)
    /// </summary>
    [Parameter(
        Mandatory = true,
        Position = 2,
        ValueFromPipelineByPropertyName = true)]
    public string? Scope { get; set; }

    /// <summary>
    /// The MSI scope to use, defaults to `api://AzureADTokenExchange/.default`, see <see href="https://learn.microsoft.com/en-us/entra/workload-id/workload-identity-federation-config-app-trust-managed-identity">this page</see> for options
    /// </summary>
    [Parameter(
        Mandatory = false,
        Position = 3,
        ValueFromPipelineByPropertyName = true)]
    public string MsiScope { get; set; } = "api://AzureADTokenExchange/.default";

    /// <summary>
    /// The user assigned identity to use, if not set, the system assigned identity will be used
    /// </summary>
    [Parameter(
        Mandatory = false,
        Position = 4,
        ValueFromPipelineByPropertyName = true)]
    public string? UserAssignedIdentity { get; set; }


    [ServiceDependency(Required = true)]
    private Microsoft.Extensions.Logging.ILogger<GetMsiToken>? _logger;

    /// <inheritdoc />
    public override async Task ProcessRecordAsync(CancellationToken cancellationToken)
    {        
        _logger?.LogInformation("Getting a token for {Scope} in tenant: {TenantId} using managed identity", Scope, TenantId);

        // Create MSI credential (if the UserAssignedIdentity is set, it will use that, otherwise it will use the system assigned identity)
        var msiCredential = new ManagedIdentityCredential(UserAssignedIdentity);

        // Fix the MsiScope
        if (!MsiScope.EndsWith("/.default"))
        {
            MsiScope += "/.default";
        }

        // Get a token for the MsiScope
        var token = await msiCredential.GetTokenAsync(new TokenRequestContext(new[] { MsiScope }), cancellationToken);
        _logger?.LogDebug("Got MSI token");

        // Use the MSI token to get a token for the requested scope
        var tokenCredential = new ClientAssertionCredential(TenantId!, ClientId!,() => token.Token);

        // Fix the scope
        if (!Scope!.EndsWith("/.default"))
        {
            Scope += "/.default";
        }

        // Get a token for the requested scope
        var scopeToken = await tokenCredential.GetTokenAsync(new TokenRequestContext(new[] { Scope! }), cancellationToken);
        _logger?.LogInformation("Got access token for {Tenant} that expires at {ExpiresAt}", TenantId, scopeToken.ExpiresOn);
        WriteObject(scopeToken.Token);
    }
}
