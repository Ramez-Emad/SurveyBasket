using Microsoft.AspNetCore.Authorization;


namespace Presentation.Filters.Authentication;
public class HasPermissionAttribute(string permission) : AuthorizeAttribute(permission)
{
}
