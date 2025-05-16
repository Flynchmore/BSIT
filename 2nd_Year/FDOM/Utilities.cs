using System;
using Manager;
using Dispatcher;
using Customer;

namespace EntryPoint.Utilities{

   public static class ConsoleCenter{
        public static void WriteCentered(string text)
        {
            // Get the current console width
            int consoleWidth = Console.WindowWidth;

            // Calculate the left padding to center the text
            int leftPadding = Math.Max((consoleWidth - text.Length) / 2, 0);

            // Write the text with padding
            Console.WriteLine(new string(' ', leftPadding) + text);
        }
    }

    public static class GetHelper{
        public static string GetCenteredInput(string prompt)
        {
            ConsoleCenter.WriteCentered(prompt);
            return Console.ReadLine() ?? string.Empty;
        }

        public static int GetCenteredIntInput(string prompt)
        {
            ConsoleCenter.WriteCentered(prompt);
            string input = Console.ReadLine() ?? string.Empty;
            return int.TryParse(input, out int result) ? result : 0; // Default to 0 if parsing fails
        }

        public static decimal GetCenteredDecimalInput(string prompt)
        {
            ConsoleCenter.WriteCentered(prompt);
            string input = Console.ReadLine() ?? string.Empty;
            return decimal.TryParse(input, out decimal result) ? result : 0; // Default to 0 if parsing fails
        }
    }

    public static class userChoice{
        public static void DisplayMenu(string title, string[] options, Action<int> handleSelection)
        {
            int selectedIndex = 0;
            ConsoleKey key;

            do
            {
                Console.Clear();
                ConsoleCenter.WriteCentered($"{title}");
                ConsoleCenter.WriteCentered("========================================================");

                // Display menu options
                for (int i = 0; i < options.Length; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        ConsoleCenter.WriteCentered($"➤ {options[i]}");
                        Console.ResetColor();
                    }
                    else
                    {
                        ConsoleCenter.WriteCentered($"  {options[i]}");
                    }
                }

                // Read user input
                key = Console.ReadKey(true).Key;

                // Update the selected index based on arrow key input
                if (key == ConsoleKey.UpArrow)
                {
                    selectedIndex = (selectedIndex - 1 + options.Length) % options.Length;
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    selectedIndex = (selectedIndex + 1) % options.Length;
                }

            } while (key != ConsoleKey.Enter);

            // Perform the selected action
            handleSelection(selectedIndex);
        }
    }
    public static class MainProgram{
        public static void HandleMainMenuSelection(int selectedIndex)
            {
                switch (selectedIndex)
                {
                    case 0: // Manager
                        PromptAccount("Manager", ManagerUI.ManagerLogin, ManagerUI.ManagerRegister);
                        break;

                    case 1: // Dispatcher
                        PromptAccount("Dispatcher", DispatcherUI.DispatcherLogin, DispatcherUI.DispatcherRegister);
                        break;

                    case 2: // Customer
                        PromptAccount("Customer", CustomerUI.CustomerLogin, CustomerUI.CustomerRegister);
                        break;

                    case 3: // Exit
                        ConsoleCenter.WriteCentered("Exiting the program...");
                        Environment.Exit(0);
                        break;
                    default:
                        ConsoleCenter.WriteCentered("Invalid selection. Returning to main menu...");
                        break;
                }
          }

        public static void PromptAccount(string role, Action proceedFunction, Action registerFunction)
        {
            string[] options = { "Yes", "No" };
            userChoice.DisplayMenu($"Do you have an account for the {role} System?", options, HandleAccountPrompt);

            void HandleAccountPrompt(int selectedIndex)
            {
                if (selectedIndex == 0) // Yes
                {
                    ConsoleCenter.WriteCentered($"Redirecting to {role} functionality...");
                    proceedFunction();
                }
                else if (selectedIndex == 1) // No
                {
                    ConsoleCenter.WriteCentered($"Redirecting to {role} registration...");
                    registerFunction();
                }
                else
                {
                    ConsoleCenter.WriteCentered("Invalid selection. Returning to main menu...");
                }
            }
       }
    }
}
