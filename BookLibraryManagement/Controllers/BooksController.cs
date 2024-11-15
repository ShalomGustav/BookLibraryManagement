﻿using BookLibraryManagement.Commands;
using BookLibraryManagement.Models;
using BookLibraryManagement.Queries;
using BookLibraryManagement.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> GetBookAsync(Guid id)
    {
        var result = await _mediator.Send(new GetBookByIdQuery(id));
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBookAsync([FromBody] BookModelDto bookModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var model = BookModelDto.CreateBook(bookModel);
        var result = await _mediator.Send(new CreateBookCommand(model));
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBookAsync([FromRoute] Guid id, [FromQuery] string title, string genre, int publishedYear)
    {
        var existingBook = await _bookServices.GetBookByIdAsync(id);
        if (existingBook == null)
        {
            throw new NullReferenceException();
        }

        var result = await _mediator.Send(new UpdateBookCommand(id, title, genre, publishedYear));
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<bool> DeleteBookAsync(Guid id)
    {
        var result = await _mediator.Send(new DeleteBookCommand(id));
        return result;
    }
    
}
