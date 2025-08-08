using System;

namespace ConsoleApp4
{
    public class Game
    {
        private static int totalGames = 0;
        // Fields
        private int itemNumber;
        private string itemName = string.Empty;
        private decimal price;
        private double userRating;
        private int quantity;
        // Properties
        public int ItemNumber
        {
            get => itemNumber;
            set
            {
                if (value >= 1000 && value <= 9999)
                    itemNumber = value;
                else
                    throw new ArgumentException("Item number must be a 4-digit code.");
            }
        }

        public string ItemName
        {
            get => itemName;
            set => itemName = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException("Item name cannot be empty.");
        }

        public decimal Price
        {
            get => price;
            set => price = value >= 0 ? value : throw new ArgumentException("Price cannot be negative.");
        }

        public double UserRating
        {
            get => userRating;
            set
            {
                if (value >= 0 && value <= 5)
                    userRating = value;
                else
                    throw new ArgumentException("User rating must be between 0 and 5.");
            }
        }

        public int Quantity
        {
            get => quantity;
            set => quantity = value >= 0 ? value : throw new ArgumentException("Quantity cannot be negative.");
        }

        // Constructors
        public Game(int itemNumber, string itemName, decimal price, double userRating, int quantity)
        {
            ItemNumber = itemNumber;
            ItemName = itemName;
            Price = price;
            UserRating = userRating;
            Quantity = quantity;

            totalGames++;
        }

        public Game() : this(1000, "Unknown Game", 0.00m, 0.0, 0) { }

        public bool IsAvailable() => quantity > 0;

        public void ApplyDiscount(decimal percent)
        {
            if (percent < 0 || percent > 100)
                throw new ArgumentException("Discount must be between 0 and 100.");
            Price -= Price * (percent / 100);
        }

        public static int GetTotalGamesCreated() => totalGames;

        public override string ToString()
        {
            return $"\n[GAME INFO]\n" +
                   $"Item #:      {ItemNumber}\n" +
                   $"Title:       {ItemName}\n" +
                   $"Price:       ${Price:F2}\n" +
                   $"Rating:      {UserRating}/5\n" +
                   $"In Stock:    {Quantity}\n" +
                   $"Availability:{(IsAvailable() ? "Yes" : "No")}\n";
        }
        //Menu
        public static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\n=== VIDEO GAME SHOP MENU ===");
                Console.WriteLine("1. Display All Games");
                Console.WriteLine("2. Add New Game");
                Console.WriteLine("3. Search by Item Number");
                Console.WriteLine("4. Search by Maximum Price");
                Console.WriteLine("5. Statistical Analysis");
                Console.WriteLine("6. Exit");
                Console.Write("Enter your choice: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        DisplayAllGames();
                        break;
                    case "2":
                        AddNewGame();
                        break;
                    case "3":
                        SearchByItemNumber();
                        break;
                    case "4":
                        SearchByMaxPrice();
                        break;
                    case "5":
                        StatisticalAnalysis();
                        break;
                    case "6":
                        Console.WriteLine("\nThank you for using the Video Game Shop!");
                        Console.WriteLine("Press Enter to exit...");
                        Console.ReadLine(); 
                        return;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }
        }

        //Methods 

        static void DisplayAllGames()
        {
            if (!File.Exists("VideoGames.txt"))
            {
                Console.WriteLine("No games in inventory.");
            }
            else
            {
                string[] lines = File.ReadAllLines("VideoGames.txt");
                bool anyDisplayed = false;

                foreach (var line in lines)
                {
                    string[] parts = line.Split(',');

                    if (parts.Length != 5)
                        continue;

                    // Try to parse and validate
                    if (int.TryParse(parts[0], out int itemNumber) &&
                        itemNumber >= 1000 && itemNumber <= 9999 &&
                        decimal.TryParse(parts[2], out decimal price) &&
                        double.TryParse(parts[3], out double rating) &&
                        int.TryParse(parts[4], out int quantity))
                    {
                        try
                        {
                            string itemName = parts[1].Replace('_', ' ');
                            Game g = new Game(itemNumber, itemName, price, rating, quantity);
                            Console.WriteLine(g.ToString());
                            anyDisplayed = true;
                        }
                        catch
                        {
                            // Skip invalid game entries
                        }
                    }
                }

                if (!anyDisplayed)
                {
                    Console.WriteLine("No valid games found in inventory.");
                }
            }

            Console.WriteLine("\nPress Enter to return to the menu...");
            Console.ReadLine();
        }
        static void AddNewGame()
        {
            static void SaveGame(Game g)
            {
                string line = $"{g.ItemNumber},{g.ItemName.Replace(' ', '_')},{g.Price},{g.UserRating},{g.Quantity}";
                using (StreamWriter writer = new StreamWriter("VideoGames.txt", append: true))
                {
                    writer.WriteLine(line);
                }
            }
            if (totalGames >= 100)
            {
                Console.WriteLine("Inventory full.");
                return;
            }
            try
            {
                int itemNumber;
                while (true)
                {
                    Console.Write("Enter item number (or leave blank to auto-generate): ");
                    string input = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(input))
                    {
                        Random rand = new Random();
                        itemNumber = rand.Next(1000, 10000); // auto-generate
                        break;
                    }
                    else if (int.TryParse(input, out itemNumber) && itemNumber >= 1000 && itemNumber <= 9999)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a 4-digit number.");
                    }
                }

