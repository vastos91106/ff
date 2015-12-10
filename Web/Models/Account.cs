
using System.Collections.Generic;
using System.Linq;

namespace Web.Models
{
    public partial class ApplicationUser
    {
        public virtual ICollection<Film> UserFilms { get; set; } 
    }
}
