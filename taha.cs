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
                        break;
                    case "2":
                        AddNewGame();
                        break;
                    case "3":
                        break;
                    case "4":
                        break;
                    case "5":
                        break;
                    case "6":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }
        }
        
        //Methods 
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
    }
}
