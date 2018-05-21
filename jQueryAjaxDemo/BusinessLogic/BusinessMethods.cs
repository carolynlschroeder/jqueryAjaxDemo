using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace jQueryAjaxDemo.BusinessLogic
{
    public class BusinessMethods
    {
        public static string GetFileExtension(string fileName)
        {
            return fileName.Substring(fileName.LastIndexOf('.'));
        }

        public static byte[] FileToBytes(HttpPostedFileBase file)
        {
            byte[] bytes = null;
            using (var ms = new MemoryStream())
            {
                file.InputStream.CopyTo(ms);
                bytes = ms.ToArray();
            }

            return bytes;


        }
    }
}