using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            MultiKeyDictionary <int, Name, string> usrerTypedict = new MultiKeyDictionary<int, Name, string>();

            usrerTypedict.Add((1, new Name("Roman")), "RValue");
            usrerTypedict.Add((2, new Name("Dima")), "DValue");
            usrerTypedict.Add((3, new Name("EGOR")), "EValue");
            foreach (var x in usrerTypedict)
            {
                Console.WriteLine(x.Key.id + x.Key.name.firstName + " " + x.Value);
            }
            Console.WriteLine();
            try {
                usrerTypedict.Add((3, new Name("EGOR")), "EValue");    //Пытаемся добавить элемент с неуникальным id                                                            
            }
            catch
            {
                Console.WriteLine("Вы добавили элемент с неуникальным id");
            }

        }


    }
}
