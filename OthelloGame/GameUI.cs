using System;

namespace Othello
{
    class GameUI
    {
        public static void DisplayWelcomeMessage()
        {
            Console.WriteLine("Welcome to Othello!" + Environment.NewLine);
        }

        public static string PromptForPlayerName(string prompt)
        {
            Console.Write(prompt);
            string name = Console.ReadLine();
            while (string.IsNullOrEmpty(name))
            {
                Console.Write($"Invalid Name. {prompt}");
                name = Console.ReadLine();
            }
            return name;
        }

        public static string PromptForOpponent()
        {
            Console.Write("Choose your opponent: type '1' to play against the computer or '2' to play against a human: ");
            return Console.ReadLine();
        }

        public static int PromptForBoardSize()
        {
            Console.Write("Choose board NxN size (6 or 8): ");
            string userInput;
            bool isBoardSizeValid;

            do
            {
                userInput = Console.ReadLine();
                isBoardSizeValid = GameManager.CheckIfBoardSizeValid(userInput);
                if (!isBoardSizeValid)
                {
                    Console.Write("Invalid board size. Choose board NxN size (6 or 8): ");
                }
            } while (!isBoardSizeValid);

            int.TryParse(userInput, out int boardSize);
            return boardSize;
        }

        public static (bool, string) RequestForMoveOrExit(Player currentPlayer)
        {
            Console.Write($"Type 'Q' to quit the game or choose a cell (e.g., E3) to place your disc '{currentPlayer.PlayerDisc}': ");
            string userInput = Console.ReadLine();
            bool isExit = userInput.ToUpper() == "Q";

            if (isExit)
            {
                Console.WriteLine("You chose to exit the game. Goodbye!");
            }

            return (isExit, userInput.ToUpper());
        }

        public static void PrintBoard(Board board)
        {
            char[,] boardArray = board.BoardArray;

            for (int rowIndex = 0; rowIndex < boardArray.GetLength(0); rowIndex++)
            {
                for (int colIndex = 0; colIndex < boardArray.GetLength(1); colIndex++)
                {
                    Console.Write(boardArray[rowIndex, colIndex]);
                }

                Console.WriteLine();
            }
        }

        public static void AskUserForRematchOrExit(ref bool exitGame)
        {
            Console.WriteLine("Press 'Q' to exit or any other key to play again.");
            string userChoice = Console.ReadLine();
            bool isExit = userChoice.ToUpper() == "Q";

            if (isExit)
            {
                Console.WriteLine("Exiting the game...");
                exitGame = true;
            }
            else
            {
                Console.WriteLine("Starting a new game...");
            }
        }
    }
}
