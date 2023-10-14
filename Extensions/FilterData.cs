using StorageApp.Models;

namespace StorageApp.Extensions
{
    public static class FilterData
    {
        public static List<T> FilterByDynamicProperty<T>(this List<T> sourceList, string PropertyName, string PropertyValue)
        {
            if (PropertyName == null || string.IsNullOrWhiteSpace(PropertyName))
            {
                return sourceList;
            }
            var property = typeof(T).GetProperty(PropertyName);
            if (property == null)
            {
                return sourceList;
            }
            return sourceList.Where(item =>
            {
                var propertyValue = property.GetValue(item, null) as string;
                return propertyValue != null && propertyValue.ToLower().Contains(PropertyValue);
            }).ToList();
        }
        public static List<T> OderByDynamicProperty<T>(this List<T> sourceList, string PropertyName,string asc_Desc)
        {
            if (PropertyName == null || string.IsNullOrWhiteSpace(PropertyName))
            {
                return sourceList;
            }
            var property = typeof(T).GetProperty(PropertyName);
            if (property == null)
            {
                return sourceList;
            }
            if (asc_Desc == "asc")
            {
                return sourceList.OrderBy(item => property.GetValue(item, null)).ToList();
            }
            else
            {
                return sourceList.OrderByDescending(item => property.GetValue(item, null)).ToList();
            }
            
        }
        public static List<FileInformation> FormatFileSize(this List<FileInformation> sourceList)
        {
            sourceList.ForEach(e =>
            {
                e.fileSize = MimeMapping.FormatFileSize(e.size);
            });
            return sourceList;

        }
        public static List<FileInformation> RemoveSubDirectoryFiles(this List<FileInformation> sourceList)
        {
            sourceList = sourceList.Where(e => !e.fileName.Contains("/") && !e.fileName.EndsWith("/")).ToList();
            return sourceList;

        }
    }
}
