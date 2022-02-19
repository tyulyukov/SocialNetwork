using ImageProcessor;
using ImageProcessor.Plugins.WebP.Imaging.Formats;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;

namespace SocialNetwork.Helpers
{
    public static class Media
    {
        public static readonly List<String> ImageExtensions = new List<String> { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG" };
        public static String WebRootPath;
        public static String StoragePath => WebRootPath + "\\storage\\";

        public static bool IsImage(string fileExtension)
        {
            String value = fileExtension.ToUpper();
            return ImageExtensions.Contains(value);
        }

        public static string UploadImage(IFormFile file, String directoryName)
        {
            if (file == null)
                return String.Empty;

            String fileExtension = Path.GetExtension(file.FileName);

            if (!IsImage(fileExtension))
                return String.Empty;

            String fileName = Guid.NewGuid().ToString() + ".webp";
            String basePath = CreateDirectory(DateTime.Today, directoryName) + "/" + fileName;

            using (var webPFileStream = file.OpenReadStream())
                using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                    imageFactory.Load(webPFileStream)
                                .Format(new WebPFormat())
                                .Quality(70)
                                .Save(StoragePath + basePath);

            return "/storage/" + basePath;
        }

        public static String CreateDirectory(DateTime date, String directoryName)
        {
            if (!Directory.Exists(StoragePath + directoryName + "\\" + date.Year))
                Directory.CreateDirectory(StoragePath + directoryName + "\\" + date.Year);

            if (!Directory.Exists(StoragePath + directoryName + "\\" + date.Year + "\\" + date.Month))
                Directory.CreateDirectory(StoragePath + directoryName + "\\" + date.Year + "\\" + date.Month);

            return directoryName + "/" + date.Year + "/" + date.Month;
        }
    }
}
