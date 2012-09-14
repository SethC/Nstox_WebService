using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NSTOX.BODataProcessor.Model
{
    [Serializable]
    public class SalesTransactionGroup
    {
        public SalesTransactionDetailItem Item { get; set; }
    }
}
