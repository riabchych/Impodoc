using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImpoDoc.Entities
{
    [Table("Employees")]
    public class Employee : BaseEntity<Employee>
    {
        public string FirstName
        {
            get { return GetValue(() => FirstName); }
            set { SetValue(() => FirstName, value); }
        }

        public string LastName
        {
            get { return GetValue(() => LastName); }
            set { SetValue(() => LastName, value); }
        }

        public string MiddleName
        {
            get { return GetValue(() => MiddleName); }
            set { SetValue(() => MiddleName, value); }
        }

        public Gender Gender
        {
            get { return GetValue(() => Gender); }
            set { SetValue(() => Gender, value); }
        }

        public string Email
        {
            get { return GetValue(() => Email); }
            set { SetValue(() => Email, value); }
        }

        public string Department
        {
            get { return GetValue(() => Department); }
            set { SetValue(() => Department, value); }
        }

        public string PhoneNumber
        {
            get { return GetValue(() => PhoneNumber); }
            set { SetValue(() => PhoneNumber, value); }
        }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{1:dd.MM.yyyy}")]
        public string DateOfBirth
        {
            get { return GetValue(() => DateOfBirth); }
            set { SetValue(() => DateOfBirth, value); }
        }

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
