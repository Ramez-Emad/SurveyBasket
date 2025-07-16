﻿using Microsoft.AspNetCore.Authorization;
using Shared.Abstractions.Consts;


namespace Presentation.Filters.Authentication;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (context.User.Identity is not { IsAuthenticated: true } ||
            !context.User.Claims.Any(x => x.Value == requirement.Permission && x.Type == Permissions.Type))
            return Task.CompletedTask;

        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}