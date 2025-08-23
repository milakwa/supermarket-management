namespace supermarket_management 
{
    public class Product
    {
        // Product attributes 
        public string Name;
        public string Category;
        public int Quantity;
        public DateTime ProductionDate;
        public DateTime ExpiryDate;
        // Constructor: it calls when we add a new Product and fills in attributes
        public Product(string name, string category, int quantity, DateTime productionDate, DateTime expiryDate)
        {
            Name = name;
            Category = category;
            Quantity = quantity;
            ProductionDate = productionDate;
            ExpiryDate = expiryDate;
        }
        public void Display()
        {
            Console.WriteLine(Name + ", " + Category + ", " + Quantity + ", " + ProductionDate + ", " + ExpiryDate);
        }
    }
    public static class DataStore
    {
        public static List<Product> products = new List<Product>();
        public static string filePath = "Products.txt";

        public static void loadProducts()
        {
            if (File.Exists(filePath))
            {
                foreach (string line in File.ReadAllLines(filePath))
                {
                    string[] parts = line.Split(',');  // To line Split from file 
                    Product p = new Product(
                        parts[0],                      // Name
                        parts[1],                      // Category
                        int.Parse(parts[2]),           // Quantity
                        DateTime.Parse(parts[3]),      // ProductionDate
                        DateTime.Parse(parts[4])       // ExpiryDate
                        );
                    products.Add(p);
                }
            }


        }
        public static void saveProducts()
        {
            List<string> lines = new List<string>();
            foreach (Product p in products)
            {
                lines.Add($"{p.Name} ,{p.Category} ,{p.Quantity} ,{p.ProductionDate} ,{p.ExpiryDate}.");
            }

            File.WriteAllLines(filePath, lines);
        }


    }
    public class Admin
    {
        private string Username;
        private string Password;
        public string username { get; set; }
        public string password { get; set; }

        public Admin(string user, string pass)
        {
            username = user;
            password = pass;
        }
        public void AddProduct(Product p, int q)
        {


            DataStore.products.Add(p);

        }
        public void viewExpiryAlerts()
        {

            DateTime today;
            bool valid = false;


            while (!valid)
            {
                Console.Write("Enter today's date (yyyy-MM-dd): ");
                string input = Console.ReadLine();

                if (DateTime.TryParse(input, out today))
                {
                    valid = true;

                    bool found = false;

                    foreach (Product p in DataStore.products)
                    {
                        TimeSpan diff = p.expiryDate - today;

                        if (diff.TotalDays <= 7 && diff.TotalDays >= 0)
                        {
                            Console.WriteLine($"{p.name} (Qty: {p.quantity}) is near expiry.");
                            found = true;
                        }
                    }

                    if (!found)
                    {
                        Console.WriteLine("No products near expiry.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid date format! Please try again.");
                }
            }
        }
        public bool checkExpiry(Product p)
        {
            DateTime today = DateTime.Now;
            TimeSpan diff = p.ExpiryDate - today;

            if (diff.TotalDays <= 7 && diff.TotalDays >= 0)
            {
                return true;
            }

            return false;
        }

    }

    internal class Program
    {
        static void Main(string[] args)
        {
                while(true){
                Console.WriteLine("1. Admin");
                Console.WriteLine("2. Customer");
                Console.WriteLine("3. Exit");
                Console.Write("Enter your choice: ");
                int choice = int.Parse(Console.ReadLine());
            switch(choice){
                case 1:
                    Console.Write("Enter your name: ");
                    string user = Console.ReadLine();
                    Console.Write("Creat your pass: ");
                    string pass = Console.ReadLine();
                    Console.WriteLine("Hello "+ user);
                    Console.WriteLine("1. Show all products");
                    Console.WriteLine("2. Add new products");
                    Console.WriteLine("3. Show expiry products");
                    Console.WriteLine("4. Exit");
                    Console.Write("Enter your choice: ");
                    int choice1 = int.Parse(Console.ReadLine());
                    switch(choice1){
                        case 1:
                           Display();
                           break;
                        case 2:
                           AddProuct();
                           break;
                        case 3:
                           ViewExpiryAlerts();
                           break;
                        case 4:
                           return;
                           break;
                        default:
                           Console.WriteLine("Invalid choice. Try again");           
                           break;
                    }
                    break;
                case 2:
                    Console.Write("Enter your name: ");
                    string user = Console.ReadLine();
                    Console.Write("Creat your pass: ");
                    string pass = Console.ReadLine();
                    Console.WriteLine("Hello "+ user);
                    Console.WriteLine("1. Show all products");
                    Console.WriteLine("2. Show list products bought");
                    Console.Write("Enter your choice: ");
                    int choice2 = int.Parse(Console.ReadLine());
                    switch(choice2){
                            case 1:
                                Display();
                                Console.Write("Enter num products to buy; ");
                                    //
                                break;
                            case 2: 
                                    //
                                break;
                            default:
                                break;
                    }
                    break;
                case 3:
                    return;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Try again");
                    break; 
            }
          }
        }
    }
}
