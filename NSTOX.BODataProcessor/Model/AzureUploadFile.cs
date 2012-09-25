using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NSTOX.BODataProcessor.Model
{
    [DataContract]
    public class AzureUploadFile
    {
        [DataMember]
        public Uri AccountName { get; set; }

        [DataMember]
        public string Container { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Signature { get; set; }
    }
}
