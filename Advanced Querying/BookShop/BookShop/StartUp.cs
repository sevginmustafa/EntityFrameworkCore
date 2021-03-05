namespace BookShop
{
    using Data;
    using Initializer;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            //1. Age Restriction
            //string ageRestriction = Console.ReadLine();
            //Console.WriteLine(GetBooksByAgeRestriction(db, ageRestriction));

            //2. Golden Books
            //Console.WriteLine(GetGoldenBooks(db));

            //3. Books by Price
            //Console.WriteLine(GetBooksByPrice(db));

            //4.Not Released In
            //int year = int.Parse(Console.ReadLine());
            //Console.WriteLine(GetBooksNotReleasedIn(db, year));

            //5. Book Titles by Category
            //string categories = Console.ReadLine();
            //Console.WriteLine(GetBooksByCategory(db, categories));

            //6. Released Before Date
            //string date = Console.ReadLine();
            //Console.WriteLine(GetBooksReleasedBefore(db, date));

            //7. Author Search
            //string input = Console.ReadLine();
            //Console.WriteLine(GetAuthorNamesEndingIn(db, input));

            //8. Book Search
            //string input = Console.ReadLine();
            //Console.WriteLine(GetBookTitlesContaining(db, input));

            //9. Book Search by Author
            //string input = Console.ReadLine();
            //Console.WriteLine(GetBooksByAuthor(db, input));  

            //10. Count Books
            //int length = int.Parse(Console.ReadLine());
            //Console.WriteLine(CountBooks(db, length));

            //11. Total Book Copies
            //Console.WriteLine(CountCopiesByAuthor(db));      

            //12. Profit by Category
            //Console.WriteLine(GetTotalProfitByCategory(db));

            //13. Most Recent Books
            //Console.WriteLine(GetMostRecentBooks(db));

            //14. Increase Prices
            //GetMostRecentBooks(db);

            //15. Remove Books
            //Console.WriteLine(RemoveBooks(db));
        }


        //1. Age Restriction
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var booksInfo = context.Books
                .AsEnumerable()
                .Where(x => x.AgeRestriction.ToString().ToLower() == command.ToLower())
                .Select(x => x.Title)
                .OrderBy(x => x)
                .ToList();

            return string.Join(Environment.NewLine, booksInfo);
        }


        //2. Golden Books
        public static string GetGoldenBooks(BookShopContext context)
        {
            var booksInfo = context.Books
                .AsEnumerable()
                .Where(x => x.EditionType.ToString() == "Gold" && x.Copies < 5000)
                .Select(x => x.Title)
                .ToList();

            return string.Join(Environment.NewLine, booksInfo);
        }


        //3. Books by Price
        public static string GetBooksByPrice(BookShopContext context)
        {
            var booksInfo = context.Books
                    .Where(x => x.Price > 40)
                    .Select(x => new
                    {
                        x.Title,
                        x.Price
                    })
                    .OrderByDescending(x => x.Price)
                    .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var book in booksInfo)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }


        //4. Not Released In
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var booksInfo = context.Books
                   .Where(x => x.ReleaseDate.Value.Year != year)
                   .Select(x => x.Title)
                   .ToList();

            return string.Join(Environment.NewLine, booksInfo);
        }


        //5. Book Titles by Category
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            List<string> categories = input.ToLower().Split().ToList();

            var booksInfo = context.Books
                       .Where(x => categories.Contains(x.BookCategories.Select(x => x.Category.Name).FirstOrDefault().ToLower()))
                       .Select(x => x.Title)
                       .OrderBy(x => x)
                       .ToList();

            return string.Join(Environment.NewLine, booksInfo);
        }


        //6. Released Before Date
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var booksInfo = context.Books
                   .Where(x => x.ReleaseDate < DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture))
                   .OrderByDescending(x => x.ReleaseDate)
                   .Select(x => new
                   {
                       x.Title,
                       x.EditionType,
                       x.Price
                   })
                   .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var book in booksInfo)
            {
                sb.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }


        //7. Author Search
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authorsInfo = context.Authors
                .Where(x => x.FirstName.EndsWith(input))
                .Select(x => x.FirstName + " " + x.LastName)
                .OrderBy(x => x)
                .ToList();

            return string.Join(Environment.NewLine, authorsInfo);
        }


        //8. Book Search
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var booksInfo = context.Books
                .Where(x => x.Title.ToLower().Contains(input.ToLower()))
                .Select(x => x.Title)
                .OrderBy(x => x)
                .ToList();

            return string.Join(Environment.NewLine, booksInfo);
        }


        //9. Book Search by Author
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var booksInfo = context.Books
                .Where(x => x.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .Select(x => new
                {
                    x.Title,
                    FullName = x.Author.FirstName + " " + x.Author.LastName
                })
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var book in booksInfo)
            {
                sb.AppendLine($"{book.Title} ({book.FullName})");
            }

            return sb.ToString().TrimEnd();
        }


        //10. Count Books
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            return context.Books.Where(x => x.Title.Length > lengthCheck).Count();
        }


        //11. Total Book Copies
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authorsInfo = context.Authors
                .Select(x => new
                {
                    FullName = x.FirstName + " " + x.LastName,
                    BookCopiesCount = x.Books.Sum(x => x.Copies)
                })
                .OrderByDescending(x => x.BookCopiesCount)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var authorInfo in authorsInfo)
            {
                sb.AppendLine($"{authorInfo.FullName} - {authorInfo.BookCopiesCount}");
            }

            return sb.ToString().TrimEnd();
        }


        //12. Profit by Category
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var categoriesInfo = context.Categories
                .Select(x => new
                {
                    CategoryName = x.Name,
                    TotalProfit = x.CategoryBooks.Sum(x => x.Book.Copies * x.Book.Price)
                })
                .OrderByDescending(x => x.TotalProfit)
                .ThenBy(x => x.CategoryName)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var category in categoriesInfo)
            {
                sb.AppendLine($"{category.CategoryName} ${category.TotalProfit:f2}");
            }

            return sb.ToString().TrimEnd();
        }


        //13. Most Recent Books
        public static string GetMostRecentBooks(BookShopContext context)
        {
            var categoriesInfo = context.Categories
                .Select(x => new
                {
                    CategoryName = x.Name,
                    RecentBooks = x.CategoryBooks
                    .OrderByDescending(x => x.Book.ReleaseDate)
                    .Take(3)
                    .Select(x => new
                    {
                        x.Book.Title,
                        x.Book.ReleaseDate.Value.Year
                    })
                })
                .OrderBy(x => x.CategoryName)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var category in categoriesInfo)
            {
                sb.AppendLine($"--{category.CategoryName}");
                foreach (var book in category.RecentBooks)
                {
                    sb.AppendLine($"{book.Title} ({book.Year})");
                }
            }

            return sb.ToString().TrimEnd();
        }


        //14. Increase Prices
        public static void IncreasePrices(BookShopContext context)
        {
            var booksInfo = context.Books.Where(x => x.ReleaseDate.Value.Year < 2010).ToList();

            booksInfo.ForEach(x => x.Price += 5);

            context.SaveChanges();
        }


        //15. Remove Books
        public static int RemoveBooks(BookShopContext context)
        {
            var booksToRemoveFromBooksCategories = context.BooksCategories.Where(x => x.Book.Copies < 4200).ToList();

            context.BooksCategories.RemoveRange(booksToRemoveFromBooksCategories);

            var booksToRemoveFromBooks = context.Books.Where(x => x.Copies < 4200).ToList();

            context.Books.RemoveRange(booksToRemoveFromBooks);

            context.SaveChanges();

            return booksToRemoveFromBooks.Count;
        }
    }
}
