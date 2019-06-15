using System.ComponentModel.DataAnnotations.Schema;

namespace ImpoDoc.Entities
{
    [Table("Counters")]
    public class Counter : BaseEntity<Counter>
    {
        public int Sheets
        {
            get { return GetValue(() => Sheets); }
            set { SetValue(() => Sheets, value); }
        }

        public int Copy
        {
            get { return GetValue(() => Copy); }
            set { SetValue(() => Copy, value); }
        }
    }
}
