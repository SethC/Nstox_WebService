using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using NSTOX.BODataProcessor.Model;

namespace NSTOX.BODataProcessor.Model
{
    [DataContract]
    public class BOFile
    {
        [DataMember]
        public int RetailerId { get; set; }

        [DataMember]
        public string RetailerName { get; set; }

        [DataMember]
        public BOFileType FileType { get; set; }

        [DataMember]
        public byte[] FileContent { get; set; }

        [DataMember]
        public DateTime FileDate { get; set; }
    }
}