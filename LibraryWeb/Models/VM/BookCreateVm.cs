using LibraryWeb.Models.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryWeb.Models.VM {
    public class BookCreateVm {
        public BookCreateVm()
        {
             Book = new BookCreateDTO();
        }
        public BookCreateDTO Book { get; set; }
         [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }
    }
}