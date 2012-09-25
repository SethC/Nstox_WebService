using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSTOX.DAL.Model
{
    public enum ElementStatus
    {
        New,
        Update,
        Delete,
        Unchanged
    }

    public enum InputFileType
    {
        Items = 1,
        Departments = 2,
        Transactions = 3
    }

    public enum BOFileStatus
    {
        NotUploaded = 0,
        New = 1,
        Processing = 2,
        FileNotFound = 3,
        Done = 4,
    }
}
