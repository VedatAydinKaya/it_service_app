using System.ComponentModel.DataAnnotations;

namespace it_service_app.ViewModels
{
    public class PasswordUpdateViewModel
    {
        [Required(ErrorMessage ="Eski sifre alanı gereklidir")]
        [StringLength(100,MinimumLength =6,ErrorMessage ="Sifreniz en az 6 karakterli olmalıdır")]
        [Display(Name ="Eski Sifre")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Yeni sifre alanı gereklidir")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Sifreniz en az 6 karakterli olmalıdır")]
        [Display(Name = "Yeni Sifre")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Sifre tekrar alanı gereklidir")]
        [DataType(DataType.Password)]
        [Display(Name ="Yeni Sifre Tekrar")]
        [Compare(nameof(NewPassword),ErrorMessage ="Sifreler uyusmuyor")]
        public string ConfirmNewPassword { get; set; }





    }
}
