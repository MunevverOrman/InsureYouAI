using System.ComponentModel.DataAnnotations;

namespace InsureYouAI.Dtos
{
    public class CreateUserRegisterDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }


        [Range(typeof(bool), "true", "true", ErrorMessage = "Kullanım şartlarını kabul etmelisiniz.")]
        public bool TermsAccepted { get; set; }
    }
}
