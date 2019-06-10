using System;

namespace ImpoDoc.Entities
{
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
