using System;
using SkiaSharp;

namespace PlayGround
{
    class Program
    {
        //todo points stay within bitmap frame
        //static SKColor BgColor = SKColor.Parse("#0077be");
        //static SKColor BgColor = SKColors.Wheat;
        //static SKColor BgColor = SKColors.Black;
        //static SKColor BgColor = SKColor.Parse("#30A9B6");
        static SKColor BgColor = SKColors.Transparent;

        static Random Rand = new Random();

        static void Main(string[] args)
        {
            var width = 2400;
            var height = 2400;
            var step = 35;
            var numberOfPoints = 30000;

            if (args.Length > 3)
            {
                int.TryParse(args[0], out width);
                int.TryParse(args[1], out height);
                int.TryParse(args[2], out step);
                int.TryParse(args[3], out numberOfPoints);
            }

            Console.WriteLine("Started!");            
            try
            {
                new Chaos(
                    width, height,
                    numberOfPoints, step, BgColor,
                    "images").Draw(CreateString(10));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
            Console.WriteLine("Finished!");
        }

        internal static string CreateString(int stringLength)
        {
            const string allowedChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789!@$?_-";
            char[] chars = new char[stringLength];

            for (int i = 0; i < stringLength; i++)
            {
                chars[i] = allowedChars[Rand.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }


    }
}
