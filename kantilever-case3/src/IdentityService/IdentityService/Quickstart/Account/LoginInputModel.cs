// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IdentityServer4.Quickstart.UI
{
    public class LoginInputModel
    { 
        [Required]
        [DisplayName("Gebruikersnaam")]
        public string Username { get; set; }
        
        [Required] 
        [DisplayName("Wachtwoord")]
        public string Password { get; set; }
        
        [DisplayName("Onthoud mij")]
        public bool RememberLogin { get; set; }
        
        public string ReturnUrl { get; set; }
    }
}