using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        public static Resource GetResource(this Dictionary<string, Dictionary<string, object>> data)
        {
            if (data != null)
            {
                foreach (var item in data)
                {
                    var json = JsonConvert.SerializeObject(item.Value);
                    var type = string.Format("UCWA.NET.Resources.{0}{1}, UCWA.NET.Resources", char.ToUpper(item.Key[0]), item.Key.Substring(1));
                    var t = Type.GetType(type);

                    return JsonConvert.DeserializeObject(json, Type.GetType(type)) as Resource;
                }
            }

            return default(Resource);
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
