using System;
using System.Linq;
using System.Threading;

namespace Club
{
    // Utility class to handle text color
    static class Utility
    {
        public static void SetTextColor(ConsoleColor color) => Console.ForegroundColor = color;
        public static void ResetTextColor() => Console.ResetColor();

        public static void Clubname()
        {
            SetTextColor(ConsoleColor.Cyan);
            Console.WriteLine(" ******************************* ");
            Console.WriteLine(" *                             * ");
            Console.WriteLine(" *   Welcome to the BJK CLUB   * ");
            Console.WriteLine(" *                             * ");
            Console.WriteLine(" ******************************* ");
            ResetTextColor();
            Console.WriteLine();
        }
    }

    // Abstract base class for all games
    abstract class Game
    {
        public abstract void StartGame(ref int totalMoney, string name = "");
    }

    // 4-Digit Code Guessing Game
    class CodeGuessingGame : Game
    {
        public override void StartGame(ref int totalMoney, string name = "")
        {
            Console.Clear();
            Utility.SetTextColor(ConsoleColor.Yellow);
            Console.WriteLine("Welcome to the 4-Digit Code Guessing Game!");
            Utility.ResetTextColor();

            // Asking for the bet
            Console.Write("\nEnter your bet amount: ");
            int betAmount;
            while (!int.TryParse(Console.ReadLine(), out betAmount) || betAmount <= 0 || betAmount > totalMoney)
            {
                Console.WriteLine($"Invalid bet. Please bet an amount between 1 and {totalMoney}: ");
            }

            Random rand = new Random();
            int code = rand.Next(1000, 10000); // Generates a 4-digit code

            int attempts = 0;
            bool guessedCorrectly = false;

            while (attempts < 5 && !guessedCorrectly)
            {
                Console.Write("\nGuess the 4-digit code: ");
                string input = Console.ReadLine();

                Console.Clear(); // Clear the console after each input

                if (int.TryParse(input, out int playerGuess) && playerGuess >= 1000 && playerGuess <= 9999)
                {
                    attempts++;

                    string feedback = CompareCode(code, playerGuess);

                    // Displaying the input digits in a vertical column with bars
                    for (int i = 0; i < 4; i++)
                    {
                        char digit = input[i];
                        Console.WriteLine("   +-------+ ");
                        if (feedback[i] == 'C') // Correct number and correct position (green)
                        {
                            Utility.SetTextColor(ConsoleColor.Green);
                        }
                        else if (feedback[i] == 'P') // Correct number but wrong position (yellow)
                        {
                            Utility.SetTextColor(ConsoleColor.Yellow);
                        }
                        else // Incorrect number (red)
                        {
                            Utility.SetTextColor(ConsoleColor.Red);
                        }

                        Console.WriteLine($"   |   {digit}   | ");
                        Utility.ResetTextColor();
                        Console.WriteLine("   +-------+ ");
                    }

                    if (playerGuess == code)
                    {
                        guessedCorrectly = true;
                        Console.WriteLine("Correct! You've guessed the code.");
                        totalMoney += betAmount; // Win the bet if they guess correctly
                        Console.WriteLine($"You won {betAmount}! Your new balance: {totalMoney}");
                    }
                    else
                    {
                        if (attempts == 5)
                        {
                            // After 5 incorrect attempts, show the message and reveal the correct code
                            Utility.SetTextColor(ConsoleColor.Yellow);
                            Console.WriteLine("\nWow, you must have really bad luck, huh? BANG BANG BANG");
                            Utility.ResetTextColor();
                            Console.WriteLine($"The correct code was: {code}");
                            Thread.Sleep(2000); // Wait for 2 seconds before exiting
                            Environment.Exit(0); // Exit if the player loses after 5 attempts
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a 4-digit number.");
                }
            }
        }

        // Compare the player's guess with the correct code and provide feedback
        private string CompareCode(int correctCode, int playerGuess)
        {
            char[] feedback = new char[4];
            string correctCodeStr = correctCode.ToString();
            string playerGuessStr = playerGuess.ToString();

            for (int i = 0; i < 4; i++)
            {
                if (playerGuessStr[i] == correctCodeStr[i])
                {
                    feedback[i] = 'C'; // Correct number and position
                }
                else if (correctCodeStr.Contains(playerGuessStr[i]))
                {
                    feedback[i] = 'P'; // Correct number but wrong position
                }
                else
                {
                    feedback[i] = 'I'; // Incorrect number
                }
            }
            return new string(feedback);
        }
    }

    // Russian Roulette Game
    class RussianRoulette : Game
    {
        public override void StartGame(ref int totalMoney, string name)
        {
            Random rand = new Random();
            int[] chamber = { 0, 0, 1, 0, 0, 1 }; // 2/6 chance of the gun firing
            chamber = chamber.OrderBy(x => rand.Next()).ToArray(); // Shuffle the chambers

            int currentChamber = 0;
            Console.Clear();
            Utility.SetTextColor(ConsoleColor.Red);
            Console.WriteLine($"Welcome to Russian Roulette, {name}. You have a 2/6 chance of dying.");
            Utility.ResetTextColor();

            Console.Write("\nEnter your bet amount: ");
            int betAmount;
            while (!int.TryParse(Console.ReadLine(), out betAmount) || betAmount <= 0 || betAmount > totalMoney)
            {
                Console.WriteLine($"Invalid bet. Please bet an amount between 1 and {totalMoney}: ");
            }

            Console.Clear();

            while (true)
            {
                Console.WriteLine("\n1: Spin the barrel");
                Console.WriteLine("2: Pull the trigger");
                Console.WriteLine("3: Leave the game");

                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine() ?? "";

                Console.Clear(); // Clear the console after each input

                switch (choice)
                {
                    case "1":
                        currentChamber = rand.Next(0, 6); // Spin the barrel randomly
                        Console.WriteLine("You spin the barrel... It's random now.");
                        break;

                    case "2":
                        Console.WriteLine("You pull the trigger...");
                        Thread.Sleep(2000); // Add a small delay for effect

                        if (chamber[currentChamber] == 1)
                        {
                            Utility.SetTextColor(ConsoleColor.Red);
                            Console.WriteLine("BANG! You're dead.");
                            Utility.ResetTextColor();
                            Thread.Sleep(3000);
                            Environment.Exit(0); // Exit if the player dies
                        }
                        else
                        {
                            Utility.SetTextColor(ConsoleColor.Green);
                            Console.WriteLine("Click... You survived.");
                            Utility.ResetTextColor();
                            totalMoney += betAmount; // Win the bet if they survive
                            Console.WriteLine($"You won {betAmount}! Your new balance: {totalMoney}");
                            currentChamber = (currentChamber + 1) % 6; // Move to the next chamber
                        }
                        break;

                    case "3":
                        Console.WriteLine("You leave the Russian Roulette game.");
                        return;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }
    }

    // Rock Paper Scissors Game
    class RockPaperScissors : Game
    {
        public override void StartGame(ref int totalMoney, string name)
        {
            string[] choices = { "rock", "paper", "scissors" }; // *** ARRAY USED HERE ***
            Random rand = new Random();

            Console.Clear();
            Console.WriteLine("Welcome to Rock Paper Scissors!");

            Console.Write("\nEnter your bet amount: ");
            int betAmount;
            while (!int.TryParse(Console.ReadLine(), out betAmount) || betAmount <= 0 || betAmount > totalMoney)
            {
                Console.WriteLine($"Invalid bet. Please bet an amount between 1 and {totalMoney}: ");
            }

            Console.Clear();

            while (true)
            {
                Console.Write("\nChoose (rock, paper, scissors) or type 'exit' to leave: ");
                string playerChoice = Console.ReadLine()?.ToLower() ?? ""; // Handle null

                Console.Clear(); // Clear the console after each input

                if (playerChoice == "exit")
                {
                    Console.WriteLine("You leave the Rock Paper Scissors game.");
                    return;
                }

                if (!choices.Contains(playerChoice))
                {
                    Console.WriteLine("Invalid choice. Try again.");
                    continue;
                }

                string computerChoice = choices[rand.Next(0, 3)];
                Console.WriteLine($"Opponent chose: {computerChoice}");

                // Determine the winner
                if (playerChoice == computerChoice)
                {
                    Utility.SetTextColor(ConsoleColor.Yellow);
                    Console.WriteLine("It's a tie!");
                    Utility.ResetTextColor();
                }
                else if ((playerChoice == "rock" && computerChoice == "scissors") ||
                         (playerChoice == "scissors" && computerChoice == "paper") ||
                         (playerChoice == "paper" && computerChoice == "rock"))
                {
                    Utility.SetTextColor(ConsoleColor.Green);
                    Console.WriteLine("You win!");
                    Utility.ResetTextColor();
                    totalMoney += betAmount; // Win the bet if they win
                    Console.WriteLine($"You won {betAmount}! Your new balance: {totalMoney}");
                }
                else
                {
                    Utility.SetTextColor(ConsoleColor.Red);
                    Console.WriteLine("You lose!");
                    Utility.ResetTextColor();
                    Thread.Sleep(2000);
                    Environment.Exit(0); // Exit if the player loses
                }

                // Ask if the player wants to continue
                if (totalMoney <= 0)
                {
                    Utility.SetTextColor(ConsoleColor.Red);
                    Console.WriteLine("You have no money left. The game is over.");
                    Utility.ResetTextColor();
                    Thread.Sleep(2000);
                    Environment.Exit(0); // Exit if the player runs out of money
                }
                else
                {
                    Console.Write("\nDo you want to play again? (yes/no): ");
                    string playAgain = Console.ReadLine()?.ToLower() ?? "no";

                    if (playAgain != "yes")
                    {
                        Console.WriteLine("You leave the Rock Paper Scissors game.");
                        return;
                    }
                }
            }
        }
    }

    // Main Program class with entry point
    class Program
    {
        static void Main(string[] args)
        {
            string playerName = "";
            int totalMoney = 0;

            // Ask for the player's name
            Console.Write("Enter your name: ");
            playerName = Console.ReadLine() ?? ""; // Use null-coalescing to handle null input

            // Ask for the player's starting money
            Console.Write("Enter your starting money: ");
            while (!int.TryParse(Console.ReadLine(), out totalMoney) || totalMoney <= 0)
            {
                Console.WriteLine("Invalid amount. Please enter a positive number: ");
            }

            Console.Clear();

            Utility.Clubname();

            Utility.SetTextColor(ConsoleColor.Cyan);
            Console.WriteLine($"Hi {playerName}, welcome to BJK CLUB!");
            Utility.ResetTextColor();

            // Main Menu loop
            while (true)
            {
                if (totalMoney <= 0)
                {
                    Utility.SetTextColor(ConsoleColor.Red);
                    Console.WriteLine("You lost all your money and need to GET OUT!");
                    Utility.ResetTextColor();
                    Thread.Sleep(2000);
                    Environment.Exit(0);
                }

                // Color the main options
                Utility.SetTextColor(ConsoleColor.Red);
                Console.WriteLine("\n1: Go upstairs (Russian Roulette)");
                Utility.ResetTextColor();

                Utility.SetTextColor(ConsoleColor.Green);
                Console.WriteLine("2: Go behind the stairs (Rock Paper Scissors)");
                Utility.ResetTextColor();

                Utility.SetTextColor(ConsoleColor.Blue);
                Console.WriteLine("3: Stay here");
                Utility.ResetTextColor();

                Utility.SetTextColor(ConsoleColor.Yellow);
                Console.WriteLine("4: Play the 4-digit code guessing game");
                Utility.ResetTextColor();

                Console.WriteLine("5: Leave");

                Console.Write("Enter your choice: ");
                string mainMenuChoice = Console.ReadLine() ?? "";

                switch (mainMenuChoice)
                {
                    case "1":
                        Console.Clear();
                        Console.WriteLine("You go upstairs and find a mysterious game of Russian Roulette...");
                        new RussianRoulette().StartGame(ref totalMoney, playerName);
                        break;

                    case "2":
                        Console.Clear();
                        Console.WriteLine("You went behind the stairs.");
                        Console.Write("Enter the secret code: ");
                        if (Console.ReadLine() == "rps123")
                        {
                            Console.Clear();
                            new RockPaperScissors().StartGame(ref totalMoney, playerName);
                        }
                        else
                        {
                            Utility.SetTextColor(ConsoleColor.Red);
                            Console.WriteLine("Invalid code. You cannot enter the secret room.");
                            Utility.ResetTextColor();
                        }
                        break;

                    case "3":
                        Console.Clear();
                        Console.WriteLine("You stayed here.");
                        break;

                    case "4":
                        // Welcome message for the 4-digit guessing game
                        Console.Clear();
                        Console.WriteLine("Welcome to the 4-Digit Code Guessing Game!");
                        new CodeGuessingGame().StartGame(ref totalMoney, playerName);
                        break;

                    case "5":
                        Console.Clear();
                        Console.WriteLine("You left the club.");
                        Environment.Exit(0);
                        break;

                    default:
                        Utility.SetTextColor(ConsoleColor.Yellow);
                        Console.WriteLine("Invalid input.");
                        Utility.ResetTextColor();
                        break;
                }
            }
        }
    }
}
