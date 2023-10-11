namespace StorageApp.Extensions
{
    public static class MimeMapping
    {
        public static string GetContentTypeFromExtension(string filename)
        {
            Dictionary<string, string> extensionToContentType = new Dictionary<string, string>
                {
                    { ".txt", "text/plain" },
                    { ".html", "text/html" },
                    { ".jpg", "image/jpeg" },
                    { ".png", "image/png" },
                    { ".gif", "image/gif" },
                    { ".pdf", "application/pdf" },
                    { ".doc", "application/msword" },
                    { ".xls", "application/vnd.ms-excel" },
                    { ".ppt", "application/vnd.ms-powerpoint" },
                    { ".xml", "application/xml" },
                    { ".json", "application/json" },
                    { ".zip", "application/zip" },
                    { ".mp3", "audio/mpeg" },
                    { ".mp4", "video/mp4" },
                };

            string fileExtension = Path.GetExtension(filename);

            if (extensionToContentType.TryGetValue(fileExtension, out string contentType))
            {
                return contentType;
            }

            return "application/octet-stream";
        }
        public static string FormatFileSize(ulong? fileSizeInBytes)
        {
            const long KB = 1024;
            const long MB = 1024 * KB;
            const long GB = 1024 * MB;

            if (fileSizeInBytes >= GB)
            {
                return string.Format("{0:0.00} GB", (double)fileSizeInBytes / GB);
            }
            if (fileSizeInBytes >= MB)
            {
                return string.Format("{0:0.00} MB", (double)fileSizeInBytes / MB);
            }
            if (fileSizeInBytes >= KB)
            {
                return string.Format("{0:0.00} KB", (double)fileSizeInBytes / KB);
            }

            return string.Format("{0} bytes", fileSizeInBytes);
        }

    }
}
