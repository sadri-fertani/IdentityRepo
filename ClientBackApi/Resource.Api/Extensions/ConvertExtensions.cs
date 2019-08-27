using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ClientBackApi.Extensions
{
    public static class ConvertExtensions
    {
        #region Byte[] <--> Object
        /// <summary>
        /// Convert an object to byte array
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="obj">Object</param>
        /// <returns>byte array</returns>
        public static byte[] ToByteArray<T>(this T obj)
        {
            if (obj == null) return null;

            BinaryFormatter bf = new BinaryFormatter();

            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Convert a byte array to an object
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="data">byte array</param>
        /// <returns>Object</returns>
        public static T ToObject<T>(this byte[] data)
        {
            if (data == null) return default(T);

            BinaryFormatter bf = new BinaryFormatter();

            using (MemoryStream ms = new MemoryStream(data))
            {
                object obj = bf.Deserialize(ms);
                return (T)obj;
            }
        }
        #endregion
    }
}
