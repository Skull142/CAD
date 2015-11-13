using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCADApi.Intro
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

        public void Scan(DirectoryInfo root,
            Boolean recursive = false)
        {
            foreach (DirectoryInfo d in root.GetDirectories())
            {
                this.dirs.Add(d);
                foreach (FileInfo f in d.GetFiles())
                    this.files.Add(f);
                if (recursive)
                    Scan(d);
            }

        }

        public void Clear()
        {
            this.dirs.Clear();
            this.files.Clear();
        }

        public String Print(List<FileSystemInfo> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach(FileSystemInfo item in list)
            {
                if(item is FileInfo)
                {
                    sb.Append(
                        String.Format("Nombre:{0}, Tamaño:{1}\n",
                        item.Name, (item as FileInfo).Length));
                }
                else if (item is DirectoryInfo)
                {
                    sb.Append(
                        String.Format("{0}>>{1}\n",
                        (item as DirectoryInfo).Parent.Name,item.Name
                        ));
                }
            }
            return sb.ToString();
        }

        public String GetSize(DirectoryInfo root)
        {
            throw new NotImplementedException();
            //10 Mb 200KB 30 Bytes

        }

        public override string ToString()
        {
            return String.Format("Files:{0}, Dirs{1}",
                this.files.Count,this.dirs.Count);
        }

    }
}
