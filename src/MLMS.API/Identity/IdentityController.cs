using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MLMS.API.Common;
using MLMS.API.Identity.Requests;
using MLMS.API.Identity.Responses;
using MLMS.Domain.Identity.Interfaces;

namespace MLMS.API.Identity;

[Route("v1/identity")]
public class IdentityController(IIdentityService identityService) : ApiController
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var response = await identityService.AuthenticateAsync(request.ToLoginCredentials());
        
        return response.Match(u => Ok(u.ToContract()), Problem);
    }
    
    [Authorize(Policy = AuthorizationPolicies.SuperAdmin)]
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var response = await identityService.RegisterAsync(request.ToDomainUser());
        
        return response.Match(_ => Ok(), Problem);
    }

    [Authorize]
    [HttpGet("users/{id:int}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var response = await identityService.GetUserAsync(id);

        return response.Match(Ok, Problem);
    }
    
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
    {
        var response = await identityService.RefreshTokenAsync(request.RefreshToken);
        
        return response.Match(u => Ok(u.ToContract()), Problem);
    }
    
    [Authorize]
    [HttpPost("revoke-refresh-token")]
    public async Task<IActionResult> RevokeRefreshToken(RevokeRefreshTokenRequest request)
    {
        var response = await identityService.RevokeRefreshTokenAsync(request.RefreshToken);
        
        return response.Match(_ => NoContent(), Problem);
    }
    
    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
    {
        var response = await identityService.ChangePasswordAsync(request.CurrentPassword, request.NewPassword);
        
        return response.Match(_ => NoContent(), Problem);
    }
    
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
    {
        var response = await identityService.ForgotPasswordAsync(request.WorkId);
        
        return response.Match(_ => NoContent(), Problem);
    }
    
    [HttpPost("reset-forgotten-password")]
    public async Task<IActionResult> ResetPassword(ResetForgottenPasswordRequest request)
    {
        var response = await identityService.ResetPasswordAsync(request.WorkId, request.NewPassword, request.Token);
        
        return response.Match(_ => NoContent(), Problem);
    }
}