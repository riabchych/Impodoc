using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImpoDoc.Entities
{
    public class InternalDocument: Document
    {
        public InternalDocument() : base()
        {
            Addresser = new Employee();
            Addressee = new Employee();
        }

        public DateTime ReceivedAt
        {
            get { return GetValue(() => ReceivedAt); }
            set { SetValue(() => ReceivedAt, value); }
        }

        public int? AddresserId { get; set; }

        [ForeignKey("AddresserId")]
        public Employee Addresser
        {
            get { return GetValue(() => Addresser); }
            set { SetValue(() => Addresser, value); }
        }

        public int? AddresseeId { get; set; }

        [ForeignKey("AddresseeId")]
        public Employee Addressee
        {
            get { return GetValue(() => Addressee); }
            set { SetValue(() => Addressee, value); }
        }
    }
}
