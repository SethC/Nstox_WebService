using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSTOX.DAL.Model
{
    public class JobAudit : ModelBase
    {
        [UpdateableField(ExcludeOnInsert = true)]
        public int Id { get; set; }
        [UpdateableField(ExcludeOnUpdate = true)]
        public InputFileType FileType { get; set; }
        [UpdateableField(ExcludeOnUpdate = true)]
        public string FilePath { get; set; }
        [UpdateableField]
        public int ItemsProcessed { get; set; }
        [UpdateableField]
        public int NewItems { get; set; }
        [UpdateableField]
        public int UpdatedItems { get; set; }
        [UpdateableField]
        public int DeletedItems { get; set; }
        [UpdateableField]
        public int ErrorCount { get; set; }
        [UpdateableField]
        public long ProcessTime { get; set; }
        [UpdateableField]
        public BOFileStatus JobStatus { get; set; }

        public void IncrementNew(int errCount)
        {
            NewItems += errCount > 0 ? 0 : 1;
        }

        public void IncrementUpdated(int errCount)
        {
            UpdatedItems += errCount > 0 ? 0 : 1;
        }

        public void IncrementDeleted(int errCount)
        {
            DeletedItems += errCount > 0 ? 0 : 1;
        }

        public void IncrementError(int count)
        {
            ErrorCount += count;
        }
    }
}
