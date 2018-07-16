using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XtenderSolutions.AXRESTClient
{
    public class AXRESTClientFile : IDisposable
    {
        public enum AXClientFileTypes
        {
            Bin,
            Annotation,
            Text,
            Rendition
        }

        public Stream Stream { get; set; }
        public AXClientFileTypes Type { get; set; }
        public string FileName { get; set; }
        public string TypeName
        {
            get
            {
                switch(Type)
                {
                    case AXClientFileTypes.Annotation:
                        return "annotation";
                    case AXClientFileTypes.Text:
                        return "text";
                    case AXClientFileTypes.Bin:
                    default:
                        return "bin";
                }
            }
        }
        public static AXRESTClientFile LoadFromFile(string fullpath, AXClientFileTypes t)
        {
            if(string.IsNullOrEmpty(fullpath) || !System.IO.File.Exists(fullpath))
                throw new FileNotFoundException("The file cannot be found {0}", fullpath);

            AXRESTClientFile ret = new AXRESTClientFile();
            ret.Type = t;
            ret.FileName = System.IO.Path.GetFileName(fullpath);

            ret.Stream = File.Open(fullpath, FileMode.Open);

            return ret;
        }

        public static AXRESTClientFile LoadFromStream(Stream s, string filename, AXClientFileTypes t)
        {
            AXRESTClientFile ret = new AXRESTClientFile();
            ret.Type = t;
            ret.FileName = filename;
            s.Seek(0, SeekOrigin.Begin);
            ret.Stream = s;

            return ret;
        }

        public static AXRESTClientFile LoadFromMemoryBytes(byte[] bArray, string filename, AXClientFileTypes t)
        {
            AXRESTClientFile ret = new AXRESTClientFile();
            ret.Type = t;
            ret.FileName = filename;

            MemoryStream ms = new MemoryStream(bArray);
            ms.Position = 0;

            ret.Stream = ms;

            return ret;
        }

        public void SaveToLocal(string fullpath)
        {
            string fname = string.Empty;
            if(string.IsNullOrEmpty(fullpath))
            {
                //If the fullpath is provided, use it to save the file
                fname = fullpath;
            }
            else
            {
                //else save the file under current directory using original file name
                fname = FileName;
            }

            var fileStream = File.Create(fname);
            Stream.Seek(0, SeekOrigin.Begin);
            Stream.CopyTo(fileStream);
            fileStream.Close();
        }

        public void Dispose()
        {
            Dispose(true);
        }
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if(Stream != null)
                    Stream.Dispose();
            }
        }
        ~AXRESTClientFile()
        {
            Dispose(false);
        }
    }
}
