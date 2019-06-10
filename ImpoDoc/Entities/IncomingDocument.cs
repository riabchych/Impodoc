using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImpoDoc.Entities
{
    public class IncomingDocument : Document
    {
        public IncomingDocument() : base()
        {
            Correspondent = new Company();
            Resolution = new Resolution();
        }

        public int? CorrespondentId { get; set; }

        [ForeignKey("CorrespondentId")]
        public Company Correspondent
        {
            get { return GetValue(() => Correspondent); }
            set { SetValue(() => Correspondent, value); }
        }

        public DateTime ReceivedAt
        {
            get { return GetValue(() => ReceivedAt); }
            set { SetValue(() => ReceivedAt, value); }
        }

        public int? ResolutionId { get; set; }

        [ForeignKey("ResolutionId")]
        public Resolution Resolution
        {
            get { return GetValue(() => Resolution); }
            set { SetValue(() => Resolution, value); }
        }
    }
}
