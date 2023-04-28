using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMarket.DataBase;
using OnlineMarket.Features.Roles.Models;
using OnlineMarket.Features.Roles.Views;
using OnlineMarket.Features.Users.Models;
using StudentUptBackend.Database;

namespace OnlineMarket.Features.Roles;

[ApiController]
[Route("roles")]
public class RolesController : ControllerBase
{
    private readonly IRepository<RoleModel> repository;
    private readonly IRepository<UserModel> userRepo;
    
    public RolesController(IRepository<RoleModel> repository, IRepository<UserModel> userRepo)
    {
        this.repository = repository;
        this.userRepo = userRepo;

    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<RoleResponse>> Add([FromBody]RoleRequest request)
    {
        var role = new RoleModel
        {
            Name = request.Name,
            Description = request.Description
        };
        
        role = await repository.AddAsync(role);

        var res = new RoleResponse
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description
        };

        return Ok(res);
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RoleResponse>>> Get()
    {
        var roles = await repository.GetAsync();
        
        var ret = roles.Select(role => new RoleResponse
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description
            });

        return Ok(ret);
    }

    
    
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RoleResponse>> Get([FromRoute]string id)
    {
        var role = await repository.GetAsync(id);
        if (role is null) return NotFound("role not found");
        
        var res = new RoleResponse
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description
        };

        return Ok(res);
    }

    [HttpPatch("{Id}")]
    public async Task<ActionResult<RoleResponse>> Update([FromRoute]string Id, RoleRequestForPatch request)
    {
        if (request.Name == "string" || request.Name == "") request.Name = null;
        if (request.Description == "string" || request.Description == "") request.Description = null;

        var role = await repository.UpdateAsync(Id, request);
        if (role is null) return NotFound("role not found");

        var ret = new RoleResponse
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description
        };

        return ret;
    }

    [HttpDelete("{Id}")]
    public async Task<ActionResult<RoleResponse>> Delete([FromRoute] string Id)
    {
        var role = await repository.DeleteAsync(Id);
        if (role is null) return NotFound("role not found");

        return Ok($"role with id={role.Id} was successfully deleted");
    }

}