using System;
using System.IO;
using System.Linq;
using Lab_2.Contracts;
using Lab_2.Domain;
using Lab_2.Repositories;
using Lab_2.Services;

namespace Lab_2
{
    /// <summary>
    /// Entry point for the Book Recommendation Program.
    /// Handles file loading, menus, login flow, ratings, and recommendations.
    /// </summary>
    internal class Program
    {
        private static readonly IBookRepository bookRepo = new BookRepository();
        private static readonly IMemberRepository memberRepo = new MemberRepository();
        private static readonly IRatingRepository ratingRepo = new RatingRepository();
        private static readonly IAuthServiceRepository authService = new AuthService(memberRepo);

        private static readonly RecommendationService recommendationService =
            new RecommendationService(ratingRepo, bookRepo);
        private static readonly AverageRatingRecommender averageRatingRecommender =
            new AverageRatingRecommender(ratingRepo, bookRepo);
        
        /// <summary>
        /// Starts the program and loads book and rating files.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        private static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Book Recommendation Program.");
            Console.WriteLine();

            Console.Write("Enter books file: ");
            string booksFile = Console.ReadLine() ?? "";

            Console.Write("Enter rating file: ");
            string ratingsFile = Console.ReadLine() ?? "";
            Console.WriteLine();

            LoadBooks(booksFile);
            LoadRatings(ratingsFile);

            Console.WriteLine($"# of books: {bookRepo.Count()}");
            Console.WriteLine($"# of memberList: {memberRepo.Count()}");
            Console.WriteLine();

            RunMenu();

            Console.WriteLine();
            Console.WriteLine("Thank you for using the Book Recommendation Program!");
        }

        /// <summary>
        /// Runs the main menu loop until the user quits.
        /// </summary>
        private static void RunMenu()
        {
            bool running = true;

            while (running)
            {
                PrintMenu();

                Console.Write("Enter a menu option: ");
                string choice = Console.ReadLine() ?? "";
                Console.WriteLine();

                if (!authService.IsLoggedIn())
                {
                    switch (choice)
                    {
                        case "1":
                            AddMember();
                            break;
                        case "2":
                            AddBook();
                            break;
                        case "3":
                            Login();
                            break;
                        case "4":
                            running = false;
                            break;
                        default:
                            Console.WriteLine("Invalid menu option.");
                            break;
                    }
                }
                else
                {
                    switch (choice)
                    {
                        case "1":
                            AddMember();
                            break;
                        case "2":
                            AddBook();
                            break;
                        case "3":
                            RateBook();
                            break;
                        case "4":
                            ViewRatings();
                            break;
                        case "5":
                            SeeRecommendations();
                            break;
                        case "6":
                            authService.Logout();
                            break;
                        default:
                            Console.WriteLine("Invalid menu option.");
                            break;
                    }
                }

                if (running)
                    Console.WriteLine();
            }
        }

        /// <summary>
        /// Displays the correct menu depending on whether a member is logged in.
        /// </summary>
        private static void PrintMenu()
        {
            Console.WriteLine("************** MENU **************");

            if (!authService.IsLoggedIn())
            {
                Console.WriteLine("* 1. Add a new member            *");
                Console.WriteLine("* 2. Add a new book              *");
                Console.WriteLine("* 3. Login                       *");
                Console.WriteLine("* 4. Quit                        *");
            }
            else
            {
                Console.WriteLine("* 1. Add a new member            *");
                Console.WriteLine("* 2. Add a new book              *");
                Console.WriteLine("* 3. Rate book                   *");
                Console.WriteLine("* 4. View ratings                *");
                Console.WriteLine("* 5. See recommendations         *");
                Console.WriteLine("* 6. Logout                      *");
            }

            Console.WriteLine("**********************************");
            Console.WriteLine();
        }

        /// <summary>
        /// Logs in a member using their account id.
        /// </summary>
        private static void Login()
        {
            Console.Write("Enter member account: ");
            string accountId = Console.ReadLine() ?? "";

            if (authService.Login(accountId))
            {
                var member = memberRepo.GetByAccount(accountId);
                Console.WriteLine($"{member?.Name}, you are logged in!");
            }
            else
            {
                Console.WriteLine("Member not found.");
            }
        }

        /// <summary>
        /// Adds a new member and creates default 0 ratings for all existing books.
        /// </summary>
        private static void AddMember()
        {
            Console.Write("Enter the name of the new member: ");
            string name = Console.ReadLine() ?? "";

            string accountId = GenerateNextMemberId();
            var member = new Member(accountId, name);
            memberRepo.Add(member);

            foreach (var book in bookRepo.GetAll())
            {
                ratingRepo.Add(new Rating(accountId, book.ISBN, RatingValue.NotRead));
            }

            Console.WriteLine($"{name} (account #: {accountId}) was added.");
        }

