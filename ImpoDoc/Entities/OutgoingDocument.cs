using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImpoDoc.Entities
{
    [Table("Outgoing_docs")]
    public class OutgoingDocument: Document
    {
        public DateTime SentAt
        {
            get { return GetValue(() => SentAt); }
            set { SetValue(() => SentAt, value); }
        }

        public DateTime ReceivedAt
        {
            get { return GetValue(() => ReceivedAt); }
            set { SetValue(() => ReceivedAt, value); }
        }

        public int? CorrespondentId { get; set; }

        [ForeignKey("CorrespondentId")]
        public Company Correspondent
        {
            get { return GetValue(() => Correspondent); }
            set { SetValue(() => Correspondent, value); }
        }
    }
}
