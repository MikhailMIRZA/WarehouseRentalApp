using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class FrontendBooking
{
    public int Id { get; set; }
    public int StorageUnitId { get; set; }                   // было RoomId
    public string StorageUnitName { get; set; } = string.Empty; // было RoomName
    public string StorageUnitType { get; set; } = string.Empty; // было RoomClass
    public string UserName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; } = "Confirmed";
    public decimal TotalPrice { get; set; }

    // Вычисляемые свойства
    public string FormattedStartDate => StartDate.ToString("dd.MM.yyyy");
    public string FormattedEndDate => EndDate.ToString("dd.MM.yyyy");
    public string StatusBadgeClass => Status switch
    {
        "Confirmed" => "bg-success",
        "Cancelled" => "bg-danger",
        "Completed" => "bg-info",
        _ => "bg-secondary"
    };
    public string StatusText => Status switch
    {
        "Confirmed" => "Подтверждено",
        "Cancelled" => "Отменено",
        "Completed" => "Завершено",
        _ => "Ожидание"
    };
}