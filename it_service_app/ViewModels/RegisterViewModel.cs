using System.ComponentModel.DataAnnotations;

namespace it_service_app.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="Kullanıcı adı alanı gereklidir")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Ad alanı gereklidir")]
        [Display(Name ="Ad")]
        [StringLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage ="Soyad alanı gereklidir")]
        [Display(Name ="Soyad")]
        [StringLength(50)]
        public string Surname { get; set; }

        [Required(ErrorMessage ="E-Posta alanı gereklidir")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage ="Şifre alanı gereklidir")]
        [StringLength (50,MinimumLength =6,ErrorMessage ="Sifreniz minimum 6 karakterli olmalıdır!")]
        [Display(Name ="Sifre")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage ="Sİfre tekrar alanı gereklidir")]
        [Display(Name = "Sifre Tekrar")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password),ErrorMessage ="Sİfreler uyusmuyor")]
        public string ConfirmPassword { get; set; }





    }
}
