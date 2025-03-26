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
            Console.Write("Select error correction code (1 or 2): ");

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
                default:
                    Console.WriteLine("Incorrect choice!");
                    break;
            }
        }
    }
}
