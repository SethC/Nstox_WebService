using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NSTOX.BODataProcessor.Model
{
    [Serializable]
    public class SalesTransactionDetail
    {
        public int SequenceNumber { get; set; }

        [XmlElement(ElementName = "TransactionDetailGroup")]
        public SalesTransactionGroup Details { get; set; }
    }
}
