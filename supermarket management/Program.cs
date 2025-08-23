using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static supermarket_management.Program;

namespace supermarket_management
{
    //Habeba
    // Product class to represent a product in the supermarket
    public class Product
    {
        // Product attributes 
        public string Name {  get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        public DateTime ProductionDate { get; set; }
        public DateTime ExpiryDate { get; set; }

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
    //Malak & Habeba
    // DataStore class to manage products and admins
    public static class DataStore
    {
        // Static lists to store products and admins
        public static List<Product> products = new List<Product>();
        public static List<Admin> admins = new List<Admin>();
        // File path to store products data
        public static string filePath = "Products.txt";

        // Static constructor to load products from file when the program starts
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
        // Method to save products to file
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
    //Habeba
    // Order class to represent a customer's order in the supermarket
    public class Order
    {
        // Order attributes
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public Order(string productName, int quantity)
        {
            ProductName = productName;
            Quantity = quantity;
        }
        // Method to display the product in the customer's cart
        public void DisplayProductInCatt()
        {
            Console.WriteLine($"- {ProductName} (qty:{Quantity})");
        }
    }
    //Aya
    // Customer class to represent a customer in the supermarket
    public class Customer
    {
        // Customer attributes
        public string Name { get; set; }
        public List<Order> cart { get; set; }

        public Customer(string name)
        {
            Name = name;
            cart = new List<Order>();
        }
        // Method to add a product to the customer's cart
        public void buyProduct(Product p, int quantity)
        {
            if (p.Quantity >= quantity)
            {
                // Deduct from stock
                p.Quantity -= quantity;

                // Add the order to the customer's cart (list of orders)
                cart.Add(new Order(p.Name, quantity));

                Console.WriteLine($"\nYou have successfully purchased {quantity} of {p.Name}!");
                Console.WriteLine($"Remaining stock: {p.Quantity}");
            }
            else
            {  
                // If not enough stock, display an error message
                Console.WriteLine($"\nPurchase failed. Not enough {p.Name} in stock.");
                Console.WriteLine($"Available quantity is {p.Quantity}");
            }
        }
    
        // Method to display the customer's cart
        public void displayCart()
        {
            Console.WriteLine("\n--- Your Purchases ---\n");
            if (cart.Count == 0)
            {
                Console.WriteLine("No purchases yet.");
            }
            foreach (Order order in cart)
            {
                order.DisplayProductInCatt();
            }
        }
    }
    //Sohila
    // Admin class to manage products in the supermarket
    public class Admin
    {
        // Admin attributes
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
            Product existingProduct = null;

            foreach (Product prod in DataStore.products)
            {
                if ((prod.Name.ToLower().Trim()) == (p.Name.ToLower().Trim()) && (prod.Category.ToLower().Trim()) == (p.Category.ToLower().Trim()))
                {
                    existingProduct = prod;
                    break;
                }
            }
            // If the product already exists, update its quantity; otherwise, add it as a new product
            if (existingProduct != null)
            {
                existingProduct.Quantity += q;
                Console.WriteLine($"\nUpdated {existingProduct.Name}, new quantity = {existingProduct.Quantity}");
            }
            else
            {
                DataStore.products.Add(p);
                Console.WriteLine($"\nAdded new product: {p.Name}");
            }
        }
        // Method to view all products in store
        public void ViewProducts()
        {
            Console.WriteLine("\n--- Products List ---\n");
            foreach (Product p in DataStore.products)
            {
                p.Display();
            }
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
        //Malak & Habeba
        static void Main(string[] args)
        {
            DataStore.loadProducts();

            while (true)
            {
                // Display the main menu options
                Console.WriteLine("--- Supermarket Management ---");
                Console.WriteLine("1. Creat Admin");
                Console.WriteLine("2. Login as Admin (You already have an account)");
                Console.WriteLine("3. Continue as Customer");
                Console.WriteLine("4. Exit");
                Console.Write("Choose: ");
                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    Admin admin = CreateAdmin(); // Here we can use the created admin object
                    DataStore.admins.Add(admin);// To add the created admin to the list
                    Console.WriteLine($"\nWelcome, Admin {admin.username}!");
                    AdminMenu(admin);// To call the AdminMenu method with the created admin object
                }
                else if (choice == "2")
                {
                    // To call the LoginAdmin method to login as admin
                    Admin admin = LoginAdmin();
                    if (admin != null)
                    {
                        Console.WriteLine($"\nWelcome back, Admin {admin.username}!");
                        AdminMenu(admin);
                    }
                    
                }
                else if (choice == "3")
                {
                    // Create a new customer object
                    Console.Write("\nEnter your name: ");
                    string cname = Console.ReadLine();
                    Customer customer = new Customer(cname);
                    Console.WriteLine($"\nWelcome, {customer.Name}!");
                    // To call the CustomerMenu method with the created customer object
                    CustomerMenu(customer);
                }
                else
                {
                    Console.WriteLine("\nExiting.......");
                    return;
                }
            }
        }
        //Malak & Habeba
        // Method to create an admin account
        public static Admin CreateAdmin()
        {
            string name, pass;
            while (true)
            {
                // Prompt the user for admin username and password
                Console.WriteLine("\n--- Create Admin ---");
                Console.Write("Enter Username: ");
                name = Console.ReadLine();
                Console.Write("Enter Password: ");
                pass = Console.ReadLine();

                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(pass))
                {
                    Console.WriteLine("\nUsername or Password cannot be empty. Please try again.\n");
                    continue;
                }
                // Check if the username already exists in the admin list
                Admin existingAdmin = DataStore.admins.Find(a => a.username == name);

                if (existingAdmin != null)
                {
                    Console.WriteLine("\nThis username already exists. Logging you in...\n");
                    
                    if (existingAdmin.password == pass)
                    {
                        Console.WriteLine($"Welcome back {existingAdmin.username}!");
                        return existingAdmin;
                    }
                    else
                    {
                        Console.WriteLine("\nWrong password for this username. Please login again.\n");
                        return null;
                    }
                }
                break;
            }
            // Create an Admin object with the provided username and password
            Admin admin = new Admin(name, pass);
            Console.WriteLine("\nAdmin created successfully!");
            return admin; // Return the created admin object
        }
        //Malak & Habeba
        // Method to check login admin
        public static Admin LoginAdmin()
        {
            if (DataStore.admins.Count == 0)
            {
                Console.WriteLine("\nNo admin accounts found. Please create an admin account first.\n");
                return null;
            }

            string user, pass;
            
            while(true)
            {
                // Prompt the user for admin username and password
                Console.WriteLine("\n--- Admin Login ---");
                Console.Write("Enter Username: ");
                user = Console.ReadLine();
                Console.Write("Enter Password: ");
                pass = Console.ReadLine();

                if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
                {
                    Console.WriteLine("\nUsername or Password cannot be empty. Please try again.\n");
                    continue;
                }
                // Check if the username and password match an existing admin account
                Admin admin = DataStore.admins.Find(a => a.username == user && a.password == pass);

                if (admin != null)
                {
                    Console.WriteLine("\nLogged in successfully!");
                    return admin;
                }

                else
                {
                    // If no match found, prompt the user to try again or create an account
                    Console.WriteLine("\nInvalid username or password or you don't have admin account!\n");
                    Console.Write("you have account(y/n):");
                    string haveAccount = Console.ReadLine().ToLower();
                    if (haveAccount == "y")
                    {
                        continue; // To continue the loop for login again
                    }
                    else if (haveAccount == "n")
                    {
                        return null; // To return null if the user doesn't have an account
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter 'y' or 'n'.");
                        continue; // To continue the loop for login again

                    }

                }

            }
            
            
        }
        //Malak & Habeba
        //Admin menu
        public static void AdminMenu(Admin admin)
        {
            while (true)
            {
                // Display the admin menu options
                Console.WriteLine("\n--- Admin Menu ---");
                Console.WriteLine("1. View Products");
                Console.WriteLine("2. Add Product");
                Console.WriteLine("3. View Expiry Alerts");
                Console.WriteLine("4. Save & Back to Main Menu ");
                Console.Write("Choose: ");
                string Choice = Console.ReadLine();

                // Check the user's choice and perform the corresponding action
                switch (Choice)
                {
                    case "1":
                        
                        if (DataStore.products.Count == 0)
                        {
                            Console.WriteLine("\nNo products available.\n");
                            continue; // To continue the loop if no products are available
                        }
                        else
                        {
                            // Display all products in the store
                            admin.ViewProducts();
                        }
                        break;

                    case "2":
                        // Input product details
                        InputProductsDetails(admin);
                        break;

                    case "3":
                        // View products that are near expiry
                        Console.WriteLine("\n--- Products near expiry ---\n");
                        admin.viewExpiryAlerts();
                        break;

                    case "4":
                        // Save products to file and exit
                        DataStore.saveProducts();
                        Console.WriteLine("\nProducts saved. Exit from the list of Admin...\n");
                        return;

                    default:
                        // Handle invalid choice
                        Console.WriteLine("\nInvalid choice. Please try again.\n");
                        break;
                }
                
            }
        }
        //Malak & Habeba
        // Method to input product details
        public static void InputProductsDetails(Admin admin)
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
        //Malak & Habeba
        // Customer menu
        public static void CustomerMenu(Customer customer)
        {
            while (true)
            {
                // Display the customer menu options
                Console.WriteLine("\n--- Customer Menu ---");
                Console.WriteLine("1. View Products");
                Console.WriteLine("2. Buy Product");
                Console.WriteLine("3. View My Purchases");
                Console.WriteLine("4.Back to Main Menu");
                Console.Write("Choose: ");
                string Choice = Console.ReadLine();

                switch (Choice)
                {
                    case "1":
                        // Display all available products
                        if (DataStore.products.Count == 0)
                        {
                            Console.WriteLine("No products available.");
                        }
                        else
                        {
                            Console.WriteLine("\n--- Available Products ---");
                            foreach (Product p in DataStore.products)
                            {
                                p.Display();
                            }
                        }
                        break;

                    case "2":
                        // Buy a product

                        while (true)
                        {
                            
                            Console.WriteLine("\n--- Buy Product ---");
                            Console.Write("Enter Product Name: ");
                            string pname = Console.ReadLine();
                            
                            Console.Write("Enter Quantity to Buy: ");
                            int qty;
                            if (!int.TryParse(Console.ReadLine(), out qty) || qty <= 0)
                            {
                                Console.WriteLine("\nInvalid quantity.  Please try again.\n");
                                continue;
                            }
                            if (string.IsNullOrEmpty(pname))
                            {
                                Console.WriteLine("\nProduct name cannot be empty.  Please try again.\n");
                                continue;
                            }
                            // Find the product in the DataStore
                            Product product = null;

                            foreach (Product p in DataStore.products)
                            {
                                if ((p.Name.ToLower().Trim()) == (pname.ToLower().Trim()))
                                {
                                    product = p;
                                    break;
                                }
                            }
                            // Check if the product exists
                            if (product != null)
                            {
                                // Check if the requested quantity is available
                                while (true)
                                {
                                    if (product.Quantity >= qty)
                                    {
                                        customer.buyProduct(product, qty);
                                        break; 
                                    }
                                    else
                                    {
                                        Console.WriteLine($"\nNot enough stock. Only {product.Quantity} available.");
                                        if (product.Quantity > 0)
                                        {
                                            Console.Write("Do you want to buy a smaller quantity (y/n)? ");
                                            string response = Console.ReadLine().ToLower();
                                            if (response == "y")
                                            {
                                                Console.Write($"Enter a smaller quantity (max {product.Quantity}): ");
                                                if (!int.TryParse(Console.ReadLine(), out qty) || qty <= 0)
                                                {
                                                    Console.WriteLine("\nInvalid quantity. Cancelling purchase.");
                                                    break;
                                                }
                                            }
                                            else if (response == "n")
                                            {
                                                Console.WriteLine("\nCancelling purchase.");
                                                break; 
                                            }
                                            else
                                            {
                                                Console.WriteLine("\nInvalid response. Cancelling purchase.");
                                                break; 
                                            }

                                            
                                        }
                                        else
                                        {
                                            Console.WriteLine("\nThis product is completely out of stock.");
                                            break;
                                        }
                                    }
                                }

                                break; // exit main buy loop after finishing purchase attempt
                            }
                            else
                            {
                                Console.WriteLine("\nProduct not found. Please try again.\n");
                                continue; // ask again
                            }
                        }

                    break;

                    case "3":
                        // View customer's purchases
                        customer.displayCart();
                    break;

                    case "4":
                        // Back to main menu
                        Console.WriteLine("Exit from the list of Customer...\n");
                        DataStore.saveProducts();
                    return;

                    default:
                        // Handle invalid choice
                        Console.WriteLine("Invalid choice. Please try again.");
                    break;
                }

            }
        }
        
    }

}
