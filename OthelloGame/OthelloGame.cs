using System;

namespace Othello
{
    class OthelloGame
    {
        public static GameManager s_GameManager;

        public static void InitializeGame()
        {
            GameUI.DisplayWelcomeMessage();

            // Prompt for player names and board size once
            string player1Name = GameUI.PromptForPlayerName("Enter Player 1's name: ");
            string isComputer = GameUI.PromptForOpponent();
            string player2Name;

            if (isComputer == "2")
            {
                player2Name = GameUI.PromptForPlayerName("Enter Player 2's name: ");
            }
            else
            {
                player2Name = ""; // Empty string indicates playing against the computer
            }

            int boardSize = GameUI.PromptForBoardSize();

            // Initialize the GameManager once with the provided players and board size
            s_GameManager = new GameManager(boardSize, player1Name, player2Name);

            bool exitGame = false;

            // Game loop to handle multiple games
            while (!exitGame)
            {
                s_GameManager.StartGame(); // No arguments needed

                // After the game ends, ask the user if they want to play again
                GameUI.AskUserForRematchOrExit(ref exitGame);

                if (!exitGame)
                {
                    s_GameManager.ReinitializeBoard(); // Reset the game state for a new game
                }
            }

            Console.WriteLine("Thank you for playing!");
        }
    }
}
