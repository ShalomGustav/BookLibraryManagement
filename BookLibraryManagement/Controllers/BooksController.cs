using BookLibraryManagement.Commands;
using BookLibraryManagement.Models;
using BookLibraryManagement.Queries;
using BookLibraryManagement.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BookLibraryManagement.Controllers;

[Route("api/books")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly BookServices _bookServices;
    private readonly IMediator _mediator;

    public BooksController(BookServices bookServices, IMediator mediator)
    {
        _bookServices = bookServices;
        _mediator = mediator;
    }

    [HttpGet("test-error")]
    public IActionResult TestError()
    {
        throw new Exception("Тестовое исключение");
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBooksAsync()
    {
        var result = await _mediator.Send(new GetAllBooksQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBookByIdAsync([Required] Guid id, CancellationToken ctx)
    {
        var result = await _mediator.Send(new GetBookByIdQuery(id), ctx);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBookAsync([FromBody] BookModelDto bookModel, CancellationToken ctx)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var model = BookModelDto.CreateBook(bookModel);
        var result = await _mediator.Send(new CreateBookCommand(model), ctx);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBookAsync([FromRoute] [Required] Guid id, [FromQuery] string title, string genre, int publishedYear, CancellationToken ctx)
    {
        var existingBook = await _bookServices.GetBookByIdAsync(id, ctx);
        if (existingBook == null)
        {
            throw new NullReferenceException();
        }

        await _mediator.Send(new UpdateBookCommand(id, title, genre, publishedYear));

        var result = _mediator.Send(new GetBookByIdQuery(id), ctx);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<bool> DeleteBookAsync([Required] Guid id, CancellationToken ctx)
    {
        var result = await _mediator.Send(new DeleteBookCommand(id), ctx);
        return result;
    }
}
