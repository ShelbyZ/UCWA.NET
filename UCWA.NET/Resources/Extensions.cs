using System.IO;
using System.Runtime.Serialization.Json;

namespace UCWA.NET.Resources
{
    public static class Extensions
    {
        public static T FromBytes<T>(this byte[] bytes)
            where T : Resource
        {
            var serializer = CreateSerializer<T>();
            using (var stream = new MemoryStream(bytes))
            {
                return serializer.ReadObject(stream) as T;
            }
        }

        public static byte[] ToBytes<T>(this T obj)
            where T : Resource
        {
            var serializer = CreateSerializer<T>();
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, obj);
                return stream.ToArray();
            }
        }

        private static DataContractJsonSerializer CreateSerializer<T>()
            where T : Resource
        {
            return new DataContractJsonSerializer(typeof(T), new DataContractJsonSerializerSettings
            {
                UseSimpleDictionaryFormat = true,
                DataContractSurrogate = new ResourceSurrogate()
            });
        }
    }
}
