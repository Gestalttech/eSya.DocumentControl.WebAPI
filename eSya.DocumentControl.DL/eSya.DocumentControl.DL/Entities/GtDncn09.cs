using System;
using System.Collections.Generic;

namespace eSya.DocumentControl.DL.Entities
{
    public partial class GtDncn09
    {
        public int BusinessKey { get; set; }
        public string CalendarKey { get; set; } = null!;
        public int DocumentId { get; set; }
        public string TransactionMode { get; set; } = null!;
        public int StoreCode { get; set; }
        public int MonthId { get; set; }
        public long StartDocNumber { get; set; }
        public long CurrentDocNumber { get; set; }
        public DateTime CurrentDocDate { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; } = null!;
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; } = null!;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedTerminal { get; set; }
    }
}
