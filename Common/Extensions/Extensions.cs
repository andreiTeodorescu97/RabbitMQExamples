using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Extensions
{
    public static class Extensions
    {
        public static byte[] Serialize(this Object obj)
        {
            if (obj == null)
            {
                return null;
            }

            var json = JsonConvert.SerializeObject(obj);

            return Encoding.ASCII.GetBytes(json);
        }

        public static Object DeSerialize(this byte[] arrBytes, Type type)
        {
            var json = Encoding.Default.GetString(arrBytes);

            return JsonConvert.DeserializeObject(json, type);
        }

        public static string DeSerializeText(this byte[] arrBytes)
        {
            return Encoding.Default.GetString(arrBytes);
        }
    }
}
