using Microsoft.EntityFrameworkCore;

namespace WaybillSearchApp.Models;

[Index(nameof(WaybillNumber), IsUnique = true)]
public class Waybill
{
    public int Id { get; set; }
    public string WaybillNumber { get; set; } = string.Empty;
    public string ServiceType { get; set; } = string.Empty;
    public string SenderSuburb { get; set; } = string.Empty;
    public string SenderPostalCode { get; set; } = string.Empty;
    public string RecipientSuburb { get; set; } = string.Empty;
    public string RecipientPostalCode { get; set; } = string.Empty;

    public List<Parcel> Parcels { get; set; } = new();
}
