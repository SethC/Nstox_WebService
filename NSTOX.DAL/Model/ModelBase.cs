using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSTOX.DAL.Model
{
    public class ModelBase
    {
        [UpdateableField]
        public int RetailerId { get; set; }

        public ElementStatus? Status { get; set; }
    }
}
