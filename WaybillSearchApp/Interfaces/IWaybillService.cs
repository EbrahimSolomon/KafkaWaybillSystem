using WaybillSearchApp.Models;

namespace WaybillSearchApp.Interfaces
{
    public interface IWaybillService
    {
        Task<Waybill?> GetWaybillByNumberAsync(string waybillNumber);
        /*Task<List<Waybill>> GetAllWaybillsAsync();
        Task<Waybill?> GetWaybillByIdAsync(int id);
        Task CreateWaybillAsync(Waybill waybill);
        Task UpdateWaybillAsync(Waybill waybill);
        Task DeleteWaybillAsync(int id);*/
    }
}
