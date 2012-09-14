using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace NSTOX.BODataProcessor.Model
{
    [Serializable]
    public class SalesTransaction
    {
        public int TransactionNumber { get; set; }

        [XmlIgnore()]
        public DateTime StartDateTime { get; set; }

        /// <summary>
        /// work arround to fix the timezone issue
        /// </summary>
        [XmlElement(ElementName = "StartDateTime")]
        public string XmlStartDateTime
        {
            get { return XmlConvert.ToString(StartDateTime, XmlDateTimeSerializationMode.RoundtripKind); }
            set { StartDateTime = DateTimeOffset.Parse(value).DateTime; }
        }

        /// <summary>
        /// Terminal number
        /// </summary>
        public int TillID { get; set; }

        /// <summary>
        /// Employee Number
        /// </summary>
        public int CashierID { get; set; }

        [XmlElement(ElementName = "TransactionDetail")]
        public SalesTransactionDetail[] TransactionDetails { get; set; }


    }
}
