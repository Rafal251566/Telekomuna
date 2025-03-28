using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks.Dataflow;

namespace HCSingleErrorCorrection
{
    public class SingleErrorCorrection
    {
        private static readonly int[,] matrix = {
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0},
        {1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 0, 0},
        {1, 1, 0, 0, 1, 1, 0, 0, 0, 0, 1, 0},
        {1, 0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 1}
    };

        public static void convertToInt(List<int> message, string text)
        {
            foreach (char c in text)
            {
                message.Add((int)char.GetNumericValue(c));
            }
        }

        public static void bitCoding(List<int> message)
        {
            if (message.Count == 0) return;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                int parrityBit = getParityBit(message, i);

                message.Add(parrityBit);
            }
        }

        public static int getParityBit(List<int> message, int row)
        {
            int sum = 0;
            for (int i = 0; i < message.Count; i++)
            {
                sum += matrix[row, i] * message[i];
            }
            return sum % 2;
        }

        public static void checkForError(List<int> message)
        {
            if (message.Count != matrix.GetLength(1))
            {
                Console.WriteLine("Bit count is not correct!");
                return;
            }

            bool isCorrect = true;
            List<int> error = new List<int>();

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                int checkedParrity = getParityBit(message, i);
                error.Add(checkedParrity);
                if (checkedParrity == 1) isCorrect = false;
            }
            Console.WriteLine("\nError: " + string.Join("", error));
            if (!isCorrect) correctError(message, error);
        }


        public static void correctError(List<int> message, List<int> error)
        {
            bool errorFound = false;

            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                errorFound = true;
                for (int j = 0; j < matrix.GetLength(0); j++)
                {
                    if (error[j] != matrix[j, i])
                    {
                        errorFound = false;
                        break;
                    }
                }
                if (errorFound)
                {
                    int temp = message[i];
                    switchBit(ref temp);
                    message[i] = temp;
                    break;
                }
            }
        }


        public static void switchBit(ref int bit)
        {
            bit = 1 - bit;
        }


        public static void RunSEC()
        {
            List<int> message = new List<int>();
            Console.WriteLine("Type a message containing 8 bits: ");

            string input = Console.ReadLine();
            if (input.Length != 8)
            {
                Console.WriteLine("Bit count is not correct!");
                return;
            }

            convertToInt(message, input);
            bitCoding(message);

            Console.WriteLine("\nEncoded message:");
            Console.WriteLine(string.Join("", message));
            Console.WriteLine();


            List<int> wrongMessage = new List<int>();

            Console.WriteLine("\nType wrong message:");
            input = Console.ReadLine();

            convertToInt(wrongMessage, input);
            checkForError(wrongMessage);

            Console.WriteLine("\nCorrected message:");
            Console.WriteLine(string.Join("", wrongMessage));

            Console.ReadKey();
        }
    }
}