using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsUploadWebApi
{
    /// <summary>
    /// 
    /// </summary>
    public class FileUploadResult
    {
        public string LocalFilePath { get; set; }
        public string Name { get; set; }
        public long FileLength { get; set; }
    }
}
