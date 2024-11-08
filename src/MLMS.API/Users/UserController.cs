using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MLMS.API.Common;
using MLMS.API.Identity;
using MLMS.API.Identity.Responses;
using MLMS.API.Users.Requests;
using MLMS.API.Users.Responses;
using MLMS.Domain.Users;

namespace MLMS.API.Users;

[Route("api/v1/users")]
public class UserController(IUserService userService) : ApiController
{
    /// <response code="404">If the user is not found.</response>
    /// <response code="200">With the user data.</response>
    [Authorize]
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(int id)
    {
        var response = await userService.GetByIdAsync(id);

        return response.Match(u => Ok(u.ToContract()), Problem);
    }
    
    /// <response code="400">If the user data to update is invalid.</response>
    /// <response code="404">If the user is not found or major is not found or department is not found.</response>
    /// <response code="409">if the updated workId is already in use by another user.</response>
    /// <response code="204">Updated successfully.</response>
    [Authorize(Policy = AuthorizationPolicies.SuperAdmin)]
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update(int id, UpdateUserRequest request)
    {
        var response = await userService.UpdateAsync(id, request.ToDomain());

        return response.Match(_ => NoContent(), Problem);
    }
    
    /// <response code="404">If the user is not found.</response>
    /// <response code="204">The user is deleted successfully.</response>
    [Authorize(Policy = AuthorizationPolicies.SuperAdmin)]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await userService.DeleteAsync(id);

        return response.Match(_ => NoContent(), Problem);
    }
}