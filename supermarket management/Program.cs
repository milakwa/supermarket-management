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
            Admin a = new Admin("malak", "1234");
            int n = 2;
            while (n>0)
            {
                
                string name;
                
                string c;

                int q;
                Console.Write("productionDate:");
                DateTime p =DateTime.Parse(Console.ReadLine());
                Console.Write("expirydate:");
                DateTime e = DateTime.Parse(Console.ReadLine());
                Product product = new Product(name="malak", c="hhh", q=90, p, e);
                
                a.AddProduct(product, product.Quantity);
                n--;
            }
            

            a.viewExpiryAlerts();


        }
    }
}
