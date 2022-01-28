using System.ComponentModel.DataAnnotations;

namespace it_service_app.ViewModels
{
    public class LoginViewModel
    {

        [Display(Name ="Kullanıcı Adı")]
        [Required(ErrorMessage ="Kullanıcı adı alanı  gereklidir")]
        public string UserName { get; set; }


        [Required(ErrorMessage ="Sifre Alanı gereklidir")]
        [StringLength(100,ErrorMessage ="Minimum sifreniz 6 karakterli olmalıdır",MinimumLength =6)]
        [Display(Name ="Sifre")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Beni hatırla")]
        public bool RememberMe { get; set; } 


    }
}
