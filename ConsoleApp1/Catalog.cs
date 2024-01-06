
using Laba3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Laba3
{
    public class Catalog
    {
        private List<Book> catalog = new List<Book>();
        
        public IEnumerable<Book> GetAllBooks
        {
            get
            {
                return catalog;
            }
        }
        public void AddBook(Book book)
        {
            catalog.Add(book);
        }

        public void SearchByNameOfBook(string name_of_book)
        {
            Book found = catalog.Find(book => book.name_of_book == name_of_book);
            if (found != null)
            {
                Console.WriteLine("Название:{0}, автор:{1}, ISBN:{2}, аннотация:{3}, кратное содержание:{4}, содержание:{5}", found.name_of_book, found.autor, found.ISBN, found.annotation, found.summary, found.Text);

            }
            else
            {
                Console.WriteLine("книги с таким названием в каталоге не найдено");

            }
        }

        public void SearchByAutor(string autor)
        {
            if (catalog.FindAll(book => book.autor == autor).Count != 0)
            {
                foreach (Book book in catalog.FindAll(book => book.autor == autor))
                {
                    Console.WriteLine("Название:{0}, автор:{1}, ISBN:{2}, аннотация:{3}, кратное содержание:{4}, содержание:{5}", book.name_of_book, book.autor, book.ISBN, book.annotation, book.summary, book.Text);
                }
            }
            else
            {
                Console.WriteLine("книг этого автора в каталоге не найдено");
            }
        }

        public void SearchByISBN(int ISBN)
        {
            Book found = catalog.Find(book => book.ISBN == ISBN);

            if (found != null)
            {
                Console.WriteLine("Название:{0}, автор:{1}, ISBN:{2}, аннотация:{3}, кратное содержание:{4}, содержание:{5}", found.name_of_book, found.autor, found.ISBN, found.annotation, found.summary, found.Text);

            }
            else
            {
                Console.WriteLine("книги с таким ISBN в каталоге не найдено");

            }
        }

        public void SearchByFragment(string fragment)
        {
            if (catalog.Count != 0)
            {
                foreach (Book book in catalog)
                {
                    bool b = book.Text.Contains(fragment);
                    if (b)
                    {
                        Console.WriteLine("Название:{0}, автор:{1}, ISBN:{2}, аннотация:{3}, кратное содержание:{4}, содержание:{5}", book.name_of_book, book.autor, book.ISBN, book.annotation, book.summary, book.Text);
                    }

                    else
                    {
                        Console.WriteLine("книги, содержащей такой фрагмент, в каталоге не найдено");
                    }
                }
            }
            else
            {
                Console.WriteLine("книги, содержащей такой фрагмент, в каталоге не найдено");
            }
        }
        public void SearchByKeyWord(string KeyWord)
        {
            List<Book> found = new List<Book>();
            List<Book> Sorting = new List<Book>();
            foreach (Book book in catalog)
            {
                bool bT = book.Text.Contains(KeyWord);
                bool bA = book.annotation.Contains(KeyWord);
                if (bT || bA)
                {
                    found.Add(book);
                }
                else
                {
                    continue;
                }
            }
            ; if (found.Count == 0)
            {
                Console.WriteLine("книги, содержащей такое ключевое слово, в каталоге не найдено");
            }
            if (found.Count > 0)
            {
                foreach (Book book in found)
                {
                    bool bT = book.Text.Contains(KeyWord);
                    bool bA = book.annotation.Contains(KeyWord);

                    if (bT && bA)
                    {
                        book.count += Regex.Matches(book.Text, KeyWord, RegexOptions.IgnoreCase).Count;
                        book.count += Regex.Matches(book.annotation, KeyWord, RegexOptions.IgnoreCase).Count;
                    }
                    else
                    {
                        if (bT)
                        {
                            book.count += Regex.Matches(book.Text, KeyWord, RegexOptions.IgnoreCase).Count;
                            book.flag = false;

                        }
                        else
                        {
                            book.count += Regex.Matches(book.annotation, KeyWord, RegexOptions.IgnoreCase).Count;
                        }
                    }

                }
                found.Sort((x, y) => x.count.CompareTo(y.count));
                found.Reverse();
                foreach (Book book in found)
                {
                    if (book.flag == true)
                    {
                        Console.WriteLine("(ключевые слова найдеты в аннотации) Название:{0}, автор:{1}, ISBN:{2}, кратное содержание:{3}", book.name_of_book, book.autor, book.ISBN, book.summary);
                    }
                    else
                    {
                        Console.WriteLine("Название:{0}, автор:{1}, ISBN:{2}, кратное содержание:{3}", book.name_of_book, book.autor, book.ISBN, book.summary);
                    }
                }
            }

        }
    }
}
