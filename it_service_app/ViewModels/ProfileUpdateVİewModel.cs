namespace it_service_app.ViewModels
{
    public class ProfileUpdateVİewModel
    {

        public UserProfileViewModel UserProfileViewModel { get; set; } = new();

        public PasswordUpdateViewModel PasswordUpdateViewModel { get; set; }=new();
    }
}
