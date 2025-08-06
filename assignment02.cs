using System;

namespace assignment02
{
    public class Game
    {
        public static void Main(string[] args)
        {
            Game game1 = new Game(1234, "Minecraft", 59.99m, 4.7, 15);
            Console.WriteLine(game1);

            game1.ApplyDiscount(10);
            Console.WriteLine("After applying 10% discount:");
            Console.WriteLine(game1);

            // Create another game
            Game game2 = new Game();
            Console.WriteLine(game2);

            // total games created
            Console.WriteLine($"Total games created: {Game.GetTotalGamesCreated()}");
        }

        // Static field to track total games created
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
    }
}
