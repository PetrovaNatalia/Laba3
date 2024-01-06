using lab3.DAL;
using Laba3.DAL.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;
using System.Text.Json;
/*Информация о книге включает название, имя автора, жанры, дату публикации, аннотацию, ISBN.
Из главного меню программы пользователь, посредством выбора одного из пронумерованных пунктов, может производить следующие операции:
-добавление книги в каталог
-выборку информации о конкретной книге по:
--названию или его фрагменту
--имени автора 
--ISBN
--ключевым словам
-выход.
Поиск по ключевым словам должен возвращать краткие описания книг (без аннотации) в таком порядке,
чтобы в начале выборки оказывались книги c наибольшим количеством найденных ключевых слов, 
а в конце – с наименьшим количеством. Если ключевое слово найдено в аннотации, это должно быть отражено в выборке.
*/
namespace Laba3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Catalog catalog = new Catalog();
            Program.Menu();
            bool flag = true;
            while (flag)
            {
                switch (int.Parse(Console.ReadLine()))
                {
                    case 1:
                        {

                            Console.WriteLine("Введите название книги: ");
                            string name_of_book = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(name_of_book))
                            {
                                throw new ArgumentNullException(nameof(name_of_book));
                            }


                            Console.WriteLine("Введите автора книги: ");
                            string autor = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(autor))
                            {
                                throw new ArgumentNullException(nameof(autor));
                            }


                            Console.WriteLine("Введите ISBN книги: ");
                            int ISBN = int.Parse(Console.ReadLine());


                            Console.WriteLine("Введите аннотацию книги: ");
                            string annotation = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(annotation))
                            {
                                throw new ArgumentNullException(nameof(annotation));
                            }

                            Console.WriteLine("Введите краткое содержание книги: ");
                            string summary = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(summary))
                            {
                                throw new ArgumentNullException(nameof(summary));
                            }

                            Console.WriteLine("Введите сожержание книги: ");
                            string Text = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(Text))
                            {
                                throw new ArgumentNullException(nameof(Text));
                            }

                            Book book = new Book(name_of_book, autor, ISBN, annotation, summary, Text);
                            catalog.AddBook(book);

                            break;
                        }
                    case 2:
                        {
                            Console.WriteLine("Введите название книги: ");
                            string name_of_book = Console.ReadLine();
                            catalog.SearchByNameOfBook(name_of_book);

                            break;
                        }
                    case 3:
                        {
                            Console.WriteLine("Введите фрагмент книги: ");
                            string fragment = Console.ReadLine();
                            catalog.SearchByFragment(fragment);

                            break;
                        }
                    case 4:
                        {
                            Console.WriteLine("Введите ключевое слово: ");
                            string KeyWord = Console.ReadLine();
                            catalog.SearchByKeyWord(KeyWord);

                            break;
                        }
                    case 5:
                        {
                            Console.WriteLine("Введите имя автора: ");
                            string autor = Console.ReadLine();
                            catalog.SearchByAutor(autor);

                            break;
                        }
                    case 6:
                        {
                            Console.WriteLine("Введите ISBN книги: ");
                            int ISBN = int.Parse(Console.ReadLine());
                            catalog.SearchByISBN(ISBN);

                            break;
                        }
                    case 7:
                        {
                            Serializer.SaveToJSON(catalog);
                            break;
                        }
                    case 8:
                        {
                            Serializer.LoadFromJSON();
                            break;
                        }
                    case 9:
                        {
                            Serializer.SaveToXML(catalog);
                            break;
                        }
                    case 10:
                        {
                            Serializer.LoadFromXML();
                            break;
                        }
                    case 11:
                        {
                            Serializer.SaveToSQLite(catalog); 
                            break;
                        }
                    case 12:
                        {
                            Serializer.LoadFromSQLite(); 
                            break;
                        }
                    case 13:
                        {
                            flag = false;
                            break;
                        }

                }
                Program.Menu();
            }
        }
        public static void Menu()
        {
            Console.WriteLine("Выбирите действие :");
            Console.WriteLine("1. добавить книгу в каталог");
            Console.WriteLine("2. поиск по названию");
            Console.WriteLine("3. поиск по фрагменту");
            Console.WriteLine("4. поиск по ключевым словам");
            Console.WriteLine("5. поиск по имени автора");
            Console.WriteLine("6. поиск по ISBN");
            Console.WriteLine("7. сохранить  JSON");
            Console.WriteLine("8. открыть  JSON");
            Console.WriteLine("9. сохранить XML");
            Console.WriteLine("10. открыть XML");
            Console.WriteLine("11. сохранить SQLite");
            Console.WriteLine("12. открыть SQLite");
            Console.WriteLine("13. выход ");
        }
    }
}