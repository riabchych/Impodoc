using ImpoDoc.Entities;

namespace ImpoDoc.Data
{
    public class CompanyContext: ApplicationContext<Company>
    {
        protected override string Name => "companies";
        protected override string FileName => "company.db";
    }
}
