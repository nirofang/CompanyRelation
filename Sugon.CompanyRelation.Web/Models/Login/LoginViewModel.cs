using System.ComponentModel.DataAnnotations;

namespace Sugon.CompanyRelation.Web.Models.Login
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "用户名")]
        //[EmailAddress]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密  码")]
        public string Password { get; set; }

        [Display(Name = "记住用户名")]
        public bool RememberMe { get; set; }
    }
}