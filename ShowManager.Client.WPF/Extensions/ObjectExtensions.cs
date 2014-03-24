using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ShowManager.Client.WPF.Extensions
{
    public static class ObjectExtensions
    {
        public static T Clone<T>(this T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("They type must be serializable.", "source");
            }

            T clone = default(T);

            if (!Object.ReferenceEquals(source, null))
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new MemoryStream();
                using (stream)
                {
                    formatter.Serialize(stream, source);
                    stream.Seek(0, SeekOrigin.Begin);
                    clone = (T)formatter.Deserialize(stream);
                }
            }

            return clone;
        }
    }
}
