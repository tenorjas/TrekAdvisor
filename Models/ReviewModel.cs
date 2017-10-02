using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TrekAdvisor.Models
{
    public class ReviewModel
    {
        [Key]
        public int ReviewID {get; set;}
        public string Title {get; set;}
        public string Body {get; set;}
        public DateTime DatePosted {get; set;} = DateTime.Now;
        public string DateOfStay {get; set;}
        public int HotelID {get; set;}
        public HotelModel Hotel {get; set;}

        // the following is necessary in order to link the user to the review
        public string ApplicationUserId {get; set;}
        public ApplicationUser ApplicationUser {get; set;}
    }
}