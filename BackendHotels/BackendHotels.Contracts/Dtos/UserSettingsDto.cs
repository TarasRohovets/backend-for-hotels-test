using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendHotels.Contracts.Dtos
{
    public class UserSettingsDto
    {
        public string? OrderBy {get;set;}
        public string? Sort { get; set; }
        public string? InputValue { get; set; }
        public string? InputType { get; set; }
        public int? CurrentPage { get; set; }
    }
}
