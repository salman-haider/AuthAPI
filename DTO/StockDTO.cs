using API.Model;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.DTO
{
    public class StockDTO
    {
        [Required]
        [MinLength(3, ErrorMessage = "Symbol minimum 3 characters are required")]
        [MaxLength(4, ErrorMessage = "Symbol cannot exceed 4 characters")]
        public string Symbol { get; set; } = string.Empty;
        [Required]
        [MinLength(5, ErrorMessage = "Company Name minimum 5 characters are required")]
        [MaxLength(120, ErrorMessage = "Company Name cannot exceed 120 characters")]
        public string CompanyName { get; set; } = string.Empty;
        public decimal Purchase { get; set; }
        public decimal LastDiv { get; set; }
        public string Industry { get; set; } = string.Empty;
        public long MarketCap { get; set; }
    }


}
