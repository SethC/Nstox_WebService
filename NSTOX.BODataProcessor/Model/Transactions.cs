using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NSTOX.BODataProcessor.Model
{
    [Serializable]
    [XmlRoot(ElementName="biztalk_1")]
    public class Transactions
    {
        [XmlArray(ElementName = "body")]
        [XmlArrayItem(ElementName = "ActiveStore_SalesTransaction_1.70")]
        public List<SalesTransaction> SaleTransactions { get; set; }
    }
}
