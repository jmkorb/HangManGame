
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Hangman
{
    public class ProgramUI
    {
        string frame0 = "__________";
        string frame1 = "|        |";
        string frame2 = "|";
        string frame3 = "|";
        string frame4 = "|";
        string frame5 = "|";
        string frame6 = "|_________";

        string wrongGuess0 = "|        O";
        string wrongGuess1 = "|       / ";
        string wrongGuess2 = "|       /|";
        string wrongGuess3 = @"|       /|\";
        string wrongGuess4 = "|        |";
        string wrongGuess5 = "|       /";
        string wrongGuess6 = @"|       / \";

        public void Run()
        {
            List<string> currentFramework = new List<string> { frame0, frame1, frame2, frame3, frame4, frame5, frame6 };
            List<string> deadmanFrame = new List<string> { wrongGuess0, wrongGuess1, wrongGuess2, wrongGuess3, wrongGuess4, wrongGuess5, wrongGuess6 };

            List<char> guessedLettersList = new List<char>();

            GameText("Welcome to Hangman!\n" +
                        "Player 1, Enter your name:");
            string playerOneName = Console.ReadLine();

            Console.WriteLine("Player 2, Enter your name:");
            string playerTwoName = Console.ReadLine();

            Console.Clear();
            GameText($"Hi {playerOneName}\n" +
                $"Would you like to:\n" +
                $"1. Get a random word/phrase, or\n" +
                $"2. Enter your own word/phrase for {playerTwoName} to guess?");
            string guessingWord = default;

            bool getProperInput = true;
            string switchCase = default;
            while (getProperInput)
            {
                switchCase = Console.ReadLine();
                switch (switchCase)
                {
                    case "1":
                        guessingWord = RandomWords();
                        getProperInput = false;
                        break;
                    case "2":
                        GameText("Please enter a word/phrase:");
                        guessingWord = ProperWord();
                        getProperInput = false;
                        break;
                    default:
                        PrintError("Please select 1 or 2.");
                        break;
                }
            }


            List<char> currentBoard = new List<char>();
            //creating the blank board to display to player 2, including spaces or symbols
            for(int i = 0; i < guessingWord.Length; i++)
            {
                if (hasSpecialChar(guessingWord[i]))
                {
                    currentBoard.Add(guessingWord[i]);
                }
                else
                {

                    currentBoard.Add('_');
                }
            }
            
            GameText("\n\n" +
                $"The word to guess is: {guessingWord}\n\n" +
                $"Press any key to continue, and then pass to {playerTwoName}...");
            Console.ReadKey();

            Console.Clear();

            GameText("\n\n"+
                $"{playerTwoName}, there are {guessingWord.Length} letters in {playerOneName}'s word/phrase.");
            int wrongAnswerCount = 0;
            bool keepGuessing = true;

            while (keepGuessing)
            {
                GameText($"{playerTwoName}, guess a letter.");

                PrintCurrentFrame(currentFramework);
                BoardColor(string.Join(" ", currentBoard));

                if (!currentBoard.Contains('_'))
                {
                    PrintHappy($"Congrats, you won! The answer was {guessingWord}.");
                    keepGuessing = false;
                    PlayAgain();
                    break;
                };

                string player2Guess = Console.ReadLine();
                char guessedLetter = default;
                bool checkParse = char.TryParse(player2Guess, out char result);
                if (checkParse)
                {
                    guessedLetter = char.Parse(player2Guess);
                }

                Console.Clear();

                if (player2Guess.Length > 1)
                {
                    PrintError("One letter at a time, please.");
                }
                else if (player2Guess == string.Empty)
                {
                    PrintError("Please guess a letter.");
                }

                if (guessedLettersList.Contains(guessedLetter))
                {
                    PrintError("You've already guessed this letter. Try again.");
                }
                else if (hasSpecialChar(guessedLetter) || char.IsDigit(guessedLetter))
                {
                    PrintError("Only letters, no numbers or special characters");
                }
                else if (player2Guess.Length == 1)
                {
                    guessedLettersList.Add(guessedLetter);
                    bool isNotInWord = true;

                    for (int i = 0; i < guessingWord.Length; i++)
                    {
                        if (guessedLetter == guessingWord[i])
                        {
                            currentBoard[i] = guessedLetter;
                            isNotInWord = false;
                        }
                    }

                    if (!isNotInWord)
                    {
                        PrintHappy($"Correct! {guessedLetter} is in the answer!");
                    }
                    else if (isNotInWord)
                    {
                        
                        PrintError($"Wrong! There's no {guessedLetter} in the answer.");
                        //IncorrectGuesses();
                        if(wrongAnswerCount == 0)
                        {
                            currentFramework[2] = deadmanFrame[wrongAnswerCount];
                            wrongAnswerCount++;
                        }
                        else if(wrongAnswerCount < 4)
                        {
                            currentFramework[3] = deadmanFrame[wrongAnswerCount];
                            wrongAnswerCount++;
                        }
                        else if(wrongAnswerCount == 4)
                        {
                            currentFramework[4] = deadmanFrame[wrongAnswerCount];
                            wrongAnswerCount++;
                        }
                        else if(wrongAnswerCount == 5)
                        {
                            currentFramework[5] = deadmanFrame[wrongAnswerCount];
                            wrongAnswerCount++;
                        }
                        else if (wrongAnswerCount == 6)
                        {
                            currentFramework[5] = deadmanFrame[wrongAnswerCount];
                            PrintError("One more wrong guess before you lose.");
                            wrongAnswerCount++;
                        }
                        else if(wrongAnswerCount > 6)
                        {
                            PrintError($"You Lost. The answer was '{guessingWord}'.");
                            keepGuessing = false;
                            PlayAgain();
                        }
                    }
                }
            }
        }
        private void PlayAgain()
        {
            GameText("\nWould you like to play again? y/n");
            bool keepAsking = true;
            while (keepAsking)
            {
                string playAgain = Console.ReadLine().ToLower();
                if (playAgain == "y")
                {
                    keepAsking = false;
                    Console.Clear();
                    PrintHappy("Run it back!");

                    Run();
                }
                else if(playAgain == "n")
                {
                    keepAsking = false;
                    GameText("Thank you for playing!\n" +
                        "Press any key to exit...");
                    Console.ReadKey();
                }
                else
                {
                    GameText("Enter y or n");
                }
            }
        }


        private string RandomWords()
        {
            List<string> words = new List<string> {"pineapple", "selling sea shells", "chocolate", "spectacular",
                "crazy glue", "french bread", "canned peaches", "la croix", "superman", "labrador retriever",
                "apple sauce", "chimichanga", "legoland", "mickey mouse", "red dead redemption", "captain crunch",
                "porcupine", "captain jack sparrow", "quest", "umbrella", "utopia", "pangea", "highlighter", "road rage",
                "tandom bike", "new york city", "headphones", "graduation", "the one ring", "this is the way", "laser pointer",
                "professor oak", "indianapolis", "broken bone", "recursion", "fairy godmother", "eraser", "snoopy", "willy wonka",
                "pine tree", "eleven fifty academy", "phil smith", "terry brown", "where's waldo", "captain ahab", "charles dickens",
                "alex trebek", "whose line is it anyway?", "artemis", "apollo", "ice cream", "fireworks", "chuck e. cheese",
                "computer virus", "tattoo", "minneapolis", "sacramento", "los angeles", "mario and luigi", "jacob korb", "gaby costilla",
                };
            Random rand = new Random();
            return words[rand.Next(words.Count)];
        }

        private string ProperWord()
        {
            bool properWord = true;
            string inputWord = default;

            while (properWord)
            {
                inputWord = Console.ReadLine().ToLower();
                bool containsInt = inputWord.Any(char.IsDigit);
                if (containsInt)
                {
                    PrintError("No numbers allowed within your word/phrase. Please try again.");
                }
                else if (inputWord == string.Empty)
                {
                    PrintError("You need to input a word.");
                }
                else
                {
                    properWord = false;
                }
            }
            return inputWord;
        }
        private void PrintCurrentFrame(List<string> frameList)
        {
            foreach (string framePiece in frameList)
            {
                BoardColor(framePiece);
            }
        }

        private static bool hasSpecialChar(char letter)
        {
            string specialChar = @"[]+~`\*|!#$%^&/()=?»«@£§€{}.-:;'<>_, ";
            foreach (var item in specialChar)
            {
                if (item == letter) return true;
            }
            return false;
        }

        private void PrintError(string error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(error);
        }

        private void PrintHappy(string happyText)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(happyText);
        }
        private void GameText(string text)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(text);
        }

        private void BoardColor(string board)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(board);
        }
    }
}
