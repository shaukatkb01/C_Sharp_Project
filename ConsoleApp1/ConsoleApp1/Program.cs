// See https://aka.ms/new-console-template for more information

// Datatype in C#

// bool Datatype

using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

//int Age = 30;
//int Best = 18;

//Console.WriteLine(Age > Best); // output "True" 
//Console.WriteLine(Age < Best); // output "False"
//Console.ReadLine();

// byte Datatype

//byte a = 255; // max number store 255, size 1 byte
//byte b = 256; show erro 

//Console.WriteLine(a);
//Console.ReadLine();

// sbyte Datatype

//sbyte a = (127); // max number store +127
//sbyte b = (-128); // max number store _128
//sbyte c = (128); // show error 
//sbyte d = (-129); // show error

//Console.WriteLine(a + b); // output -1
//Console.ReadLine();

// short Datatype

//short a = 1; // max number store  32767 and -32768 size 2 byte
//short b = 32767;
////short c = a + b; // show error
//int c = b - a; //  
//Console.WriteLine(c);

//Console.WriteLine(int.MaxValue); // this function show datatype max value
//Console.WriteLine(int.MinValue); // this funciton shwo datatype min value

//Console.ReadLine();

// constant
// constant
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;


namespace ConstansCsahrp
{

    class Program
    {
        //public const short ADCod = 230;

        static void Main(string[] args)
        {

            // declare int value
            int a, b, c, d, f, g, h, i, j, k;
            a = 4;
            a = +2;
            b = +3;
            // tarnery operator

            //string l = (a > b) ? $"{b} is greter:{a}" : $"{a} is lessthen {b}";


            // precedance in c#
            //a = 2 + 2 / 1 * 5 - 1;

            // if statement

            //if (a == 2) 
            //{
            //    Console.WriteLine("number = 4");
            //    ++a;
            //}
            //else
            //{
            //    Console.WriteLine("number not 4");
            //}
            string z = @"this is your ""zone"", 
            i \maean                karachi\";
                        
            Console.WriteLine(z);
            Console.ReadLine();
        }
    }
}


