using Microsoft.AspNetCore.Authorization;


namespace Presentation.Filters.Authentication;
public class PermissionRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}