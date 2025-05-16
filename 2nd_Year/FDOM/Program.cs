using System;
using EntryPoint.Utilities;

namespace mainUI
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            string[] roles = { "Manager", "Dispatcher", "Customer", "Exit" };
            userChoice.DisplayMenu("Welcome to Food Delivery Order Manager!", roles, MainProgram.HandleMainMenuSelection);
        }
    }
}