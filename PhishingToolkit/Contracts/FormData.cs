using System.Runtime.Serialization;

namespace Contracts
{
    [DataContract]
    public class FormData
    {
        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public long Timestamp { get; set; }

        public FormData()
        {
        }
    }
}
