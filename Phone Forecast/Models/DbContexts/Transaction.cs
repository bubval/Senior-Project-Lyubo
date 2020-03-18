using Phone_Forecast.Models.Enums;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Phone_Forecast.Models.DbContexts
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "This filed is required.")]
        [DisplayName("Phone:")]
        public PhoneModel Phone { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Purhase Date:")]
        [DataType(DataType.Date)]
        public DateTime PurchaseDate { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Price:")]
        public double Price { get; set; }
    }
}
