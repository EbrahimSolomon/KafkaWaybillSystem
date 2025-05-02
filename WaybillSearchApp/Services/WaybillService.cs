using Microsoft.EntityFrameworkCore;
using WaybillSearchApp.Data;
using WaybillSearchApp.Interfaces;
using WaybillSearchApp.Models;

namespace WaybillSearchApp.Services;

public class WaybillService : IWaybillService
{
    private readonly WaybillContext _context;

    public WaybillService(WaybillContext context)
    {
        _context = context;
    }

    public async Task<Waybill?> GetWaybillByNumberAsync(string waybillNumber)
    {
        return await _context.Waybills
            .Include(w => w.Parcels)
            .FirstOrDefaultAsync(w => w.WaybillNumber == waybillNumber);
    }

  /* public async Task<List<Waybill>> GetAllWaybillsAsync()
     {
         return await _context.Waybills.AsNoTracking().ToListAsync();
     }

     public async Task<Waybill?> GetWaybillByIdAsync(int id)
     {
         return await _context.Waybills.FindAsync(id);
     }

     public async Task CreateWaybillAsync(Waybill waybill)
     {
         _context.Waybills.Add(waybill);
         await _context.SaveChangesAsync();
     }

     public async Task UpdateWaybillAsync(Waybill waybill)
     {
         _context.Waybills.Update(waybill);
         await _context.SaveChangesAsync();
     }

     public async Task DeleteWaybillAsync(int id)
     {
         var waybill = await _context.Waybills.FindAsync(id);
         if (waybill != null)
         {
             _context.Waybills.Remove(waybill);
             await _context.SaveChangesAsync();
         }
     }*/
}
