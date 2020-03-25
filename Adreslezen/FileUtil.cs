using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Adreslezen
{
    class FileUtil
    {
        public static void UnZip(String file, String folder)
        {
            if (Directory.Exists(folder))
            {
                Directory.Delete(folder);
            }
            ZipFile.ExtractToDirectory(file, folder);
        }
    }
}
