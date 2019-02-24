using System.Collections.Generic;
using System.Threading.Tasks;
using ImpoDoc.Models;

namespace ImpoDoc.Services
{
    public interface IEmployeeDataService
    {
        void Add(Employee employee);
        void Update(Employee employee);
        void Remove(Employee employee);
        Task<List<Employee>> LoadAsync();
    }
}