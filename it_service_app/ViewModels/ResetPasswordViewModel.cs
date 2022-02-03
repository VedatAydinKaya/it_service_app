using System.ComponentModel.DataAnnotations;

namespace it_service_app.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage ="Eski sifre alanı gereklidir")]
        [StringLength(100,MinimumLength =6,ErrorMessage ="Sifreniz minumum 6 karakterli olmalıdır")]
        [Display(Name ="Yeni sifre")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage ="Sifre tekrar alanı gereklidir")]
        [DataType(DataType.Password)]
        [Display(Name ="Yeni Sifre Tekrar")]
        [Compare(nameof(NewPassword),ErrorMessage ="Sifreler uyusmuyor")]
        public string ConfirmNewPassword { get; set; }

        public string Code { get; set; }
        public string UserId { get; set; }
    }
}
