using MessageConsumer.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Globalization;

namespace MessageConsumer.Services;

public class PostgresService : IMessageProcessorService
{
    private readonly string _connectionString;
    private readonly ILogger<PostgresService> _logger;

    public PostgresService(IConfiguration config, ILogger<PostgresService> logger)
    {
        _connectionString = config["Postgres:ConnectionString"]!;
        _logger = logger;
    }

    /*  public async Task ProcessMessageAsync(string message)
      {
          try
          {
              await using var conn = new NpgsqlConnection(_connectionString);
              await conn.OpenAsync();

              var cmd = new NpgsqlCommand("INSERT INTO file_events (content) VALUES (@content)", conn);
              cmd.Parameters.AddWithValue("content", message);

              await cmd.ExecuteNonQueryAsync();

              _logger.LogInformation("Inserted message into PostgreSQL.");
          }
          catch (Exception ex)
          {
              _logger.LogError(ex, "Failed to insert message into PostgreSQL");
          }
      }*/

    /* public async Task ProcessMessageAsync(string message)
     {
         var parts = message.Split('|');

         if (parts.Length != 11)
         {
             _logger.LogError("Invalid message format received: {Message}", message);
             return;
         }

         var waybillNumber = parts[0].Trim();
         var serviceType = parts[1].Trim();
         var senderSuburb = parts[2].Trim();
         var senderPostalCode = parts[3].Trim();
         var recipientSuburb = parts[4].Trim();
         var recipientPostalCode = parts[5].Trim();
         var parcelNumber = parts[6].Trim();
         var length = double.Parse(parts[7].Trim(), CultureInfo.InvariantCulture);
         var breadth = double.Parse(parts[8].Trim(), CultureInfo.InvariantCulture);
         var height = double.Parse(parts[9].Trim(), CultureInfo.InvariantCulture);
         var mass = double.Parse(parts[10].Trim(), CultureInfo.InvariantCulture);

         await using var conn = new NpgsqlConnection(_connectionString);
         await conn.OpenAsync();

         await using var transaction = await conn.BeginTransactionAsync();

         try
         {
             // Insert or update Waybill
             var cmdWaybill = new NpgsqlCommand(@"
 INSERT INTO ""Waybills"" (""WaybillNumber"", ""ServiceType"", ""SenderSuburb"", ""SenderPostalCode"", ""RecipientSuburb"", ""RecipientPostalCode"")
 VALUES (@WaybillNumber, @ServiceType, @SenderSuburb, @SenderPostalCode, @RecipientSuburb, @RecipientPostalCode)
 ON CONFLICT (""WaybillNumber"") DO UPDATE SET
     ""ServiceType"" = EXCLUDED.""ServiceType"",
     ""SenderSuburb"" = EXCLUDED.""SenderSuburb"",
     ""SenderPostalCode"" = EXCLUDED.""SenderPostalCode"",
     ""RecipientSuburb"" = EXCLUDED.""RecipientSuburb"",
     ""RecipientPostalCode"" = EXCLUDED.""RecipientPostalCode""
 RETURNING ""Id"";", conn);

             cmdWaybill.Parameters.AddWithValue("WaybillNumber", waybillNumber);
             cmdWaybill.Parameters.AddWithValue("ServiceType", serviceType);
             cmdWaybill.Parameters.AddWithValue("SenderSuburb", senderSuburb);
             cmdWaybill.Parameters.AddWithValue("SenderPostalCode", senderPostalCode);
             cmdWaybill.Parameters.AddWithValue("RecipientSuburb", recipientSuburb);
             cmdWaybill.Parameters.AddWithValue("RecipientPostalCode", recipientPostalCode);

             var waybillId = (int)await cmdWaybill.ExecuteScalarAsync();

             // Insert or update Parcel
             var cmdParcel = new NpgsqlCommand(@"
 INSERT INTO ""Parcels"" (""ParcelNumber"", ""Length"", ""Breadth"", ""Height"", ""Mass"", ""WaybillId"")
 VALUES (@ParcelNumber, @Length, @Breadth, @Height, @Mass, @WaybillId)
 ON CONFLICT (""ParcelNumber"") DO UPDATE SET
     ""Length"" = EXCLUDED.""Length"",
     ""Breadth"" = EXCLUDED.""Breadth"",
     ""Height"" = EXCLUDED.""Height"",
     ""Mass"" = EXCLUDED.""Mass"",
     ""WaybillId"" = EXCLUDED.""WaybillId"";", conn);

             cmdParcel.Parameters.AddWithValue("ParcelNumber", parcelNumber);
             cmdParcel.Parameters.AddWithValue("Length", length);
             cmdParcel.Parameters.AddWithValue("Breadth", breadth);
             cmdParcel.Parameters.AddWithValue("Height", height);
             cmdParcel.Parameters.AddWithValue("Mass", mass);
             cmdParcel.Parameters.AddWithValue("WaybillId", waybillId);

             await cmdParcel.ExecuteNonQueryAsync();

             await transaction.CommitAsync();

             _logger.LogInformation("Successfully processed Waybill: {WaybillNumber}, Parcel: {ParcelNumber}", waybillNumber, parcelNumber);
         }
         catch (Exception ex)
         {
             await transaction.RollbackAsync();
             _logger.LogError(ex, "Failed to process Waybill: {WaybillNumber}, Parcel: {ParcelNumber}", waybillNumber, parcelNumber);
         }
     }*/

