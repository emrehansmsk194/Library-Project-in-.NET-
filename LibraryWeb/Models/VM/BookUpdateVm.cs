using LibraryWeb.Models.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryWeb.Models.VM {
    public class BookUpdateVm {
        public BookUpdateVm() {
            Book = new BookUpdateDTO();
        } 
        public BookUpdateDTO Book { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }
    }
}