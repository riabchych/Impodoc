using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImpoDoc.Entities
{
    public class Document : BaseEntity<Document>
    {
        public Document()
        {
            Attachment = new Attachment();
            Checkout = new Checkout();
            Counter = new Counter();
            Execution = new Execution();
        }
        public string Name
        {
            get { return GetValue(() => Name); }
            set { SetValue(() => Name, value); }
        }

        public string IncomingIndex
        {
            get { return GetValue(() => IncomingIndex); }
            set { SetValue(() => IncomingIndex, value); }
        }

        public string OutgoingIndex
        {
            get { return GetValue(() => OutgoingIndex); }
            set { SetValue(() => OutgoingIndex, value); }
        }

        public DateTime CreatedAt
        {
            get { return GetValue(() => CreatedAt); }
            set { SetValue(() => CreatedAt, value); }
        }

        public string Description
        {
            get { return GetValue(() => Description); }
            set { SetValue(() => Description, value); }
        }

        public string DocumentType
        {
            get { return GetValue(() => DocumentType); }
            set { SetValue(() => DocumentType, value); }
        }

        public string Media
        {
            get { return GetValue(() => Media); }
            set { SetValue(() => Media, value); }
        }

        public string Location
        {
            get { return GetValue(() => Location); }
            set { SetValue(() => Location, value); }
        }

        public int? AttachmentId { get; set; }

        [ForeignKey("AttachmentId")]
        public Attachment Attachment
        {
            get { return GetValue(() => Attachment); }
            set { SetValue(() => Attachment, value); }
        }

        public int? CheckoutId { get; set; }

        [ForeignKey("CheckoutId")]
        public Checkout Checkout
        {
            get { return GetValue(() => Checkout); }
            set { SetValue(() => Checkout, value); }
        }

        public int? ExecutionId { get; set; }

        [ForeignKey("ExecutionId")]
        public Execution Execution
        {
            get { return GetValue(() => Execution); }
            set { SetValue(() => Execution, value); }
        }

        public int? CounterId { get; set; }

        [ForeignKey("CounterId")]
        public Counter Counter
        {
            get { return GetValue(() => Counter); }
            set { SetValue(() => Counter, value); }
        }
    }
}
