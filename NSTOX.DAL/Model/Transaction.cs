using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace NSTOX.DAL.Model
{
    public class Transaction : ModelBase
    {
        [UpdateableField]
        public int Number { get; set; }
        [UpdateableField]
        public DateTime Date { get; set; }
        [UpdateableField]
        public int TerminalNo { get; set; }
        [UpdateableField]
        public int EmployeeNo { get; set; }
        [UpdateableField]
        public int Line { get; set; }
        [UpdateableField]
        public long? SKU { get; set; }
        [UpdateableField]
        public int? DepartmentId { get; set; }
        [UpdateableField]
        public double QuantitySold { get; set; }
        [UpdateableField]
        public double UnitSell { get; set; }
        [UpdateableField]
        public double UnitCost { get; set; }
        [UpdateableField]
        public double ExtendedPrice { get; set; }
        [UpdateableField]
        public double ExtendedCost { get; set; }
        [UpdateableField]
        public double SellMargin { get; set; }
        [UpdateableField]
        public double TaxAmount { get; set; }

        public string ToXML()
        {
            StringBuilder sb = new StringBuilder();
            using (TextWriter tw = new StringWriter(sb))
            {
                XmlSerializer deSerializer = new XmlSerializer(typeof(Transaction));
                deSerializer.Serialize(tw, this);
            }
            return sb.ToString();
        }
    }
}
