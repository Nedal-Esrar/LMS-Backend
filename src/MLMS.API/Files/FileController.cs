using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MLMS.API.Common;
using MLMS.API.Files.Requests;
using MLMS.Domain.Files;

namespace MLMS.API.Files;

[Route("api/v1/files")]
public class FileController(IFileService fileService, IWebHostEnvironment webHostEnvironment) : ApiControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Upload([FromForm] UploadFileRequest request)
    {
        await using var fileContentStream = request.File.OpenReadStream();

        var result = await fileService.UploadAsync(fileContentStream, request.File.ToDomain());

        return result.Match(id => Ok(new { FileId = id }), Problem);
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await fileService.DeleteAsync(id);

        return result.Match(_ => NoContent(), Problem);
    }
}