        /// <summary>
        /// Adds a new book and creates default 0 ratings for all existing members.
        /// </summary>
        private static void AddBook()
        {
            Console.Write("Enter the author of the new book: ");
            string author = Console.ReadLine() ?? "";

            Console.Write("Enter the title of the new book: ");
            string title = Console.ReadLine() ?? "";

            Console.Write("Enter the year (or range of years) of the new book: ");
			string year = Console.ReadLine() ?? "";

            string isbn = GenerateNextBookId();
            var book = new Book(isbn, author, title, year);
            bookRepo.AddBook(book);

            foreach (var member in memberRepo.GetAll())
            {
                ratingRepo.Add(new Rating(member.AccountId, isbn, RatingValue.NotRead));
            }

            Console.WriteLine($"{isbn}, {author}, {title}, {year} was added.");
        }

        /// <summary>
        /// Displays all ratings for the currently logged-in member.
        /// </summary>
        private static void ViewRatings()
        {
            string memberId = authService.CurrentMemberId()!;
            var member = memberRepo.GetByAccount(memberId);

            Console.WriteLine($"{member?.Name}'s ratings...");

            foreach (var book in bookRepo.GetAll())
            {
                var value = ratingRepo.GetValue(memberId, book.ISBN);
                Console.WriteLine($"{book.ISBN}, {book.Author}, {book.Title}, {book.Year} => rating: {(int)value}");
            }
        }

        /// <summary>
        /// Lets the current member rate or re-rate a book.
        /// </summary>
        private static void RateBook()
        {
            string memberId = authService.CurrentMemberId()!;

            Console.Write("Enter the ISBN for the book you'd like to rate: ");
            string isbn = Console.ReadLine() ?? "";

            var book = bookRepo.GetByISBN(isbn);
            if (book == null)
            {
                Console.WriteLine("Book not found.");
                return;
            }

            var currentRating = ratingRepo.GetByMemberAndBook(memberId, isbn);

            if (currentRating != null && currentRating.Value != RatingValue.NotRead)
            {
                Console.WriteLine($"Your current rating for {book.ISBN}, {book.Author}, {book.Title}, {book.Year} => rating: {(int)currentRating.Value}");
                Console.Write("Would you like to re-rate this book (y/n)? ");
                string answer = (Console.ReadLine() ?? "").Trim().ToLower();

                if (answer != "y")
                    return;
            }

            Console.Write("Enter your rating: ");
            int ratingInput;
            while (!int.TryParse(Console.ReadLine(), out ratingInput) || !IsValidRating(ratingInput))
            {
                Console.Write("Invalid rating. Enter -5, -3, 0, 1, 3, or 5: ");
            }

            if (ratingRepo.HasRating(memberId, isbn))
            {
                ratingRepo.Update(memberId, isbn, (RatingValue)ratingInput);
            }
            else
            {
                ratingRepo.Add(new Rating(memberId, isbn, (RatingValue)ratingInput));
            }

            Console.WriteLine($"Your new rating for {book.ISBN}, {book.Author}, {book.Title}, {book.Year} => rating: {ratingInput}");
        }

        /// <summary>
        /// Displays recommendations for the current member based on the most similar member.
        /// </summary>
        private static void SeeRecommendations()
        {
            string memberId = authService.CurrentMemberId()!;
            var mostSimilarMember = FindMostSimilarMember(memberId);

            if (mostSimilarMember == null)
            {
                Console.WriteLine("No recommendations available.");
                return;
            }

            Console.WriteLine($"You have similar taste in books as {mostSimilarMember.Name}!");
            Console.WriteLine();

            var unreadBooksFromSimilarMember = bookRepo.GetAll()
                .Where(book => ratingRepo.GetValue(memberId, book.ISBN) == RatingValue.NotRead)
                .Select(book => new
                {
                    Book = book,
                    SimilarMemberRating = ratingRepo.GetValue(mostSimilarMember.AccountId, book.ISBN)
                })
                .Where(x => x.SimilarMemberRating == RatingValue.Love ||
                            x.SimilarMemberRating == RatingValue.Like ||
                            x.SimilarMemberRating == RatingValue.Ok)
                .ToList();

            var reallyLiked = unreadBooksFromSimilarMember
                .Where(x => x.SimilarMemberRating == RatingValue.Love)
                .ToList();

            var liked = unreadBooksFromSimilarMember
                .Where(x => x.SimilarMemberRating == RatingValue.Like || x.SimilarMemberRating == RatingValue.Ok)
                .ToList();

            Console.WriteLine("Here are the books they really liked:");
            foreach (var item in reallyLiked)
            {
                Console.WriteLine($"{item.Book.ISBN}, {item.Book.Author}, {item.Book.Title}, {item.Book.Year}");
            }

            Console.WriteLine();
            Console.WriteLine("And here are the books they liked:");
            foreach (var item in liked)
            {
                Console.WriteLine($"{item.Book.ISBN}, {item.Book.Author}, {item.Book.Title}, {item.Book.Year}");
            }
        }

