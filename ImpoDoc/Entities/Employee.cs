using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImpoDoc.Entities
{
    [Table("Employees")]
    public class Employee : BaseEntity<Employee>
    {
        [DisplayName("Имя")]
        [Required(ErrorMessage = "Поле \"Имя\" обязательное")]
        [StringLength(15, ErrorMessage = "Длина имени не может превышать 20 символов")]
        public string FirstName
        {
            get { return GetValue(() => FirstName); }
            set { SetValue(() => FirstName, value); }
        }

        [DisplayName("Фамилия")]
        [Required(ErrorMessage = "Поле \"Фамилия\" обязательное")]
        [StringLength(15, ErrorMessage = "Длина фамилии не может превышать 20 символов")]
        public string LastName
        {
            get { return GetValue(() => LastName); }
            set { SetValue(() => LastName, value); }
        }

        [DisplayName("Отчество")]
        [Required(ErrorMessage = "Поле \"Отчество\" обязательное")]
        [StringLength(15, ErrorMessage = "Длина отчества не может превышать 20 символов")]
        public string MiddleName
        {
            get { return GetValue(() => MiddleName); }
            set { SetValue(() => MiddleName, value); }
        }

        [DisplayName("Пол")]
        [Required(ErrorMessage = "Выберете пол")]
        public Gender Gender
        {
            get { return GetValue(() => Gender); }
            set { SetValue(() => Gender, value); }
        }

        [DisplayName("Эл. почта")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Неверный адрес электронный почты")]
        [Required(ErrorMessage = "Поле \"Эл. почта\" обязательное")]
        public string Email
        {
            get { return GetValue(() => Email); }
            set { SetValue(() => Email, value); }
        }

        [DisplayName("Отдел")]
        [Required(ErrorMessage = "Поле \"Отдел\" обязательное")]
        [StringLength(50, ErrorMessage = "Название отдела не может превышать 150 символов")]
        public string Department
        {
            get { return GetValue(() => Department); }
            set { SetValue(() => Department, value); }
        }

        [DisplayName("Номер телефона")]
        [StringLength(10, ErrorMessage = "Длина номера телефона - 10 символов")]
        [MinLength(10, ErrorMessage = "Длина номера телефона - 10 символов")]
        public string PhoneNumber
        {
            get { return GetValue(() => PhoneNumber); }
            set { SetValue(() => PhoneNumber, value); }
        }

        [DisplayName("Дата рождения")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{1:dd.MM.yyyy}")]
        public string DateOfBirth
        {
            get { return GetValue(() => DateOfBirth); }
            set { SetValue(() => DateOfBirth, value); }
        }

        [DisplayName("Виконавець")]
        public bool Executor
        {
            get { return GetValue(() => Executor); }
            set { SetValue(() => Executor, value); }
        }

        [NotMapped]
        public string DisplayName
        {
            get
            {
                return $"{LastName} {FirstName} {MiddleName}";
            }
        }
    }
}
