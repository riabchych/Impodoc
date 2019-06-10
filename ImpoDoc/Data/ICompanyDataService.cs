using System.Collections.Generic;
using System.Threading.Tasks;
using ImpoDoc.Models;

namespace ImpoDoc.Models
{
    public interface ICompanyDataService
    {
        void Add(Company company);
        void Update(Company company);
        void Remove(Company company);
        Task<List<Company>> LoadAsync();
    }
}