using System.ComponentModel.DataAnnotations.Schema;

namespace ImpoDoc.Entities
{
    [Table("Companies")]
    public class Company : BaseEntity<Company>
    {
        public string Title
        {
            get { return GetValue(() => Title); }
            set { SetValue(() => Title, value); }
        }

        public string Location
        {
            get { return GetValue(() => Location); }
            set { SetValue(() => Location, value); }
        }

        public string LegalAddress
        {
            get { return GetValue(() => LegalAddress); }
            set { SetValue(() => LegalAddress, value); }
        }

        public string MailingAddress
        {
            get { return GetValue(() => MailingAddress); }
            set { SetValue(() => MailingAddress, value); }
        }

        public string INN
        {
            get { return GetValue(() => INN); }
            set { SetValue(() => INN, value); }
        }

        public string PhoneNumber
        {
            get { return GetValue(() => PhoneNumber); }
            set { SetValue(() => PhoneNumber, value); }
        }

        public string Director
        {
            get { return GetValue(() => Director); }
            set { SetValue(() => Director, value); }
        }
    }
}