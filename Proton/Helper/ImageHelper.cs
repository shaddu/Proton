using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace Proton.Helper
{
    public class ImageHelper
    {
        internal static string SaveImage(HttpFileCollectionBase imageFiles)
        {
            if (imageFiles != null && imageFiles.Count > 0)
            {
                HttpPostedFileBase imageFile = imageFiles[0];
                if (imageFile.ContentLength > 0)
                {
                    WebImage img = new WebImage(imageFile.InputStream);
                    img.Resize(50, 50);
                    string ImageName = System.IO.Path.GetFileName(imageFile.FileName);
                    string physicalPath = HttpContext.Current.Server.MapPath("~/Images/" + ImageName);
                    img.Save(physicalPath);
                    return ImageName;
                }
            }
            return null;
        }
    }
}