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
        // Method to display product details
        public void Display()
        {
            Console.WriteLine($"{Name} ,{Category} ,{Quantity},{ProductionDate:yyyy-MM-dd},{ExpiryDate:yyyy-MM-dd}\n");
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
                lines.Add($"{p.Name},{p.Category},{p.Quantity},{p.ProductionDate:yyyy-MM-dd},{p.ExpiryDate:yyyy-MM-dd}");

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
        // Method to add a product to the product list
        public void AddProduct(Product p, int q)
        {
            DataStore.products.Add(p);
        }
        // Method to view products that are near expiry (within 7 days)
        public void viewExpiryAlerts()
        {
            DateTime today = DateTime.Now;
            bool found = false;

            foreach (Product p in DataStore.products)
            {
                if (checkExpiry(p))
                {
                    Console.WriteLine($"{p.Name} (Qty: {p.Quantity}) is near expiry.");
                    found = true;
                    continue;
                }

            }
            if (!found)
            {
                Console.WriteLine("No products near expiry.");
            }

        }        
        // Method to check if a product is near expiry (within 7 days)
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
        static List<Order> customerOrders = new List<Order>();
        static List<Admin> admins = new List<Admin>();
        static void Main(string[] args)
        {
            DataStore.loadProducts();

            while (true)
            { 
                Console.WriteLine("--- Supermarket Management ---");
                Console.WriteLine("1. Creat Admin");
                Console.WriteLine("2. Login as Admin (You already have an account)");
                Console.WriteLine("3. Continue as Customer");
                Console.WriteLine("4. Exit");
                Console.Write("Choose: ");
                string choice = Console.ReadLine();
                
                if (choice == "1")
                {
                    Admin admin=CreatAdmin(); // Here we can use the created admin object
                    admins.Add(admin);
                    AdminMenu(admin);
                }
                else if(choice == "2"){
                    Admin admin = LoginAdmin();
                    if (admin != null)
                    {
                        AdminMenu(admin);
                    }
                }
                else if(choice == "3"){
                    CustomerMenu();
                }
                else{
                    return;
                }
            }
        }
        // Method to create an admin account
        public static Admin CreatAdmin()
        {
            string name, pass;
            while (true)
            {
                Console.Write("\nEnter Username: ");
                name = Console.ReadLine();
                Console.Write("Enter Password: ");
                pass = Console.ReadLine();
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(pass))
                {
                    Console.WriteLine("\nUsername or Password cannot be empty. Please try again.\n");
                    continue;
                }
                else
                {
                    break;
                }
            }
            // Create an Admin object with the provided username and password
            Admin admin = new Admin(name, pass);
            Console.WriteLine("\nAdmin created successfully!\n");
            return admin; // Return the created admin object
        }
        // Method to check login admin
        public static Admin LoginAdmin()
        {
            Console.Write("\nEnter Username: ");
            string user = Console.ReadLine();
            Console.Write("Enter Password: ");
            string pass = Console.ReadLine();

            Admin admin = admins.Find(a => a.username == user && a.password == pass);

            if (admin != null)
                Console.WriteLine("\nLogged in successfully!\n");
            else
                Console.WriteLine("\n Invalid username or password!\n");

            return admin;
        }
        //Admin menu
        public static void AdminMenu(Admin admin)
        {
            while (true)
            {
                Console.WriteLine("\n--- Admin Menu ---");
                Console.WriteLine("1. View Products");
                Console.WriteLine("2. Add Product");
                Console.WriteLine("3. View Expiry Alerts");
                Console.WriteLine("4. Save & Exit");
                Console.Write("Choose: ");
                string aChoice = Console.ReadLine();

                switch (aChoice)
                {
                    case "1":
                        Console.WriteLine("\n--- Products List ---\n");
                        foreach (Product p in DataStore.products)
                        {
                            p.Display();
                        }
                        break;

                    case "2":
                        InputProductsDetails(admin);
                        break;

                    case "3":
                        Console.WriteLine("\n--- Products near expiry ---\n");
                        admin.viewExpiryAlerts();
                        break;

                    case "4":
                        DataStore.saveProducts();
                        Console.WriteLine("\nProducts saved. Exiting...\n");
                        return;

                    default:
                        Console.WriteLine("\nInvalid choice. Please try again.\n");
                        break;
                }
            }
        }
        // Method to input product details
        static void InputProductsDetails(Admin admin)
        {
            string name, category;
            int quantity;
            DateTime productionDate, expiryDate;

            // Loop until valid product details are entered
            while (true)
            {
                Console.WriteLine("\n--- Input Product Details ---");
                Console.Write("Enter Product Name: ");
                name = Console.ReadLine();
                Console.Write("Enter Product Category: ");
                category = Console.ReadLine();
        
                Console.Write("Enter Product Quantity: ");
                if (!int.TryParse(Console.ReadLine(), out quantity) || quantity <= 0)
                {
                    Console.WriteLine("\nQuantity must be a positive number. Please try again.");
                    continue;
                }

                Console.Write("Enter Production Date (yyyy-MM-dd): ");
                if (!DateTime.TryParse(Console.ReadLine(), out productionDate))
                {
                    Console.WriteLine("\nInvalid production date. Please try again.");
                    continue;
                }

                Console.Write("Enter Expiry Date (yyyy-MM-dd): ");
                if (!DateTime.TryParse(Console.ReadLine(), out expiryDate))
                {
                    Console.WriteLine("\nInvalid expiry date. Please try again.");
                    continue;
                }

                if (productionDate > DateTime.Now)
                {
                    Console.WriteLine("\nProduction date cannot be in the future. Please try again.");
                    continue;
                }
                if (expiryDate < productionDate)
                {
                    Console.WriteLine("\nExpiry date cannot be earlier than production date. Please try again.");
                    continue;
                }

                break;
            }

            // Create a new Product object and add it to the admin's product list
            Product newProduct = new Product(name, category, quantity, productionDate, expiryDate);
            admin.AddProduct(newProduct, quantity);
            
        }
        // Customer menu
        static void CustomerMenu()
        {
            while (true)
            {
                Console.WriteLine("\n--- Customer Menu ---");
                Console.WriteLine("1. View Products");
                Console.WriteLine("2. Buy Product");
                Console.WriteLine("3. Back to Main Menu");
                Console.WriteLine("4. View My Purchases");
                Console.Write("Choose: ");
                string Choicee = Console.ReadLine();

                if (Choicee == "1")
                {
                    Console.WriteLine("\n--- Available Products ---\n");
                    foreach (Product p in DataStore.products){ 
                        p.Display();
                    }
                }
                else if (Choicee == "2")
                {
                    Console.Write("Enter Product Name: ");
                    string pname = Console.ReadLine();

                    Console.Write("Enter Quantity to Buy: ");
                    int qty;
                    if (!int.TryParse(Console.ReadLine(), out qty) || qty <= 0)
                    {
                        Console.WriteLine("Invalid quantity.");
                        continue;
                    }

                    // To search of Product
                    Product product = DataStore.products.Find(p => p.Name.Equals(pname, StringComparison.OrdinalIgnoreCase));

                    if (product != null && product.Quantity >= qty)
                    {
                        product.Quantity -= qty; // To deducted from the store
                        DataStore.saveProducts();
                        customerOrders.Add(new Order(product.Name, qty));
                        Console.WriteLine($"You bought {qty} of {product.Name}. Remaining stock: {product.Quantity}");
                    }
                    else
                    {
                        Console.WriteLine("Not enough stock or product not found.");
                    }
                }
                else if (Choicee == "3")
                {
                    break; // To return to main menu 
                }
                else if (Choicee == "4"){
                    Console.WriteLine("\n--- Your Purchases ---\n");
                    if (customerOrders.Count == 0){
                        Console.WriteLine("No purchases yet.");
                    }
                    else{
                        foreach (Order order in customerOrders)
                        {
                            order.DisplayProduct();
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Invalid choice.");
                }
            }
        }
        // To storge customer orders
        public class Order {
            public string ProductName { get; set; }
            public int Quantity { get; set; }
            public Order(string productName, int quantity)
            {
                ProductName = productName;
                Quantity = quantity;
            }
        
            public void DisplayProduct()
            {
                Console.WriteLine($"- {ProductName} (x{Quantity})");
            }
        }
    }
}
