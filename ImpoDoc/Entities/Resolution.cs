using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImpoDoc.Entities
{
    [Table("Resolutions")]
    public class Resolution : BaseEntity<Resolution>
    {
        public string Text
        {
            get { return GetValue(() => Text); }
            set { SetValue(() => Text, value); }
        }

        public DateTime CreatedAt
        {
            get { return GetValue(() => CreatedAt); }
            set { SetValue(() => CreatedAt, value); }
        }
    }
}
