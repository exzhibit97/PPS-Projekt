using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Movies.ViewModels.ManageViewModels
{
    public class DisplayRecoveryCodesViewModel
    {
        [Required]
        public IEnumerable<string> Codes { get; set; }

    }
}
