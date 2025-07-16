using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.Abstractions.Consts;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolesController(IRoleService _roleService ) : ControllerBase
{
}
