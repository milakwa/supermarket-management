using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace supermarket_management
{
    class P
    {
        static void Main(string[] args)
        {
            displayCart();
        }
        static void displayCart()
        {
            string[] itemsBought = new string[] {" Bread  ", " Eggs   ", " Chips  ", " Pasta  ", " Soda   ", " cereal ", " Bananas", " Butter ",
            " Sugar  ", "Milk   ", "Yogurt "};
            Console.WriteLine("\n\n\t\t\t\t    --* Cart *--");
            Console.WriteLine("\n\t\t\t |  No.  | |    Items purchased    |\n");

            for (int i = 0; i < itemsBought.Length; i++)
            {
                Console.WriteLine($"\t\t\t |   {i + 1}   | |        {itemsBought[i]}       |\n");
            }
        }

    }
    public class Customer
    {
        public string Name { get; set; }
        public List<Product> cart { get; set; }

        public Customer(string name)
        {
            Name = name;
            cart = new List<Product>();
        }
        public void buyProduct(Product p, int quantity)
        {
            for (int i = 0; i < quantity; i++)
            {
                cart.Add(p);
            }
        }
    }
    /* public void AddProduct1(Product p, int q)
     {


         string path = @"C:\Users\sohil\Documents\Products.txt";
         using (StreamWriter sw = new StreamWriter(path, true))
         {
             sw.WriteLine($"{p.name},{p.category},{p.quantity},{p.productionDate},{p.expiryDate}");
         }

         Console.WriteLine("Product Added");
     }
     public void ViewProducts()
     {
         string path = @"C:\Users\sohil\Documents\Products.txt";
         string[] alllines = File.ReadAllLines(path);
         Console.WriteLine("Products List :");
         foreach (string line in alllines)
         {
             Console.WriteLine(line);
         }
     }*/


}

