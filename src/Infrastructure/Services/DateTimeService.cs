using OMS.Application.Common.Interfaces;

namespace OMS.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
