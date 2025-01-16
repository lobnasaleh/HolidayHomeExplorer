using System.ComponentModel.DataAnnotations;

namespace MagicVillaApI2.Models.DTO
{
    public class VillaNumberDTO
    {
        [Required]
        public int VillaNo { get; set; }
        [Required]
        public int VillaId { get; set; }

        public string SpecialDetails { get; set; }

        public VillaDTO Villa { get; set; } //zawedt de 3shan a include el villa wa hatetha henna bas msh fel delete wa el update 3shan el get bas hya ely baab2a mehtagaha
    }
}
