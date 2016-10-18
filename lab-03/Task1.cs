using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace lab_03
{
    public class Task1
    {
        public static void Run()
        {
            var parseDictResult = ParseDict();
            var dict = parseDictResult.Item1;
            var errors = parseDictResult.Item2;

            PrintMenu();
            HandleMenu(dict, errors);

            PreserveCommandPromptExit();
        }

        private static void HandleMenu(Dictionary<string, string> dict, string errors)
        {
            var topChoice = GetTopChoice();
            if (topChoice == -1 || topChoice == 3)
            {
                Console.WriteLine("Do you really want to exit? ");
                var wantExit = GetYesNoChoice();
                if (wantExit)
                {
                    Console.WriteLine("Goodbye");
                    Environment.Exit(0);
                }
                else
                {
                    Console.Clear();
                    PrintMenu();
                    HandleMenu(dict, errors);
                }
            }

            if (topChoice == 1)
            {
                PrintSubMenu(1);
                var subChoice = GetSubChoice();

                if (subChoice == 'e')
                {
                    Console.Clear();
                    PrintMenu();
                    HandleMenu(dict, errors);
                }
                else
                {
                    ShowSectionTranslate(dict, subChoice);
                }
            }
            else if (topChoice == 2)
            {
                PrintSubMenu(2);
                var subChoice = GetSubChoice();

                if (subChoice == 'e')
                {
                    Console.Clear();
                    PrintMenu();
                    HandleMenu(dict, errors);
                }
                else
                {
                    ShowSectionPrint(errors, subChoice);
                }
            }
        }

        private static void ShowSectionTranslate(Dictionary<string, string> dict, char subChoice)
        {
            if (subChoice == 'a')
            {
                Console.Clear();
                Console.WriteLine("Please enter word you want to translate to Ukr");
                var input = Console.ReadLine()?.Trim() ?? "";

                if (dict.ContainsKey(input))
                {
                    Console.WriteLine("Translation: " + dict[input]);
                }
                else
                {
                    Console.WriteLine("There is no such word in the dictionary, do you want to add it? (Y/N)");
                    var wantAdd = GetYesNoChoice();
                    if (wantAdd)
                    {
                        AddWordToDict(input, dict, subChoice);
                    }
                    else
                    {
                        // TODO handle Esc
                        ShowSectionTranslate(dict, subChoice);
                    }
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Please enter word you want to translate to Eng");
                var input = Console.ReadLine()?.Trim() ?? "";

                var translaction = dict.FirstOrDefault(x => x.Value == input).Key;
                if (!string.IsNullOrEmpty(translaction))
                {
                    Console.WriteLine("Translation: " + translaction);
                }
                else
                {
                    Console.WriteLine("There is no such word in the dictionary, do you want to add it? (Y/N)");
                    var wantAdd = GetYesNoChoice();
                    if (wantAdd)
                    {
                        AddWordToDict(input, dict, subChoice);
                    }
                    else
                    {
                        // TODO handle Esc
                        ShowSectionTranslate(dict, subChoice);
                    }
                }
            }
        }

        private static void AddWordToDict(string word, Dictionary<string, string> dict, char subChoice)
        {
            if (subChoice == 'a')
            {
                Console.WriteLine("Please enter Ukr translation to word " + word);
                var translation = Console.ReadLine();

                if (string.IsNullOrEmpty(translation))
                {
                    Console.WriteLine("Cannot be empty! Try again");
                    AddWordToDict(word, dict, subChoice);
                    return;
                }

                dict.Add(word, translation);
            }
            else
            {
                Console.WriteLine("Please enter Eng translation to word " + word);
                var translation = Console.ReadLine();

                if (string.IsNullOrEmpty(translation))
                {
                    Console.WriteLine("Cannot be empty! Try again");
                    AddWordToDict(word, dict, subChoice);
                    return;
                }

                dict.Add(translation, word);
            }

            var dictSerialized = "";

            foreach (var entry in dict)
            {
                dictSerialized += entry.Key + " - " + entry.Value + ";";
            }

            File.WriteAllText("./dict-modified.txt", dictSerialized);
        }

        private static void ShowSectionPrint(string errors, char subChoice)
        {
            if (subChoice == 'a')
            {
                Console.WriteLine(errors);
            }
            else if (subChoice == 'b')
            {
                var tempFileName = Path.GetTempFileName();
                Console.WriteLine("tempFileName " + tempFileName);
                File.WriteAllText(tempFileName, errors);
            }
        }

        private static Tuple<Dictionary<string, string>, string> ParseDict()
        {
            var dict = new Dictionary<string, string>();
            var sb = new StringBuilder();
            var input = File.ReadAllText("./dict.txt");
            var entries = input.Split(';');

            foreach (var entry in entries)
            {
                var parsed = entry.Trim();
                if (parsed.Length == 0)
                {
                    sb.AppendLine("Empty text between semicolons: " + parsed);
                    continue;
                }

                if (parsed.Equals("-"))
                {
                    sb.AppendLine("Only dash between semicolons: " + parsed);
                    continue;
                }

                var parsedParts = parsed.Split('-').Select(part => part.Trim()).ToArray();
                var eng = parsedParts[0];
                var ukr = parsedParts[1];

                if (string.IsNullOrEmpty(eng) || string.IsNullOrEmpty(ukr))
                {
                    sb.AppendLine("One of the translations is missing: " + parsed);
                    continue;
                }

                if (Regex.IsMatch(eng, @"^\d+$") || Regex.IsMatch(ukr, @"^\d+$"))
                {
                    sb.AppendLine("One of the translations is number: " + parsed);
                    continue;
                }

                if (eng.Length < 3 || ukr.Length < 3)
                {
                    sb.AppendLine("One of the translations is less than 3 chars: " + parsed);
                    continue;
                }

                dict.Add(eng, ukr);
            }

            var result = new Tuple<Dictionary<string, string>, string>(dict, sb.ToString());

            return result;
        }

        private static void PrintMenu()
        {
            Console.Clear();
            Console.WriteLine("1. Translate");
            Console.WriteLine("          a. Eng->Ukr");
            Console.WriteLine("          b. Ukr->Eng");
            Console.WriteLine("2. Show Parse Logs");
            Console.WriteLine("          a. In Console");
            Console.WriteLine("          b. In Text File");
            Console.WriteLine("3. Exit (Esc)");
        }

        private static void PrintSubMenu(int part)
        {
            Console.Clear();
            if (part == 1)
            {
                Console.WriteLine("          a. Eng->Ukr");
                Console.WriteLine("          b. Ukr->Eng");
            }
            else
            {
                Console.WriteLine("          a. In Console");
                Console.WriteLine("          b. In Text File");
            }
        }

        private static int GetTopChoice()
        {
            var key = Console.ReadKey();
            if (key.Key == ConsoleKey.Escape)
            {
                return -1;
            }

            var keyChar = key.KeyChar;
            int choice;
            var parseResult = int.TryParse(keyChar.ToString(), out choice);
            if (parseResult)
            {
                if (choice <= 0 || choice > 3)
                {
                    Console.WriteLine("Choice must be in range 1-3");
                    return GetTopChoice();
                }
            }
            else
            {
                Console.WriteLine("First char is not a valid number. Try again.");
                return GetTopChoice();
            }

            return choice;
        }

        private static char GetSubChoice()
        {
            var key = Console.ReadKey();
            if (key.Key == ConsoleKey.Escape)
            {
                return 'e';
            }

            char letter = key.KeyChar;

            if (letter != 'a' && letter != 'b')
            {
                Console.WriteLine("Wrong input. Try a");
                return GetSubChoice();
            }

            return letter;
        }

        private static void PreserveCommandPromptExit()
        {
            Console.ReadKey();
        }

        private static bool GetYesNoChoice()
        {
            return Console.ReadKey().KeyChar.ToString().ToLower()[0] == 'y';
        }
    }
}