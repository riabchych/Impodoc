using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ImpoDoc.Models
{
    public class Employee: PropertyChangedNotification
    {
        [Key]
        [DisplayName("Идентификатор")]
        public int Id { get; set; }

        [DisplayName("Имя")]
        [Required(ErrorMessage = "Поле \"Имя\" обязательное")]
        [StringLength(15, ErrorMessage = "Длина имени не может превышать 15 символов")]
        public string FirstName {
            get { return GetValue(() => FirstName); }
            set { SetValue(() => FirstName, value); }

        }

        [DisplayName("Фамилия")]
        [Required(ErrorMessage = "Поле \"Фамилия\" обязательное")]
        [StringLength(15, ErrorMessage = "Длина фамилии не может превышать 15 символов")]
        public string LastName
        {
            get { return GetValue(() => LastName); }
            set { SetValue(() => LastName, value); }

        }

        [DisplayName("Отчество")]
        [Required(ErrorMessage = "Поле \"Отчество\" обязательное")]
        [StringLength(15, ErrorMessage = "Длина отчества не может превышать 15 символов")]
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
        [StringLength(15, ErrorMessage = "Название отдела не может превышать 15 символов")]
        public string Department
        {
            get { return GetValue(() => Department); }
            set { SetValue(() => Department, value); }
        }

        [DisplayName("Номер телефона")]
        [StringLength(11, ErrorMessage = "Длина номера телефона - 11 символов")]
        [MinLength(11, ErrorMessage = "Длина номера телефона - 11 символов")]
        public string PhoneNumber
        {
            get { return GetValue(() => PhoneNumber); }
            set { SetValue(() => PhoneNumber, value); }
        }

        [DisplayName("Дата рождения")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime DateOfBirth
        {
            get { return GetValue(() => DateOfBirth); }
            set { SetValue(() => DateOfBirth, value); }
        }

        public Employee ShallowCopy()
        {
            return (Employee)MemberwiseClone();
        }
    }
}
