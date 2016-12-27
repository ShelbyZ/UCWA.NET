using Newtonsoft.Json;
using System.IO;

namespace UCWA.NET.Resources
{
    public static class Extensions
    {
        public static T FromBytes<T>(this byte[] bytes)
            where T : Resource
        {
            using (var stream = new MemoryStream(bytes))
            using (var reader = new StreamReader(stream))
            {
                return CreateSerializer().Deserialize(reader, typeof(T)) as T;
            }
        }

        public static byte[] ToBytes<T>(this T obj)
            where T : Resource
        {
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            {
                CreateSerializer().Serialize(writer, obj);
                writer.Flush();
                return stream.ToArray();
            }
        }

        private static JsonSerializer CreateSerializer()
        {
            return JsonSerializer.Create(new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
                NullValueHandling = NullValueHandling.Ignore
            });
        }
    }
}
