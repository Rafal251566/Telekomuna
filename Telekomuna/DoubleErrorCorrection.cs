using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCDoubleErrorCorrection
{
    class DoubleErrorCorrection
    {
        private static readonly int[,] matrix = {
        {1, 1, 1, 1, 1, 1, 1, 1,   1, 0, 0, 0, 0, 0, 0, 0},
        {1, 1, 1, 1, 0, 0, 0, 0,   0, 1, 0, 0, 0, 0, 0, 0},
        {1, 1, 0, 0, 1, 1, 0, 0,   0, 0, 1, 0, 0, 0, 0, 0},
        {1, 0, 1, 0, 1, 0, 1, 0,   0, 0, 0, 1, 0, 0, 0, 0},
        {0, 1, 0, 0, 0, 1, 0, 1,   0, 0, 0, 0, 1, 0, 0, 0},
        {0, 0, 1, 0, 0, 1, 1, 0,   0, 0, 0, 0, 0, 1, 0, 0}, 
        {0, 0, 0, 1, 0, 1, 1, 0,   0, 0, 0, 0, 0, 0, 1, 0},
        {0, 0, 0, 0, 1, 0, 1, 1,   0, 0, 0, 0, 0, 0, 0, 1}
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
                int parrityBit = getParrityBit(message, i);

                message.Add(parrityBit);
            }
        }

        public static int getParrityBit(List<int> message, int row)
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
                int checkedParrity = getParrityBit(message, i);
                error.Add(checkedParrity);
                if (checkedParrity == 1) isCorrect = false;
            }
            Console.WriteLine("\nError: " + string.Join("", error));
            if (!isCorrect) correctError(message, error);
        }


        public static void correctError(List<int> message, List<int> error)
        {
            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                for (int j = 0; j < matrix.GetLength(0); j++)
                {
                    if (error[j] != matrix[j, i]) break;
                    if (j == matrix.GetLength(0) - 1)
                    {
                        int temp = message[i];
                        switchBit(ref temp);
                        message[i] = temp;
                        return;
                    }
                }
            }

            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                for (int j = i+1; j < matrix.GetLength(1); j++)
                {
                    for (int k = 0; k < matrix.GetLength(0); k++)
                    {
                        if ((matrix[k, i] ^ matrix[k, j]) != error[k]) break;
                        if (k == matrix.GetLength(0) - 1)
                        {
                            int temp = message[i];
                            switchBit(ref temp);
                            message[i] = temp;

                            temp = message[j];
                            switchBit(ref temp);
                            message[j] = temp;
                            return;
                        }
                    }
                }
            }
        }

        public static void switchBit(ref int bit)
        {
            bit = 1 - bit;
        }

        public static void RunDEC()
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
            Console.WriteLine(wrongMessage.Count);
            checkForError(wrongMessage);

            Console.WriteLine("\nCorrected message:");
            Console.WriteLine(string.Join("", wrongMessage));

            Console.ReadKey();
        }

        public static void RunFileEncoding(byte[] fileContent)
        {
            string path = "encodedMessages.txt";
            try
            {
                File.WriteAllText(path, "");
            }
            catch (Exception)
            {
                Console.WriteLine("File not found!");
                return;
            }
            List<int> message = new List<int>();
            List<string> bits = new List<string>();
            foreach (byte b in fileContent)
            {
                string binaryString = Convert.ToString(b, 2).PadLeft(8, '0');
                bits.Add(binaryString);
                Console.WriteLine(binaryString);
            }
            for (int i = 0; i < bits.Count; i++)
            {
                message.Clear();
                convertToInt(message, bits[i]);
                bitCoding(message);
                string list = string.Join("", message);
                File.AppendAllText(path, list);
                File.AppendAllText(path, "\n");
            }
        }

        public static void RunFileDecodingDEC(string[] fileContent)
        {
            string path = "decodedMessages.txt";
            try
            {
                File.WriteAllText(path, "");
            }
            catch (Exception)
            {
                Console.WriteLine("File not found!");
                return;
            }
            List<int> message = new List<int>();
            byte[] decodedBytes = new byte[fileContent.Length];
            for (int i = 0; i < fileContent.Length; i++)
            {
                message.Clear();
                convertToInt(message, fileContent[i]);
                checkForError(message);
                message = message.Take(8).ToList();
                string list = string.Join("", message);
                Console.WriteLine(list);
                byte decodedByte = Convert.ToByte(list, 2);
                decodedBytes[i] = decodedByte;

                File.WriteAllBytes(path, decodedBytes);
            }
        }

    }
}
