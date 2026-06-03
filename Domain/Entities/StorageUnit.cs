using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Domain.Entities
{
    public class StorageUnit
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // Например, "Бокс-12", "Склад-3"
        public string Type { get; set; } = string.Empty;  // "Холодильный", "Сухой", "Морозильный" и т.д.
        public decimal Price { get; set; }  // Цена за период (день/неделя/месяц)
        public string Description { get; set; } = string.Empty;
        public double Area { get; set; }   // Площадь в м² (вместо Capacity)
        public bool IsAvailable { get; set; } = true;

        public List<Booking> Bookings { get; set; } = new();
    }
}
