using AutoMapper;
using LibApp.Data;
using LibApp.Dtos;
using LibApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibApp.Interfaces;
using LibApp.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Web.Http;
using System.Net;
using HttpDeleteAttribute = Microsoft.AspNetCore.Mvc.HttpDeleteAttribute;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using HttpPutAttribute = Microsoft.AspNetCore.Mvc.HttpPutAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace LibApp.Controllers.Api
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        public BooksController(IMapper mapper, IBookRepository bookRepository)
        {

            _mapper = mapper;
            _bookRepository = bookRepository;
        }

        // GET api/books/
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public IEnumerable<BookDto> GetBooks(string query = null)
        {
            var booksQuery = _bookRepository.BooksWhere();
            if (!String.IsNullOrWhiteSpace(query))
            {
                booksQuery = booksQuery.Where(b => b.Name.Contains(query));
            }

            return booksQuery.ToList().Select(_mapper.Map<Book, BookDto>);
        }
        // GET /api/books/{id}
        [Microsoft.AspNetCore.Mvc.HttpGet("{id}")]
        public async Task<IActionResult> GetBooks(int id)
        {
            Console.WriteLine("START REQUEST");
            var book = _bookRepository.AsyncGetBookById(id);
            await Task.Delay(2000);
            if (book == null)
            {
                return NotFound();
            }

            Console.WriteLine("END REQUEST");
            return Ok(_mapper.Map<CustomerDto>(book));
        }

        // POST /api/books
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public BookDto CreateBook(BookDto bookDto)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var book = _mapper.Map<Book>(bookDto);
            _bookRepository.AddBook(book);
            _bookRepository.Save();
            bookDto.Id = book.Id;

            return bookDto;
        }
        // PUT /api/books/{id}
        [Microsoft.AspNetCore.Mvc.HttpPut("{id}")]
        public void UpdateBook(int id, BookDto bookDto)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var bookInDb = _bookRepository.GetBookById(id);
            if (bookInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            _mapper.Map(bookDto, bookInDb);
            _bookRepository.Save();
        }

        // DELETE /api/books
        [Microsoft.AspNetCore.Mvc.HttpDelete("{id}")]
        public void DeleteBook(int id)
        {
            var bookInDb = _bookRepository.GetBookById(id);
            if (bookInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            _bookRepository.DeleteBook(id);
            _bookRepository.Save();
        }

        private readonly IMapper _mapper;
        private readonly IBookRepository _bookRepository;
    }
}