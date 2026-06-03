using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CreateStorageUnitDto
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;  // Тип склада
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public double Area { get; set; }   // Площадь в м²
        public bool IsAvailable { get; set; } = true;

        public class UpdateStorageUnitStatusDto
        {
            public bool IsAvailable { get; set; }
        }
    }
}
