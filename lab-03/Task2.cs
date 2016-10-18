using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace lab_03
{
    public class Task2
    {
        public static void Run()
        {
            var input = File.ReadAllText("./braces.txt");

            var stack = new Stack<int>();
            int closingBracesCount = 0;
            for (int index = 0; index < input.ToCharArray().Length; index++)
            {
                var ch = input.ToCharArray()[index];
                if (ch.Equals('('))
                {
                    stack.Push(index);
                }
                else if (ch.Equals(')'))
                {
                    stack.Pop();
                    ++closingBracesCount;
                }
            }

            Console.WriteLine("К-ть дужок однакова - " + (stack.Count == 0));
            Console.WriteLine("К-ть закриваючих дужок " + closingBracesCount);
        }
    }
}
