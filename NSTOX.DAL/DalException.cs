using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSTOX.DAL
{
    [Serializable]
    public class DalException : Exception
    {
        public DalException() { }
        public DalException(string message)
            : base(message)
        {
        }

        public DalException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected DalException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
