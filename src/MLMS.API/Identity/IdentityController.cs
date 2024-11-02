using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MLMS.API.Common;
using MLMS.API.Identity.Requests;
using MLMS.API.Identity.Responses;
using MLMS.Domain.Identity.Interfaces;

namespace MLMS.API.Identity;

[Route("api/v1/identity")]
public class IdentityController(IIdentityService identityService) : ApiController
{
    /// <response code="400">If the provided work ID is empty or password is less than 8 characters and missing a lowercase, uppercase, digit, or a special character</response>
    /// <response code="401">If the provided credentials are invalid.</response>
    /// <response code="200">If the user is successfully authenticated, access token and refresh token are returned in the response.</response>
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var response = await identityService.AuthenticateAsync(request.ToLoginCredentials());
        
        return response.Match(u => Ok(u.ToContract()), Problem);
    }
    
    /// <response code="400">If the provided work ID is empty or password is less than 8 characters and missing a lowercase, uppercase, digit, or a special character or email is not a proper format or phone number is not a proper format or birthday is in the future or role is an admin.</response>
    /// <response code="401">If not authenticated.</response>
    /// <response code="403">If not authorized (not an admin).</response>
    /// <response code="409">If the provided work ID has already registered.</response>
    /// <response code="404">If the major within department or department is not found is not found.</response>
    /// <response code="204">If the new user is registered successfully.</response>
    [Authorize(Policy = AuthorizationPolicies.SuperAdmin)]
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var response = await identityService.RegisterAsync(request.ToDomainUser());
        
        return response.Match(_ => NoContent(), Problem);
    }

    /// <response code="404">If the user is not found.</response>
    /// <response code="200">With the user data.</response>
    [Authorize]
    [HttpGet("users/{id:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUser(int id)
    {
        var response = await identityService.GetUserAsync(id);

        return response.Match(u => Ok(u.ToContract()), Problem);
    }
    
    /// <response code="401">If the refresh token is invalid or expired.</response>
    /// <response code="200">With new authentication token (access and refresh tokens).</response>
    [HttpPost("refresh-token")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
    {
        var response = await identityService.RefreshTokenAsync(request.RefreshToken);
        
        return response.Match(u => Ok(u.ToContract()), Problem);
    }
    
    /// <response code="401">If the user is not authorized or If the refresh token is invalid or expired.</response>
    /// <response code="204">The refresh token is revoked successfully.</response>
    [Authorize]
    [HttpPost("revoke-refresh-token")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RevokeRefreshToken(RevokeRefreshTokenRequest request)
    {
        var response = await identityService.RevokeRefreshTokenAsync(request.RefreshToken);
        
        return response.Match(_ => NoContent(), Problem);
    }
    
    /// <response code="409">If the password is not for the current user.</response>
    /// <response code="401">If the user is not authorized</response>
    /// <response code="400">If the password is weak (we know already).</response>
    /// <response code="204">The refresh token is revoked successfully.</response>
    [Authorize]
    [HttpPost("change-password")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
    {
        var response = await identityService.ChangePasswordAsync(request.CurrentPassword, request.NewPassword);
        
        return response.Match(_ => NoContent(), Problem);
    }
    
    /// <response code="404">User with given Work ID is not found.</response>
    /// <response code="204">The refresh token is revoked successfully.</response>
    [HttpPost("forgot-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
    {
        var response = await identityService.ForgotPasswordAsync(request.WorkId);
        
        return response.Match(_ => NoContent(), Problem);
    }
    
    /// <response code="400">If password is weak.</response>
    /// <response code="401">If the provided token is invalid or expired.</response>
    /// <response code="404">If user with work id is not found.</response>
    /// <response code="204">if the password is reset successfully.</response>
    [HttpPost("reset-forgotten-password")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ResetForgottenPassword(ResetForgottenPasswordRequest request)
    {
        var response = await identityService.ResetPasswordAsync(request.WorkId, request.NewPassword, request.Token);
        
        return response.Match(_ => NoContent(), Problem);
    }
}