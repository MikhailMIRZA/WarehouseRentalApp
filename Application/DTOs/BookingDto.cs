using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class BookingDto
    {
        public int Id { get; set; }
        public int StorageUnitId { get; set; }
        public string StorageUnitName { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;

        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public BookingStatus Status { get; set; }

        public StorageUnitDto? StorageUnit { get; set; }
    }
}