using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TestWorkindForEducation.Models
{
    public class FrontendStorageUnit
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;      // Бокс-12
        public string Type { get; set; } = string.Empty;      // Сухой, Холодильный...
        public decimal Price { get; set; }                    // Цена за период
        public string Description { get; set; } = string.Empty;
        public double Area { get; set; }                      // Площадь м²
        public bool IsAvailable { get; set; } = true;

        public string DisplayPrice => Price.ToString("N0") + " ₽";
        public string StatusBadgeClass => IsAvailable ? "bg-success" : "bg-danger";
        public string StatusText => IsAvailable ? "Свободен" : "Занят";
    }
}
