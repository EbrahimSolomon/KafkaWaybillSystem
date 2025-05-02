using Microsoft.EntityFrameworkCore;

namespace WaybillSearchApp.Models;

[Index(nameof(ParcelNumber), IsUnique = true)]
public class Parcel
{
    public int Id { get; set; }
    public string ParcelNumber { get; set; } = string.Empty;
    public double Length { get; set; }
    public double Breadth { get; set; }
    public double Height { get; set; }
    public double Mass { get; set; }
    public int WaybillId { get; set; }
    public Waybill Waybill { get; set; } = null!;
}
