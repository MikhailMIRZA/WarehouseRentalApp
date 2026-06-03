using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Booking
{
    public int Id { get; set; }
    public int StorageUnitId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;

    
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    
    public BookingStatus Status { get; set; } = BookingStatus.Confirmed;

    public virtual StorageUnit? StorageUnit { get; set; }
}

public enum BookingStatus
{
    Confirmed,
    Cancelled,
    Completed
}