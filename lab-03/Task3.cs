using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace lab_03
{
    public class Task3
    {
        public static void Run()
        {
            var lines = new List<string>(File.ReadAllLines("./people.txt"));

            lines.Sort((a, b) =>
            {
                if (a.Split(' ').Length <= 4)
                {
                    return 0;
                }

                var aAgeString = a.Split(' ').Reverse().ToArray()[1];
                int aAgeInt;
                var bAgeString = b.Split(' ').Reverse().ToArray()[1];
                int bAgeInt;

                if (int.TryParse(aAgeString, out aAgeInt) && int.TryParse(bAgeString, out bAgeInt))
                {
                    return aAgeInt > bAgeInt ? 1 : -1;
                }

                return 0;
            });

            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }
        }
    }
}
