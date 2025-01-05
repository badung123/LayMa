using System.ComponentModel.DataAnnotations;

namespace LayMa.WebApp.Models
{
    public class User
    {
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Password")]
        public string Password { get; set; }

    }
}
