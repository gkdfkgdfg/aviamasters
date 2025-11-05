using WarehouseManagement.Application.Common.Interfaces;

namespace WarehouseManagement.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
