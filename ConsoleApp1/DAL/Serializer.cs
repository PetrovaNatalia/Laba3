using Laba3;
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
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace lab3.DAL
{
    public  class Serializer
    {
        private static CatalogDTO GetCatalogDTO(Catalog cat)
        {
            return new CatalogDTO
            {
                GetAllBooks = cat.GetAllBooks.Select(b => new BookDTO
                {
                    name_of_book = b.name_of_book,
                    autor = b.autor,
                    ISBN = b.ISBN,
                    annotation = b.annotation,
                    summary = b.summary,
                    Text = b.Text
                }).ToList()

            };
        }
        private static BookDTO GetBookDTO(Book book)
        {
            return new BookDTO
            {
                name_of_book = book.name_of_book,
                autor = book.autor,
                ISBN = book.ISBN,
                annotation = book.annotation,
                summary = book.summary,
                Text = book.Text,
                count = book.count,
                flag = book.flag
            };
        }
       
        public static void SaveToJSON(Catalog cat)
        {
            var catDTO = GetCatalogDTO(cat);
            var options = new JsonSerializerOptions();
            string jsonString = JsonSerializer.Serialize(catDTO);
            File.WriteAllText("Catalog", jsonString);
            Console.WriteLine("каталог сохранен");
        }
        public static void LoadFromJSON()
        {
            string jsonString = File.ReadAllText("Catalog");
            var catDTO = JsonSerializer.Deserialize<CatalogDTO>(jsonString);
            foreach (var book in catDTO.GetAllBooks)
            {
                Console.WriteLine(book.name_of_book);
            }      
        }
        public static void SaveToXML(Catalog cat)
        {
            CatalogDTO myObject = GetCatalogDTO(cat);
            XmlSerializer mySerializer = new
            XmlSerializer(typeof(CatalogDTO));
            StreamWriter myWriter = new StreamWriter("Catalog.xml");
            mySerializer.Serialize(myWriter, myObject);
            myWriter.Close();
            Console.WriteLine("каталог сохранен");
        }
        public static void LoadFromXML()
        {
            var mySerializer = new XmlSerializer(typeof(CatalogDTO));
            using var myFileStream = new FileStream("Catalog.xml", FileMode.Open);
            var myObject = (CatalogDTO)mySerializer.Deserialize(myFileStream);
            foreach (var book in myObject.GetAllBooks)
            {
                Console.WriteLine(book.name_of_book);
            }
            myFileStream.Close();
        }
        public static void SaveToSQLite(Catalog cat)
        {
            var catDTO = GetCatalogDTO(cat);
            
            using (var conn = new SqliteConnection("Data Source=catalog.db"))
            {
               
                conn.Open();
                ;
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = "CREATE TABLE IF NOT EXISTS Catalog (name_of_book TEXT, author TEXT, ISBN VALUE, annotation TEXT, summary TEXT, Text TEXT )";
                    command.ExecuteNonQuery();
                    
                    foreach (BookDTO book in catDTO.GetAllBooks)
                    {
                        command.CommandText = "INSERT INTO Catalog (name_of_book, author, ISBN, annotation, summary, Text) VALUES (@name_of_book, @autor, @ISBN, @annotation, @summary, @Text)";
                        command.Parameters.Clear();
                        
                        command.Parameters.AddWithValue("@name_of_book", book.name_of_book);
                        command.Parameters.AddWithValue("@autor", book.autor);
                        command.Parameters.AddWithValue("@ISBN", book.ISBN);
                        command.Parameters.AddWithValue("@annotation", book.annotation);
                        command.Parameters.AddWithValue("@summary", book.summary);
                        command.Parameters.AddWithValue("@Text", book.Text);

                        command.ExecuteNonQuery();
                    }
                    Console.WriteLine("данные сохранены");
                }
            }

        }
        public static void LoadFromSQLite()
        {
            using (var conn = new SqliteConnection("Data Source=catalog.db"))
            {
                Catalog cat = new Catalog();
                conn.Open();
                using (var cmd = new SqliteCommand("SELECT * FROM Catalog", conn))
                {
                    using (SqliteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name_of_book = reader["name_of_book"].ToString();
                            string autor = reader["author"].ToString();
                            int ISBN = (int)(long)reader["ISBN"];
                            string annotation = reader["annotation"].ToString();
                            string summary = reader["summary"].ToString();
                            string Text = reader["Text"].ToString();
                            cat.AddBook(new Book(name_of_book, autor, ISBN, annotation, summary, Text));
                        }
                    }
                }
                foreach (var book in cat.GetAllBooks)
                {
                    Console.WriteLine(book.name_of_book);
                }
            }
        }
    }
}
