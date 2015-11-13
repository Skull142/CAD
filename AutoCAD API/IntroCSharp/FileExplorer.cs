//Ondarza Ortega Joaquin
//Diseño Asisttido por Computadora
//Semestre 2016-1
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AutoCADAPI.Intro
{
    public class FileExplorer
    {
        public List<FileSystemInfo> dirs;
        public List<FileSystemInfo> files;

        public FileExplorer()
        {
            dirs = new List<FileSystemInfo>();
            files = new List<FileSystemInfo>();
        }
        public void Scan(DirectoryInfo root, Boolean recursive = false)
        {
            foreach (DirectoryInfo d in root.GetDirectories())
            {
                this.dirs.Add ( d );
               
                if (recursive)
                    Scan( d , recursive) ;
            }
            foreach (FileInfo f in root.GetFiles())
                this.files.Add(f);

        }


        public void Clear()
        {
            this.dirs.Clear();
            this.files.Clear();
        }
        public String Print(List<FileSystemInfo> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (FileSystemInfo  item in list)
            {
                if (item is FileInfo)
                {
                    sb.Append( String.Format("Nombre:{0}\tTamaño:{1}\n", item.Name, (item as FileInfo).Length));
                }
                else if(item is DirectoryInfo)
                {
                    String.Format(
                        "{0}>>>{1}",
                        (item as DirectoryInfo).Parent.Name,
                        item.Name);
                }
            }
            return sb.ToString();

        }

        //*************TAREA*************
        public long GetSize(DirectoryInfo root, Boolean recursive = true)
        {
            long count = 0;
            foreach(FileInfo fi in root.GetFiles())
                count += fi.Length;
            if (recursive)  
            {
                foreach (DirectoryInfo dir in root.GetDirectories())
                    count += GetSize( dir , recursive );  
            }
            return count;
        }
        public String GetSizeFormat(DirectoryInfo root, Boolean recursive = true)
        {
            long size = GetSize(root, recursive);
            String str = "";
            int ln = (int)Math.Log10(size);
            switch (ln) 
            {
                default:
                    str = "Bytes";
                    ln = 1;
                    break;
                case 3:
                case 4:
                case 5:
                    str = "KB";
                    ln = 1000;
                    break;
                case 6:
                case 7:
                case 8:
                    str = "MB";
                    ln = 1000000;
                    break;

            }
            return String.Format("Dir: {0}\tSize: {1} {2}",root.Name,size/ln, str);
        }

        public override string ToString()
        {
            return String.Format("Files:{0}\tDirs{1}\n",this.files.Count,this.dirs.Count);
        }

    }
}
