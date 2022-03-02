using System;

namespace NumberToWords
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!----> " + HundresandUp("51"));
        }

        static string ones(string num)
        {
            int no = Convert.ToInt32(num);
            string no_out = string.Empty;
            switch(no)
            {
                case 1: no_out = "One"; break;
                case 2: no_out = "Two"; break;
                case 3: no_out = "Three"; break;
                case 4: no_out = "Four"; break;
                case 5: no_out = "Five"; break;
                case 6: no_out = "Six"; break;
                case 7: no_out = "Seven"; break;
                case 8: no_out = "Eight"; break;
                case 9: no_out = "Nine"; break;
            }
            return no_out;
        }
        static string tens(string num)
        {
            int no = Convert.ToInt32(num);
            string no_out = string.Empty;
            switch (no)
            {
                case 10: no_out = "Ten"; break;
                case 11: no_out = "Elevan"; break;
                case 12: no_out = "Twelve"; break;
                case 13: no_out = "Thirteen"; break;
                case 14: no_out = "Fourteen"; break;
                case 15: no_out = "Fifteen"; break;
                case 16: no_out = "Sixteen"; break;
                case 17: no_out = "Seventeen"; break;
                case 18: no_out = "Eighteen"; break;
                case 19: no_out = "Ninteen"; break;
                case 20: no_out = "Twenty"; break;
                case 30: no_out = "Thirty"; break;
                case 40: no_out = "Fourty"; break;
                case 50: no_out = "fifty"; break;
                case 60: no_out = "Sixty"; break;
                case 70: no_out = "Seventy"; break;
                case 80: no_out = "Eighty"; break;
                case 90: no_out = "Ninty"; break;
                default:
                    if(no>0)
                    {
                        no_out = tens(num.Substring(0, 1)+"0")+ " " + ones(num.Substring(1));
                    }
                    break;
            }
            return no_out;
        }
    
        static string HundresandUp(string num)
        {
            int length = num.Length;
            string no_out = string.Empty;
            switch(length)
            {
                case 1:
                    no_out = ones(num);
                    break;
                case 2:
                    no_out = tens(num);
                    break;
            }
            return no_out;
        }
    
    
    }
}
