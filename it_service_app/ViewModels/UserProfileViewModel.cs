using System.ComponentModel.DataAnnotations;

namespace it_service_app.ViewModels
{
    public class UserProfileViewModel
    {
        [Required(ErrorMessage ="Ad alanı gereklidir")]
        [Display( Name="Ad")]
        [StringLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Soyad alanı gereklidir")]
        [Display(Name = "Soyad")]
        [StringLength(50)]
        public string Surname { get; set; }

        [Required(ErrorMessage ="E-Posta alanı gereklidir")]
        [Display(Name="E-mail")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
