using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Fbay.Models
{
    public class Users
    {
        //Structure of the users table in the Fbay database
        [Display(Name = "User id")]
        [Key]
        public int Id { get; set; }

        [Required]
        [PersonalData]
        [Display(Name = "First name")]
        public string Fname { get; set; }

        [Required]
        [PersonalData]
        [Display(Name = "Last name")]
        public string Lname { get; set; }
    }
}
