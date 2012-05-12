using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FIITimetableParser;
using Objects;
namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser parser = new Parser();
            DateTime aa = DateTime.Now;
            List<TimetableItem> list = parser.GetTimetable(StudyYear.I2, "B7");
            DateTime bb = DateTime.Now;
            Console.WriteLine((bb-aa).TotalMilliseconds);

          
            if (list != null)
            {
                foreach (var item in list)
                {
                    Console.WriteLine(item);
                }
            }
            Console.Read();
        }

        
    }
}
