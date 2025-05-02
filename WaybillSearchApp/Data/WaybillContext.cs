using Microsoft.EntityFrameworkCore;
using WaybillSearchApp.Models;

namespace WaybillSearchApp.Data;

public class WaybillContext : DbContext
{
    public WaybillContext(DbContextOptions<WaybillContext> options) : base(options) { }

    public DbSet<Waybill> Waybills => Set<Waybill>();
    public DbSet<Parcel> Parcels => Set<Parcel>();
}
