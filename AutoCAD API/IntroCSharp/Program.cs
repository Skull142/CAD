//Ondarza Ortega Joaquin
//Diseño Asisttido por Computadora
//Semestre 2016-1
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCADAPI.Intro
{
    class Program
    {
        static void Main(string[] args)
        {
            const string ROOT_DIR = @"C:\Users\skull\Documents\visual studio 2015\Projects\AutoCAD API";
            FileExplorer fExp = new FileExplorer();
            fExp.Scan(new System.IO.DirectoryInfo(ROOT_DIR), true);
            Console.WriteLine(fExp.Print(
                fExp.files.Where(x => (x as System.IO.FileInfo).Extension.ToUpper().Contains("CS")).ToList())
            );
            //*************TAREA*************
            foreach (System.IO.FileSystemInfo f in fExp.dirs)
                Console.WriteLine( fExp.GetSizeFormat( f as System.IO.DirectoryInfo) );
            Console.WriteLine();
        }
    }
}
