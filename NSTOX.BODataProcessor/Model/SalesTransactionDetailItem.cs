using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NSTOX.BODataProcessor.Model
{
    [Serializable]
    public class SalesTransactionDetailItem
    {
        public long ItemID { get; set; }

        public int MerchandiseHierarchy { get; set; }

        public double Quantity { get; set; }

        public double UnitSalesPrice { get; set; }

        public double UnitCostPrice { get; set; }

        public double ExtendedAmount { get; set; }

        public double TaxAmount { get; set; }

        [XmlIgnore]
        public double ExtendedCost
        {
            get
            {
                return Quantity * UnitCostPrice;
            }
        }

        [XmlIgnore]
        public double SellMargin
        {
            get
            {
                return UnitSalesPrice == 0 ? 0 : (UnitSalesPrice - UnitCostPrice) / UnitSalesPrice;
            }
        }
    }
}