                string itemName;
                while (true)
                {
                    Console.Write("Enter item name: ");
                    itemName = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(itemName))
                    {
                        Console.WriteLine("Item name cannot be empty.");
                    }
                    else if (decimal.TryParse(itemName, out _))
                    {
                        Console.WriteLine("Item name cannot be a number.");
                    }
                    else
                    {
                        break;
                    }
                }

                decimal price;
                while (true)
                {
                    Console.Write("Enter price: ");
                    if (decimal.TryParse(Console.ReadLine(), out price) && price >= 0)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Price must be a non-negative number.");
                    }
                }

                double rating;
                while (true)
                {
                    Console.Write("Enter user rating (0–5): ");
                    if (double.TryParse(Console.ReadLine(), out rating) && rating >= 0 && rating <= 5)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Rating must be between 0 and 5.");
                    }
                }

                int quantity;
                while (true)
                {
                    Console.Write("Enter quantity: ");
                    if (int.TryParse(Console.ReadLine(), out quantity) && quantity >= 0)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Quantity must be a non-negative integer.");
                    }
                }

                Game g = new Game(itemNumber, itemName, price, rating, quantity);
                totalGames++;
                SaveGame(g);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error: " + ex.Message);
            }

        }

        static void SearchByItemNumber()
        {
            Console.Write("Enter the item number to search: ");
            string input = Console.ReadLine();

            if (!int.TryParse(input, out int itemNumber))
            {
                Console.WriteLine("Invalid item number.");
                Console.WriteLine("\nPress Enter to return to the menu...");
                Console.ReadLine();
                return;
            }

            if (!File.Exists("VideoGames.txt"))
            {
                Console.WriteLine("No data found.");
                Console.WriteLine("\nPress Enter to return to the menu...");
                Console.ReadLine();
                return;
            }

            string[] lines = File.ReadAllLines("VideoGames.txt");
            bool found = false;

            foreach (var line in lines)
            {
                string[] parts = line.Split(',');

                if (parts.Length == 5 &&
                    int.TryParse(parts[0], out int currentId) &&
                    currentId == itemNumber &&
                    currentId >= 1000 && currentId <= 9999 &&
                    decimal.TryParse(parts[2], out decimal price) &&
                    double.TryParse(parts[3], out double rating) &&
                    int.TryParse(parts[4], out int quantity))
                {
                    try
                    {
                        string itemName = parts[1].Replace('_', ' ');
                        Game g = new Game(currentId, itemName, price, rating, quantity);
                        Console.WriteLine(g.ToString());
                        found = true;
                        break;
                    }
                    catch
                    {
                        // Skip invalid entry
                    }
                }
            }

            if (!found)
            {
                Console.WriteLine("Game not found.");
            }

            Console.WriteLine("\nPress Enter to return to the menu...");
            Console.ReadLine();
        }


        static void SearchByMaxPrice()
        {
            Console.Write("Enter the maximum price: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal maxPrice))
            {
                Console.WriteLine("Invalid price.");
                return;
            }

            if (!File.Exists("VideoGames.txt"))
            {
                Console.WriteLine("No data found.");
                return;
            }

            string[] lines = File.ReadAllLines("VideoGames.txt");
            bool found = false;

            foreach (var line in lines)
            {
                string[] parts = line.Split(',');

                if (parts.Length != 5)
                    continue;

                if (int.TryParse(parts[0], out int itemNumber) &&
                    itemNumber >= 1000 && itemNumber <= 9999 &&
                    decimal.TryParse(parts[2], out decimal price) &&
                    double.TryParse(parts[3], out double rating) &&
                    int.TryParse(parts[4], out int quantity))
                {
                    if (price <= maxPrice)
                    {
                        try
                        {
                            string itemName = parts[1].Replace('_', ' ');
                            Game g = new Game(itemNumber, itemName, price, rating, quantity);
                            Console.WriteLine(g.ToString());
                            found = true;
                        }
                        catch
                        {
                            // Skip invalid lines
                        }
                    }
                }
            }

            if (!found)
            {
                Console.WriteLine("No games found within that price range.");
            }

            Console.WriteLine("\nPress Enter to return to the menu...");
            Console.ReadLine();
        }

        static void StatisticalAnalysis()
        {
            if (!File.Exists("VideoGames.txt"))
            {
                Console.WriteLine("No data available.");
                return;
            }

            string[] lines = File.ReadAllLines("VideoGames.txt");

            if (lines.Length == 0)
            {
                Console.WriteLine("No data available.");
                return;
            }

            decimal totalPrice = 0;
            double totalRating = 0;
            int totalQuantity = 0;
            int count = 0;

            foreach (var line in lines)
            {
                string[] parts = line.Split(',');

                if (parts.Length == 5 &&
                    decimal.TryParse(parts[2], out decimal price) &&
                    double.TryParse(parts[3], out double rating) &&
                    int.TryParse(parts[4], out int quantity))
                {
                    totalPrice += price;
                    totalRating += rating;
                    totalQuantity += quantity;
                    count++;
                }
            }

            if (count == 0)
            {
                Console.WriteLine("No valid game data.");
                return;
            }

            Console.WriteLine("\n--- Statistical Analysis ---");
            Console.WriteLine($"Total Games:        {count}");
            Console.WriteLine($"Average Price:      ${totalPrice / count:F2}");
            Console.WriteLine($"Average Rating:     {totalRating / count:F2}/5");
            Console.WriteLine($"Total Units In Stock: {totalQuantity}");
            Console.WriteLine("\nPress Enter to return to the menu...");
            Console.ReadLine();
        }


    }
}
