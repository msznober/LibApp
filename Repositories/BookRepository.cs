using LibApp.Data;
using LibApp.Interfaces;
using LibApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibApp.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;
        public BookRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Book> GetBooks()
        {
            return _context.Books;
        }

        Book GetBookById(int id) => _context.Books.Find(id);

        void AddBook(Book book) => _context.Books.Add(book);

        void DeleteBook(int id) => _context.Books.Remove(GetBookById(id));

        void UpdateBook(Book book) => _context.Books.Update(book);

        void Save() => _context.SaveChanges();
    }
}
