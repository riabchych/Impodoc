using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ImpoDoc.Entities
{
    public class NamedEntity : BaseEntity<NamedEntity>
    {
        [DisplayName("Наименование")]
        [Required(ErrorMessage = "Поле \"Наименование\" обязательное")]
        [StringLength(40, ErrorMessage = "Длина наименование не может превышать 40 символов")]
        public string Name
        {
            get { return GetValue(() => Name); }
            set { SetValue(() => Name, value); }
        }
    }
}