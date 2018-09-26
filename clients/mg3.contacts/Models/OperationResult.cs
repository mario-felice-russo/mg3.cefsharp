using System;
using System.Collections.Generic;

namespace Models
{
    public class SqliteResult<T> : OperationResult<T> 
    {
        public SqliteResult() : base()
        { }

        public override  bool InError { get { return Error != null && (Entity != null || (Entities != null && Entities.Count > 0)); } }
    }

    public class OperationResult<T> : OperationResult
    {
        public OperationResult()
        {
            Entities = new List<T>();
        }

        public T Entity { get; set; }
        public List<T> Entities { get; set; }

        public override bool InError { get { return Error != null && (Entity != null || (Entities != null && Entities.Count > 0)); } }
    }

    public class OperationResult
    {
        public int NrRecords { get; set; }
        public string Message { get; set; }

        public virtual Exception Error { get; set; }
        public virtual bool InError { get { return Error != null; } }
    }
}
