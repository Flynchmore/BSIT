using System;
using EntryPoint.Utilities;

namespace foodCategory;

public class FoodUI
{
    public static void Foodfunction()
    {
        string[] options = { "Breakfast", "Lunch", "Dinner", "Dessert", "Exit" };
        userChoice.DisplayMenu("Welcome to the Food Category System!", options, HandleSelection);
    }

    private static void HandleSelection(int selectedIndex)
    {
        switch (selectedIndex)
        {
            case 0:
                ConsoleCenter.WriteCentered("Breakfast System");
                break;
            case 1:
                ConsoleCenter.WriteCentered("Lunch System");
                break;
            case 2:
                ConsoleCenter.WriteCentered("Dinner System");
                break;
            case 3:
                ConsoleCenter.WriteCentered("Dessert System");
                break;
            case 4:
                ConsoleCenter.WriteCentered("Exiting Food Category System...");
                Environment.Exit(0);
                break;
        }
    }
}