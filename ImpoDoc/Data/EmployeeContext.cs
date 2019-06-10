using ImpoDoc.Entities;

namespace ImpoDoc.Data
{
    public class EmployeeContext : ApplicationContext<Employee>
    {
        protected override string Name => "employees";
        protected override string FileName => "employee.db";
    }
}
