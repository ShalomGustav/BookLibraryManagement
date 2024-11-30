using BookLibraryManagement.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookLibraryManagement.Controllers;

[Route("api/authors")]
[ApiController]
public class AuthorsController : Controller
{
    private readonly IMediator _mediator;
    public AuthorsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAuthorsAsync()
    {
        var result = await _mediator.Send(new GetAllAuthorsQuery());
        return Ok(result);
    }
}