    public async Task ProcessMessageAsync(string message)
    {
        var lines = message.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in lines)
        {
            var parts = line.Split('|');
            if (parts.Length != 11)
            {
                _logger.LogError("Invalid message format received: {Message}", line);
                continue;
            }

            var waybillNumber = parts[0].Trim();
            var serviceType = parts[1].Trim();
            var senderSuburb = parts[2].Trim();
            var senderPostalCode = parts[3].Trim();
            var recipientSuburb = parts[4].Trim();
            var recipientPostalCode = parts[5].Trim();
            var parcelNumber = parts[6].Trim();
            var length = double.Parse(parts[7].Trim(), CultureInfo.InvariantCulture);
            var breadth = double.Parse(parts[8].Trim(), CultureInfo.InvariantCulture);
            var height = double.Parse(parts[9].Trim(), CultureInfo.InvariantCulture);
            var mass = double.Parse(parts[10].Trim(), CultureInfo.InvariantCulture);

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var transaction = await conn.BeginTransactionAsync();

            try
            {
                var cmdWaybill = new NpgsqlCommand(@"
INSERT INTO ""Waybills"" (""WaybillNumber"", ""ServiceType"", ""SenderSuburb"", ""SenderPostalCode"", ""RecipientSuburb"", ""RecipientPostalCode"")
VALUES (@WaybillNumber, @ServiceType, @SenderSuburb, @SenderPostalCode, @RecipientSuburb, @RecipientPostalCode)
ON CONFLICT (""WaybillNumber"") DO UPDATE SET
    ""ServiceType"" = EXCLUDED.""ServiceType"",
    ""SenderSuburb"" = EXCLUDED.""SenderSuburb"",
    ""SenderPostalCode"" = EXCLUDED.""SenderPostalCode"",
    ""RecipientSuburb"" = EXCLUDED.""RecipientSuburb"",
    ""RecipientPostalCode"" = EXCLUDED.""RecipientPostalCode""
RETURNING ""Id"";", conn);

                cmdWaybill.Parameters.AddWithValue("WaybillNumber", waybillNumber);
                cmdWaybill.Parameters.AddWithValue("ServiceType", serviceType);
                cmdWaybill.Parameters.AddWithValue("SenderSuburb", senderSuburb);
                cmdWaybill.Parameters.AddWithValue("SenderPostalCode", senderPostalCode);
                cmdWaybill.Parameters.AddWithValue("RecipientSuburb", recipientSuburb);
                cmdWaybill.Parameters.AddWithValue("RecipientPostalCode", recipientPostalCode);

                var waybillId = (int)await cmdWaybill.ExecuteScalarAsync();

                var cmdParcel = new NpgsqlCommand(@"
INSERT INTO ""Parcels"" (""ParcelNumber"", ""Length"", ""Breadth"", ""Height"", ""Mass"", ""WaybillId"")
VALUES (@ParcelNumber, @Length, @Breadth, @Height, @Mass, @WaybillId)
ON CONFLICT (""ParcelNumber"") DO UPDATE SET
    ""Length"" = EXCLUDED.""Length"",
    ""Breadth"" = EXCLUDED.""Breadth"",
    ""Height"" = EXCLUDED.""Height"",
    ""Mass"" = EXCLUDED.""Mass"",
    ""WaybillId"" = EXCLUDED.""WaybillId"";", conn);

                cmdParcel.Parameters.AddWithValue("ParcelNumber", parcelNumber);
                cmdParcel.Parameters.AddWithValue("Length", length);
                cmdParcel.Parameters.AddWithValue("Breadth", breadth);
                cmdParcel.Parameters.AddWithValue("Height", height);
                cmdParcel.Parameters.AddWithValue("Mass", mass);
                cmdParcel.Parameters.AddWithValue("WaybillId", waybillId);

                await cmdParcel.ExecuteNonQueryAsync();

                await transaction.CommitAsync();
                _logger.LogInformation("Successfully processed Waybill: {WaybillNumber}, Parcel: {ParcelNumber}", waybillNumber, parcelNumber);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Failed to process Waybill: {WaybillNumber}, Parcel: {ParcelNumber}", waybillNumber, parcelNumber);
            }
        }
    }



}
