using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HCDoubleErrorCorrection;
using HCSingleErrorCorrection;

namespace HammingCode
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("1.SingleErrorCorrection");
            Console.WriteLine("2.DoubleErrorCorrection");
            Console.WriteLine("3.File coding with DEC");
            Console.WriteLine("4.File decoding with DEC");
            Console.Write("Select error correction (1 or 4): ");

            string choice = Console.ReadLine();

            Console.Write("\n");

            switch (choice)
            {
                case "1":
                    SingleErrorCorrection.RunSEC();
                    break;
                case "2":
                    DoubleErrorCorrection.RunDEC();
                    break;
                case "3":
                    string filePath = "messages.txt";
                    try
                    {
                        byte[] lines = File.ReadAllBytes(filePath);
                        DoubleErrorCorrection.RunFileEncoding(lines);
                    }
                    catch (Exception) {
                        Console.WriteLine("File not found!");
                    }
                    break;
                case "4":
                    filePath = "encodedMessages.txt";
                    try
                    {
                        string[] lines = File.ReadAllLines(filePath);
                        DoubleErrorCorrection.RunFileDecodingDEC(lines);
                    }
                    catch (IOException)
                    {
                        Console.WriteLine("File not found!");
                    }
                    break;
                default:
                    Console.WriteLine("Incorrect choice!");
                    break;
            }
        }
    }
}
