using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace NSTOX.DAL.Model
{
    public class Item : ModelBase
    {
        [UpdateableField]
        public long SKU { get; set; }
        [UpdateableField]
        public string Description { get; set; }
        [UpdateableField]
        public int? DepartmentId { get; set; }
        [UpdateableField]
        public int PriceGroup { get; set; }
        [UpdateableField]
        public int ProductGroup { get; set; }
        [UpdateableField]
        public bool Category7 { get; set; }
        [UpdateableField]
        public bool Category8 { get; set; }
        [UpdateableField]
        public bool Category9 { get; set; }
        [UpdateableField]
        public bool Category10 { get; set; }
        [UpdateableField]
        public string Size { get; set; }
        [UpdateableField]
        public string Unit { get; set; }
        [UpdateableField]
        public double CurrentUnitPrice { get; set; }
        [UpdateableField]
        public double QtyPrice { get; set; }
        [UpdateableField]
        public int QtyBreak { get; set; }
        public DateTime? DateAdd { get; set; }
        public DateTime? DateUpdated { get; set; }
        public DateTime? DateRemoved { get; set; }
    }
}
