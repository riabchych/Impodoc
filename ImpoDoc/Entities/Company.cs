using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ImpoDoc.Entities
{
    public class Company : BaseEntity<Company>
    {
        [DisplayName("Наименование")]
        [Required(ErrorMessage = "Поле \"Наименование\" обязательное")]
        [StringLength(40, ErrorMessage = "Длина наименование не может превышать 40 символов")]
        public string Title
        {
            get { return GetValue(() => Title); }
            set { SetValue(() => Title, value); }
        }

        [DisplayName("Город")]
        [Required(ErrorMessage = "Поле \"Город\" обязательное")]
        [StringLength(40, ErrorMessage = "Длина Город не может превышать 20 символов")]
        public string Location
        {
            get { return GetValue(() => Location); }
            set { SetValue(() => Location, value); }
        }

        [DisplayName("Юридический адрес")]
        [Required(ErrorMessage = "Поле \"Юридический адрес\" обязательное")]
        public string LegalAddress
        {
            get { return GetValue(() => LegalAddress); }
            set { SetValue(() => LegalAddress, value); }
        }

        [DisplayName("Почтовый адрес")]
        [Required(ErrorMessage = "Поле \"Почтовый адрес\" обязательное")]
        public string MailingAddress
        {
            get { return GetValue(() => MailingAddress); }
            set { SetValue(() => MailingAddress, value); }
        }

        [DisplayName("ИНН")]
        [Required(ErrorMessage = "Поле \"ИНН\" обязательное")]
        public string INN
        {
            get { return GetValue(() => INN); }
            set { SetValue(() => INN, value); }
        }

        [DisplayName("Номер телефона")]
        [StringLength(11, ErrorMessage = "Длина номера телефона - 11 символов")]
        [MinLength(11, ErrorMessage = "Длина номера телефона - 11 символов")]
        public string PhoneNumber
        {
            get { return GetValue(() => PhoneNumber); }
            set { SetValue(() => PhoneNumber, value); }
        }

        [DisplayName("Руководитель")]
        [Required(ErrorMessage = "Поле \"Руководитель\" обязательное")]
        [StringLength(40, ErrorMessage = "Длина имени не может превышать 40 символов")]
        public string Director
        {
            get { return GetValue(() => Director); }
            set { SetValue(() => Director, value); }
        }
    }
}