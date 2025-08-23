using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static supermarket_management.Program;

namespace supermarket_management
{
    
    // Product class to represent a product in the supermarket
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
    
    // DataStore class to manage products and admins
    public static class DataStore
    {
        public static List<Product> products = new List<Product>();
        // static List<Order> customerOrders = new List<Order>();
        public static List<Admin> admins = new List<Admin>();
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
    
    // Order class to represent a customer's order in the supermarket
    public class Order
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public Order(string productName, int quantity)
        {
            ProductName = productName;
            Quantity = quantity;
        }

        public void DisplayProductInCatt()
        {
            Console.WriteLine($"- {ProductName} (qty:{Quantity})");
        }
    }
    
    // Customer class to represent a customer in the supermarket
    public class Customer
    {
        public string name { get; set; }
        public List<Order> cart { get; set; }

        public Customer(string Name)
        {
            Name = name;
            cart = new List<Order>();
        }
        // Method to add a product to the customer's cart
        public void buyProduct(Product p, int quantity)
        {
            if (p.Quantity >= quantity)
            {
                p.Quantity -= quantity;
                // To add the purchased product to the customer's cart
                cart.Add(new Order(p.Name,quantity));

                Console.WriteLine($"You have successfully purchased {quantity} of {p.Name}!");
            }
            else
            {
                Console.WriteLine($"! Purchase Failed ! Not enough {p.Name} in stock");
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
    
    // Admin class to manage products in the supermarket
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
            Product existingProduct = null;

            foreach (Product prod in DataStore.products)
            {
                if (prod.Name == p.Name && prod.Category == p.Category)
                {
                    existingProduct = prod;
                    break;
                }
            }

            if (existingProduct != null)
            {
                existingProduct.Quantity += q;
                Console.WriteLine($"Updated {existingProduct.Name}, new quantity = {existingProduct.Quantity}");
            }
            else
            {
                DataStore.products.Add(p);
                Console.WriteLine($"Added new product: {p.Name}");
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
                    Admin admin = CreateAdmin(); // Here we can use the created admin object
                    DataStore.admins.Add(admin);// To add the created admin to the list
                    AdminMenu(admin);// To call the AdminMenu method with the created admin object
                }
                else if (choice == "2")
                {
                    Admin admin = LoginAdmin();// To call the LoginAdmin method to login as admin
                    if (admin != null)
                    {
                        AdminMenu(admin);
                    }
                    
                }
                else if (choice == "3")
                {
                    Customer customer = new Customer("Customer"); // Create a new customer object
                    CustomerMenu(customer);
                }
                else
                {
                    Console.WriteLine("Exiting.......");
                    return;
                }
            }
        }
        // Method to create an admin account
        public static Admin CreateAdmin()
        {
            string name, pass;
            while (true)
            {
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
                
                Admin admin = DataStore.admins.Find(a => a.username == user && a.password == pass);

                if (admin != null)
                {
                    Console.WriteLine("\nLogged in successfully!");
                    return admin;
                }

                else
                {
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
        //Admin menu
        public static void AdminMenu(Admin admin)
        {
            while (true)
            {
                Console.WriteLine("\n--- Admin Menu ---");
                Console.WriteLine("1. View Products");
                Console.WriteLine("2. Add Product");
                Console.WriteLine("3. View Expiry Alerts");
                Console.WriteLine("4. Save & Back to Main Menu ");
                Console.Write("Choose: ");
                string Choice = Console.ReadLine();

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
                        Console.WriteLine("\nInvalid choice. Please try again.\n");
                        break;
                }
                if (Choice == "4")
                {
                    break; // To exit the admin menu
                }
            }
        }
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
        // Customer menu
        public static void CustomerMenu(Customer customer)
        {
            while (true)
            {
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

                        while(true)
                        {
                            Console.WriteLine("\n--- Buy Product ---");
                            Console.Write("Enter Product Name: ");
                            string pname = Console.ReadLine();

                            Console.Write("Enter Quantity to Buy: ");
                            int qty;
                            if (!int.TryParse(Console.ReadLine(), out qty) || qty <= 0)
                            {
                                Console.WriteLine("Invalid quantity.  Please try again.");
                                continue;
                            }
                            if (string.IsNullOrEmpty(pname))
                            {
                                Console.WriteLine("Product name cannot be empty.  Please try again.");
                                continue;
                            }
                            Product product = null;

                            foreach (Product p in DataStore.products)
                            {
                                if (p.Name == pname)
                                {
                                    product = p;
                                    break;
                                }
                            }
                            if (product != null)
                            {
                                customer.buyProduct(product, qty);
                                break;
                            }
                            else
                                Console.WriteLine("Product not found. Please try again.");
                        }

                        break;

                    case "3":
                        // View customer's purchases
                        customer.displayCart();
                        break;

                    case "4":
                        // Back to main menu
                        DataStore.saveProducts();
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

            }
        }
        
    }

}
