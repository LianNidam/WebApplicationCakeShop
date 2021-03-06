using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationCakeShop.Models
{
    public enum UserType { Buyer, Admin } //guest =0 and so on..
    public class User
    {
        //Users
        public int Id { get; set; }

        //ragex for username
        //change Hehara
        [RegularExpression("^[A-Za-z0-9]+(?:[_-] [A-Za-z0-9]+)*$", ErrorMessage = "User name can be only Letters and numbers")]
        [Required]
        [MinLength(4), MaxLength(20)]
        public string Username { get; set; }



        [RegularExpression("^(?=.*[A-Z])(?=.*[a-z])(?=.*[a-zA-Z])(?=.*[!@#$%^&*()_=+]).{6,}$", ErrorMessage = "Password must contain at least 1 nubmber, 1 upper and lower letters and 1 special char")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        //Only Admin can change it
        public UserType Type { get; set; } = UserType.Buyer;

        //ask about first name, last name.. etc..

        //change
        [MinLength(2), MaxLength(20)]
        [Required(ErrorMessage = "You don't have first name?")]
        public string Firstname { get; set; }

        //change
        [MinLength(2), MaxLength(20)]
        [Required(ErrorMessage = "You don't have last name?")]
        public string Lastname { get; set; }

        //adress for email adreess
        [Display(Name = "Email Adress")]
        [DataType(DataType.EmailAddress)]
        public string Address { get; set; }

        [DataType(DataType.PhoneNumber)]
        public int Phone { get; set; }

        public Cart Cart { get; set; } = new Cart();
    }
}

