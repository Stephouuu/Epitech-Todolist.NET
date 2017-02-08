using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.IO;

namespace todolist.src
{
    class StaticTools
    {
        public static string GetLocalStoragePath()
        {
            return (ApplicationData.Current.LocalFolder.Path);
        }

        public static async void CopyFile(string src, string dest)
        {
            await Task.Run(() => File.Copy(src, dest, true));
        }

        public static string SerializeFiles(List<string> files)
        {
            if (files.Count > 0)
            {
                return (string.Join(";", files));
            }
            return "";
        }

        public static List<string> DeserializeFile(string files)
        {
                return (files.Split(';').ToList());
        }

        public static List<string> GetFilePathFromStructure(List<AdditionalFile> list)
        {
            List<string> ret = new List<string>();
            
            for (int i = 0; i < list.Count; i++)
            {
                if (i < list.Count - 1)
                {
                    ret.Add(list.ElementAt(i).path);
                }
            }
            return (ret);
        }
    }
}
