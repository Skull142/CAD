using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCADApi.Intro
{
    class Program
    {
        static void Main(string[] args)
        {
            Boolean[] doors = new Boolean[100];
            //LLenar las puertas
            for (int i = 0; i < doors.Length; i++)
            {
                doors[i] = false;
            }
            //Visitar las puertas empezando en 1, y despues 2...3..4..
            int n = 0;
            while (n < doors.Length)
            {
                for (int i = n; i < doors.Length; i++)
                    doors[n] = !doors[n];
                n++;
            }
            int count = doors.Where(x => x == true).Count();
            n = 0;
            do
            {
                String str = String.Format("{0}", doors[n] == true ? "[ ]" : "[x]");
                Console.WriteLine(str);
                n++;
            } while (n < doors.Length);

            Console.Clear();
            //Mostrar archivos
            const string ROOT_DIR= @"C:\Users\Miguel\Documents\Visual Studio 2015\Projects";

            FileExplorer fExp = new FileExplorer();
            fExp.Scan(new System.IO.DirectoryInfo(ROOT_DIR), true);
            Console.WriteLine(fExp.Print(
                fExp.files.Where(
                    x => (x as System.IO.FileInfo).Extension.ToUpper().Contains(
                        "CS")).ToList()));
            Console.ReadLine();
   

        }
    }
}
