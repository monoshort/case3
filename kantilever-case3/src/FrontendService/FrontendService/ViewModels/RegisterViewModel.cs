using FrontendService.Models;

namespace FrontendService.ViewModels
{
    public class RegisterViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Naam { get; set; }
        public string Telefoonnummer { get; set; }
        public Adres Adres { get; set; }
    }
}