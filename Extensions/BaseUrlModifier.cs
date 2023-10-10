namespace StorageApp.Extensions
{
    public static class BaseUrlModifier
    {
        public static string AddFileNameToBaseUrl(this string BaseUrl, string filename )
        {
            return BaseUrl.Replace("{filename}", filename);
        }
        public static string AddBucketName(this string BaseUrl, string filename)
        {
            return BaseUrl.Replace("{filename}", "");
        }
        public static string AddQueryStringToAzure(this string BaseUrl, string listquerystring)
        {
            return BaseUrl.Replace("{filename}", listquerystring);
        }
    }
}
