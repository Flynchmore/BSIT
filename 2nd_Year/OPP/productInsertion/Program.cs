using System;
using OOP;

class MainProgram
{
    public static void Main()
    {
        Console.WriteLine("======================================== Product Insertion ========================================");

        bool addMore = true;

        while (addMore)
        {
            string name = "";
            while (string.IsNullOrWhiteSpace(name))
            {
                Console.Write("Enter product name: ");
                name = Console.ReadLine() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("Product name is required!");
                }
            }

            int price = 0;
            bool validPrice = false;
            while (!validPrice)
            {
                Console.Write("Enter product price: ");
                string priceInput = Console.ReadLine() ?? string.Empty;
                if (int.TryParse(priceInput, out price) && price > 0)
                {
                    validPrice = true;
                }
                else
                {
                    Console.WriteLine("Please enter a valid price greater than 0.");
                }
            }

            string description = "";
            while (string.IsNullOrWhiteSpace(description))
            {
                Console.Write("Enter product description: ");
                description = Console.ReadLine() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(description))
                {
                    Console.WriteLine("Product description is required!");
                }
            }

            OOP.ManageProduct.InsertNewProduct newProduct = new OOP.ManageProduct.InsertNewProduct();
            newProduct.InsertData(name, price, description);

            Console.Write("Do you want to add another product? (y/n): ");
            string choice = Console.ReadLine() ?? string.Empty .Trim().ToLower();
            addMore = (choice == "y");
            Console.WriteLine();
        }

        Console.WriteLine("Thank you! Exiting program.");
    }
}
