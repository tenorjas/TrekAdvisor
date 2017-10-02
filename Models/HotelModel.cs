using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TrekAdvisor.Models
{
    public class HotelModel
    {
        [Key]
        public int HotelID {get; set;}
        public string HotelName {get; set;}
        public int StreetAddress {get; set;}
        public string StreetName {get; set;}
        public string City {get; set;}
        public string State {get; set;}
        public string Country {get; set;}
        public string PostalCode {get; set;}
        public int StarRating {get; set;}

        // the following is needed for photos
        public byte[] OutsidePhoto {get; set;}
        public byte[] InsidePhoto {get; set;}
        public int OutsideWidth {get; set;}
        public int OutsideHeight {get; set;}
        public string OutsideContentType {get; set;}
        public int InsideWidth {get; set;}
        public int InsideHeight {get; set;}
        public string InsideContentType {get; set;}

        // the following is necessary in order to link reviews to the hotel
        public ICollection<ReviewModel> Reviews {get; set;} = new HashSet<ReviewModel>();
    }
}