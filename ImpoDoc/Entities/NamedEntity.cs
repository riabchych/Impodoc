namespace ImpoDoc.Entities
{
    public class NamedEntity : BaseEntity<NamedEntity>
    {
        public string Name
        {
            get { return GetValue(() => Name); }
            set { SetValue(() => Name, value); }
        }
    }
}