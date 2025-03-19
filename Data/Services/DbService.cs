using Data.Data;
using Data.Models;
using Microsoft.Data.SqlClient;

namespace Data.Services
{
    public class DbService
    {
        private readonly ServerDbContext _context;
        public DbService(ServerDbContext context)
        {

            _context = context;
        }

        public string CreateTable(TaxPayer taxPayer)
        {
            try
            {
                _context.TaxPayers.Add(taxPayer);
                _context.SaveChanges();
                return "Success";
            }
            catch (SqlException ex)
            {
                return "Failed" + ex.Message;
            }
        }
        public string UpdateTable(TaxPayer taxPayer)
        {
            try
            {
                var existingTaxPayer = _context.TaxPayers.Find(taxPayer.Id);
                if (existingTaxPayer == null)
                {
                    return "Failed: TaxPayer not found";
                }

                _context.Entry(existingTaxPayer).CurrentValues.SetValues(taxPayer);
                _context.SaveChanges();
                return "Success";
            }
            catch (SqlException ex)
            {
                return "Failed: " + ex.Message;
            }
        }
        public List<TaxPayer> queryDatabase()
        {
            return _context.TaxPayers.ToList();
        }
        public string DeleteTable(TaxPayer taxpayer)
        {
            try
            {
                _context.TaxPayers.Remove(taxpayer);
                _context.SaveChanges();
                return "Success";
            }
            catch (Exception ex)
            {
                return "Failed" + ex.Message;
            }
        }

        public object searchForPayer(string query)
        {
            return _context.TaxPayers.Where(x => x.Name.Contains(query) || x.Address.Contains(query) || x.PostalCode.ToString().Contains(query)).ToList();
        }
    }
}
