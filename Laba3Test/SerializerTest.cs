using Laba3.DAL.DTO;
using Laba3;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using lab3.DAL;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestPlatform.TestHost;
namespace Laba3Test
{
    [TestClass]
    public class SerializerTest
    {
        [TestMethod]
        public void GetCatalogDTOTest()
        {
            // Arrange
            var cat = new Catalog();
            // Add some books to the catalog for testing
            cat.AddBook(new Book("Book 1", "Author 1", 1234567890, "Annotation 1", "Summary 1", "Text 1"));
            cat.AddBook(new Book("Book 2", "Author 2", 0987654321, "Annotation 2", "Summary 2", "Text 2"));

            // Act
            var result = GetCatalogDTO(cat);

            // Assert
            Assert.AreEqual(2, result.GetAllBooks.Count);
            Assert.AreEqual("Book 1", result.GetAllBooks[0].name_of_book);
            Assert.AreEqual("Author 1", result.GetAllBooks[0].autor);
            Assert.AreEqual(1234567890, result.GetAllBooks[0].ISBN);
            Assert.AreEqual("Annotation 1", result.GetAllBooks[0].annotation);
            Assert.AreEqual("Summary 1", result.GetAllBooks[0].summary);
            Assert.AreEqual("Text 1", result.GetAllBooks[0].Text);
            Assert.AreEqual("Book 2", result.GetAllBooks[1].name_of_book);
            Assert.AreEqual("Author 2", result.GetAllBooks[1].autor);
            Assert.AreEqual(0987654321, result.GetAllBooks[1].ISBN);
            Assert.AreEqual("Annotation 2", result.GetAllBooks[1].annotation);
            Assert.AreEqual("Summary 2", result.GetAllBooks[1].summary);
            Assert.AreEqual("Text 2", result.GetAllBooks[1].Text);
        }

        [TestMethod]
        public void GetBookDTOTest()
        {
            // Arrange
            var book = new Book("Book 1", "Author 1", 1234567890, "Annotation 1", "Summary 1", "Text 1");

            // Act
            var result = GetBookDTO(book);

            // Assert
            Assert.AreEqual("Book 1", result.name_of_book);
            Assert.AreEqual("Author 1", result.autor);
            Assert.AreEqual(1234567890, result.ISBN);
            Assert.AreEqual("Annotation 1", result.annotation);
            Assert.AreEqual("Summary 1", result.summary);
            Assert.AreEqual("Text 1", result.Text);
            Assert.AreEqual(1, result.count);
            Assert.AreEqual(true, result.flag);
        }


        [TestMethod]
        public void TestSaveToJSON()
        {
            // Arrange
            var cat = new Catalog();
            // Set up the necessary objects for testing

            // Act
            Serializer.SaveToJSON(cat);

            // Assert
            Assert.IsTrue(File.Exists("Catalog"));

        }

        [TestMethod]
        public void TestLoadFromJSON()
        {
            // Arrange
            var cat = new Catalog();

            // Act&Assert
            Serializer.SaveToJSON(cat);
            Serializer.LoadFromJSON();

        }

        [TestMethod]
        public void TestSaveToXML()
        {
            // Arrange
            var cat = new Catalog();


            // Act
            Serializer.SaveToXML(cat);

            // Assert
            Assert.IsTrue(File.Exists("Catalog.xml"));

        }

        [TestMethod]
        public void TestLoadFromXML()
        {
            // Arrange
            var cat = new Catalog();
            // Set up the necessary objects for testing

            // Act&Assert
            Serializer.SaveToXML(cat);
            Serializer.LoadFromXML();

        }
        [TestMethod]
        public void SaveToSQLite_SavesBooksToDatabase_Successfully()
        {
            // Arrange
            Catalog cat = new Catalog();
            var book1 = new Book("Book 1", "Author 1", 1234567890, "Annotation 1", "Summary 1", "Text 1");
            var book2 = new Book("Book 2", "Author 2", 1345678901, "Annotation 2", "Summary 2", "Text 2");
            cat.AddBook(book1);
            cat.AddBook(book2);

            // Act
            Serializer.SaveToSQLite(cat);

            // Assert
            using (var conn = new SqliteConnection("Data Source=catalog.db"))
            {
                conn.Open();
                using (var cmd = new SqliteCommand("SELECT COUNT(*) FROM Catalog", conn))
                {
                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    Assert.AreEqual(2, count, "Number of saved books in the database is incorrect.");
                }
            }
        }

        [TestMethod]
        public void LoadFromSQLite_ShouldLoadDataFromDatabase()
        {
            // Arrange
            using (var conn = new SqliteConnection($"Data Source=catalog.db"))
            {
                conn.Open();
                using (var cmd = new SqliteCommand("CREATE TABLE IF NOT EXISTS Catalog (name_of_book TEXT, author TEXT, ISBN INTEGER, annotation TEXT, summary TEXT, Text TEXT)", conn))
                {
                    cmd.ExecuteNonQuery();
                }
                using (var cmd = new SqliteCommand("INSERT INTO Catalog (name_of_book, author, ISBN, annotation, summary, Text) VALUES ('Book 1', 'Author 1', 1234567890, 'Annotation 1', 'Summary 1', 'Text 1')", conn))
                {
                    cmd.ExecuteNonQuery();
                }
                using (var cmd = new SqliteCommand("INSERT INTO Catalog (name_of_book, author, ISBN, annotation, summary, Text) VALUES ('Book 2', 'Author 2', 9876543210, 'Annotation 2', 'Summary 2', 'Text 2')", conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }

            // Act
            Serializer.LoadFromSQLite();

            // Assert
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                Serializer.LoadFromSQLite();
                string expectedOutput = "Book 1\r\nBook 2\r\n";
                Assert.AreEqual(expectedOutput, sw.ToString());
            }
        }
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
    }
}
