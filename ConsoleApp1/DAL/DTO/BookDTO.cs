using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba3.DAL.DTO
{
    public class BookDTO
    {
        public string name_of_book { get; set; }
        public string autor { get; set; }
        public int ISBN { get; set; }
        public string annotation { get; set; }
        public string summary { get; set; }
        public string Text { get; set; }
        [NonSerialized]
        public int count;
        [NonSerialized]
        public bool flag;
    }
}