        /// <summary>
        /// Finds the member whose ratings are most similar to the current member
        /// using the dot product of rating vectors.
        /// </summary>
        /// <param name="memberId">The account id of the current member.</param>
        /// <returns>The most similar member, or null if none found.</returns>
        private static Member? FindMostSimilarMember(string memberId)
        {
            int bestSimilarity = int.MinValue;
            Member? bestMatch = null;

            foreach (var otherMember in memberRepo.GetAll())
            {
                if (otherMember.AccountId == memberId)
                    continue;

                int similarity = 0;

                foreach (var book in bookRepo.GetAll())
                {
                    int myRating = (int)ratingRepo.GetValue(memberId, book.ISBN);
                    int otherRating = (int)ratingRepo.GetValue(otherMember.AccountId, book.ISBN);
                    similarity += myRating * otherRating;
                }

                if (similarity > bestSimilarity)
                {
                    bestSimilarity = similarity;
                    bestMatch = otherMember;
                }
            }

            return bestMatch;
        }

        /// <summary>
        /// Loads books from the given file and stores them in the repository.
        /// </summary>
        /// <param name="fileName">The path of the books file.</param>
        private static void LoadBooks(string fileName)
        {
            if (!File.Exists(fileName))
            {
                Console.WriteLine("Books file not found.");
                return;
            }

            string[] lines = File.ReadAllLines(fileName);

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] parts = line.Split(',', 3);

                if (parts.Length < 3)
                    continue;

                string author = parts[0].Trim();
                string title = parts[1].Trim();
                string year = parts[2].Trim();
                              
                string isbn = GenerateNextBookId();
                var book = new Book(isbn, author, title, year);
                bookRepo.AddBook(book);
            }
        }

        /// <summary>
        /// Loads members and their ratings from the given file.
        /// </summary>
        /// <param name="fileName">The path of the ratings file.</param>
        private static void LoadRatings(string fileName)
        {
            if (!File.Exists(fileName))
            {
                Console.WriteLine("Ratings file not found.");
                return;
            }

            string[] lines = File.ReadAllLines(fileName);
            var books = bookRepo.GetAll();
            int i = 0;
            while (i < lines.Length)
            {
                // Skip blank lines
                if (string.IsNullOrWhiteSpace(lines[i])) { i++; continue; }
 
                string memberName = lines[i].Trim();
                i++;
 
                // Skip blank lines between name and ratings
                while (i < lines.Length && string.IsNullOrWhiteSpace(lines[i])) i++;
                if (i >= lines.Length) break;
 
                string ratingsLine = lines[i].Trim();
                i++;
 
                // Safety check: if this line doesn't look like ratings, treat it as
                // the next member name and back up
                if (!char.IsDigit(ratingsLine[0]) && ratingsLine[0] != '-')
                {
                    i--;
                    continue;
                }
 
                string accountId = GenerateNextMemberId();
                memberRepo.Add(new Member(accountId, memberName));
 
                string[] ratingParts = ratingsLine.Split(' ');
                for (int j = 0; j < books.Count; j++)
                {
                    int ratingValue = 0;
                    if (j < ratingParts.Length)
                    {
                        int.TryParse(ratingParts[j].Trim(), out ratingValue);
                        if (!IsValidRating(ratingValue)) ratingValue = 0;
                    }
                    ratingRepo.Add(new Rating(accountId, books[j].ISBN, (RatingValue)ratingValue));
                }
            }
        }

        /// <summary>
        /// Checks whether the given number is an allowed rating value.
        /// </summary>
        /// <param name="value">The rating number to validate.</param>
        /// <returns>True if valid; otherwise false.</returns>
        private static bool IsValidRating(int value)
        {
            return value == -5 || value == -3 || value == 0 || value == 1 || value == 3 || value == 5;
        }

        /// <summary>
        /// Generates the next book id based on the current number of books.
        /// </summary>
        /// <returns>The next book id as a string.</returns>
        private static string GenerateNextBookId()
        {
            return (bookRepo.Count() + 1).ToString();
        }

        /// <summary>
        /// Generates the next member account id based on the current number of members.
        /// </summary>
        /// <returns>The next member id as a string.</returns>
        private static string GenerateNextMemberId()
        {
            return (memberRepo.Count() + 1).ToString();
        }
    }
}