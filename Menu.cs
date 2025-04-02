using System;
using CatOne;
using CatTwo;
using CatThree;
using CatFour;
using CatFive;

namespace Menu
{
    public static class FoodMenu
    {
        public static void Food()
        {
            bool exitMenu = false;
            do
            {
                Console.WriteLine("=============================================== \nWelcome to the Food Menu ===============================================");
                Console.WriteLine("[1]. Breakfast\n[2]. Lunch\n[3]. Dinner\n[4]. Dessert\n[5]. Drinks\n[6]. Exit\n");
                Console.Write("Food Category: ");
                string foodCategory = Console.ReadLine() ?? string.Empty;

                switch (foodCategory)
                {
                    case "1":
                        CatBreakfast.Breakfast();
                        break;
                    case "2":
                        CatLunch.Lunch();
                        break;
                    case "3":
                        CatDinner.Dinner();
                        break;
                    case "4":
                        CatDessert.Dessert();
                        break;
                    case "5":
                        CatDrinks.Drinks();
                        break;
                    case "6":
                        exitMenu = true;
                        Console.WriteLine("Exiting the Food Menu. Thank you!");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                if (!exitMenu)
                {
                    Console.Write("\nWould you like to select another category? (Y/N): ");
                    string continueChoice = Console.ReadLine() ?? string.Empty;
                    exitMenu = continueChoice.Equals("N", StringComparison.CurrentCultureIgnoreCase);
                }

            } while (!exitMenu);
        }
    }
}