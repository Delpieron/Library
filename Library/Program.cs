using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Library
{
    class ProgramXML
    {
        static void Main(string[] args)
        {
            var books = getBooks();
            Console.WriteLine("przed dodaniem:");
            books.ForEach(b => Console.WriteLine(b));
            Add(new Books
            {
                Name = "PowerRangers",
                Cat = "Wszyscy",
                Status = "dostepna"
            });
            Console.WriteLine("Po dodaniu:");
            var booksA = getBooks();
            booksA.ForEach(b => Console.WriteLine(b));

            Console.WriteLine("po usunieciu");
            Delete(2);
            var booksD = getBooks();
            booksD.ForEach(b => Console.WriteLine(b));
            Console.Read();


        }
        private static List<Books> getBooks()
        {
            XDocument document = XDocument.Load("book.xml");
            return document
                .Element("root")
                .Elements()
                .Select(b => new Books
                {
                    Id = int.Parse(b.Attribute(nameof(Books.Id)).Value),
                    Name = b.Attribute(nameof(Books.Name)).Value,
                    Cat = b.Attribute(nameof(Books.Cat)).Value,
                    Status = b.Attribute(nameof(Books.Status)).Value,
                })
                .ToList();
        }
        private static void Add(Books item)
        {
            XDocument document = XDocument.Load("book.xml");
            int id = GetLastId();
            XElement element = new XElement("Book",
                new XAttribute(nameof(Books.Id), ++id),
                new XAttribute(nameof(Books.Name), item.Name),
                new XAttribute(nameof(Books.Cat), item.Cat),
                new XAttribute(nameof(Books.Status), item.Status)
                );
            document.Element("root").Add(element);
            document.Save("book.xml");
        }
        private static int GetLastId()
        {
            return getBooks().Count != 0 ? getBooks().Max(i => i.Id) : 0;
        }
        private static bool Delete(int id)
        {
            XDocument document = XDocument.Load("book.xml");
            var item = getBooks().FirstOrDefault(a => a.Id == id);
            if (item == null)
                return false;
            document.Element("root").Elements()
                .Where(a => a.Attribute("Id").Value == id.ToString())
                .Remove();
            document.Save("book.xml");
            return true;


        }
    }
}
