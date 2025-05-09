using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatoveTypyStruktury
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int i = 0; //Deklarace proměnné i datového typu int

            int x = 0; //Hodnotový datový typ
            List<int> ints = new List<int>() {0, 1, 2, 3}; //Referenční datový typ - pouze odkazy na místo v paměti

            String s = "test"; //Jednoduchý datový typ
            List<String> strings = new List<String>() {"","",""}; //Výčtový datový typ - množina hodnot

            int camelCaseDeclaration = 0;


            Console.WriteLine("Byte: \t" + Byte.MinValue + " to " + Byte.MaxValue);
            Console.WriteLine("Short: \t" + short.MinValue + " to " + short.MaxValue);
            Console.WriteLine("Int: \t" + int.MinValue + " to " + int.MaxValue);
            Console.WriteLine("Long: \t" + long.MinValue + " to " + long.MaxValue);

            Console.WriteLine();

            int intValue = 1000;
            String stringValue = "value: " + intValue; // Implicitní konverze
            //Implicitní konverzi lze použít při konvertování na typ s větším rozsahem
            //->
            long longValue = intValue;
            //byte byteValue = intValue; //Nelze implicitně - způsobí error
            byte byteValueExplicit = (byte)intValue; // Musíme Explicitně

            double doubleValue2 = Convert.ToDouble(intValue); //Explicitní konverze pomocí třídy Convert
            double doubleValue = (double) intValue; //Explicitní konverze pomocí castu

            int parsedInt = Int32.Parse("34"); // Explicitni konverze pomocí parsing, může hodit error, try-catch.

            /*
             * Struktury programovacího jazyka
             * sekvence - příkazy jsou vykonávány v pořadí zápisu
             * selekce - vykonání příkazů na základě vyhodnocení platnosti podmínky (if statement)
             * cyklus - opakování příkazů dokud se nesplní nějaká podmínka.
            */

            Console.ReadKey();
        }
    }
}
