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
            List<TimetableItem> list = parser.GetTimetableForGroup(StudyYear.MIS1, HalfYear.None, "1");
            List<TimetableItem> list2 = parser.GetTimetableForYear(StudyYear.I1, HalfYear.B);
            List<TimetableItem> list3 = parser.GetTimetableForYear(StudyYear.MIS1);
            List<TimetableItem> list4 = parser.GetTimetableForYear(StudyYear.I2, HalfYear.B);
            List<TimetableItem> list5 = parser.GetTimetableForGroup(StudyYear.I3, HalfYear.B, "3");
            DateTime bb = DateTime.Now;
            Console.WriteLine((bb-aa).TotalMilliseconds);

            list.AddRange(list2);
            list.AddRange(list3);
            list.AddRange(list4);
            list.AddRange(list5);

            if (list != null)
            {
                foreach (var item in list)
                {
                    Console.WriteLine(item);
                }
            }
            Console.WriteLine(Exporter.ConvertToXML(list));
            Console.Read();
        }        
    }
}
