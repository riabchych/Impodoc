using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImpoDoc.Entities
{
    [Table("Checkouts")]
    public class Checkout : BaseEntity<Checkout>
    {
        public DateTime Date
        {
            get { return GetValue(() => Date); }
            set { SetValue(() => Date, value); }
        }

        public bool Status
        {
            get { return GetValue(() => Status); }
            set { SetValue(() => Status, value); }
        }
    }
}
