namespace ServiceAbstraction.Contracts.Roles;

public record RoleRequest(
    string Name,
    IList<string> Permissions
);