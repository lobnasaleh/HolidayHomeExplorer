using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Villa_Web.Models.DTO;

namespace Villa_Web.Models.VM
{
    public class VillaNumberDeleteVM
    {

        public VillaNumberDeleteVM()
        {
            villaNumber=new VillaNumberDTO();
        }

        //villaNumber

        public VillaNumberDTO villaNumber {  get; set; }

        //villa
        [ValidateNever]
        //his attribute prevents the villaList property from being validated during model binding.
        //This is useful because villaList is typically populated from the server-side
        //(e.g., from a database or service) and doesn’t need client-side validation
        public IEnumerable<SelectListItem> villaList { get; set; }


     
    }
}
