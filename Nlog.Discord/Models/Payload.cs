using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace NLog.Discord.Models
{
    [DataContract]
    public class Payload
    {
        [DataMember(Name = "content")]
        public string Content { get; set; }

        [DataMember(Name = "username")]
        public string Username { get; set; }



        public Payload()
        {
        }

        public string ToJson() 
        { 
            var serializer = new DataContractJsonSerializer(typeof(Payload));
            using (var memoryStream = new MemoryStream())
            {
                serializer.WriteObject(memoryStream, this);
                memoryStream.Position = 0;
                using (var reader = new StreamReader(memoryStream))
                {
                    string json = reader.ReadToEnd();
                    return json;
                }
            }
        }
    }
}