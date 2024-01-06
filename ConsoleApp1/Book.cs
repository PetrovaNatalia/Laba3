using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Laba3
{
    public class Book
    {
        public string name_of_book{get; private set;}
        public string autor { get; private set; }
        public int ISBN { get; private set; }
        public string annotation { get; private set; }
        public string summary { get; private set; }
        public string Text { get; private set; }
        public int count = 0;
        public bool flag = true;

        public Book(string name_of_book, string autor, int ISBN, string annotation, string summary, string Text)
        {
            this.name_of_book = name_of_book;
            this.autor = autor;
            this.ISBN = ISBN;
            this.annotation = annotation;
            this.summary = summary;
            this.Text = Text;
        }
    }
}
