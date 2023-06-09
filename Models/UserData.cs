using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;

namespace Academy2023.Net.Models
{
    public class UserData
    {
        public int UserDataID { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int GenderID { get; set; }
        public Gender Gender { get; set; } = new Gender();
        public DateOnly BirthDate { get; set; }
        [Display(Name = "Tax Code")]
        [RegularExpression(@"^[a-zA-Z]{6}[0-9]{2}[a-zA-Z][0-9]{2}[a-zA-Z][0-9]{3}[a-zA-Z]$",ErrorMessage = "The tax code is invalid")]
        public string CF { get; set; } = string.Empty;
        [Display(Name = "Driver License Number")]
        [RegularExpression(@"^[A-z][AaVv][A-z0-9_]{7}[A-z]$", ErrorMessage = "Not Valid")]
        public string License { get; set; } = string.Empty;
        public string AuthID { get; set; }
        //[Required(AllowEmptyStrings = true)]
        public string? Description { get; set; } = string.Empty;
        public bool HasCar { set; get; }
        public List<Car> UserCar { get; set; } = new List<Car>();

        //public List<UserDataRide> UserDataRides { get; } = new List<UserDataRide>();
        public Picture? Picture { get; set; }

        public List<UserDataRide> UserDataRides { get; set; } = new List<UserDataRide>();    

    }

    public class Car
    {
        public int CarID { get; set; }
        public int UserDataID { get; set; }
        public UserData User { get; set; } = new UserData();
        [Range(1, 10, ErrorMessage = "Number of seats must be between {1} and {2}.")]
        public int MaxSeat { get; set; }
        [Display(Name = "Registration Number")]
        [RegularExpression(@"^[ABCDEFGHJKLMNPRSTVWXYZ]{2}[0-9]{3}[ABCDEFGHJKLMNPRSTVWXYZ]{2}$", ErrorMessage = "The Registration Number is invalid")]
        public string RegNum { get; set; }  // Targa
        public int FuelTypeID { get; set; }
        public FuelType FuelType { get; set; } = new FuelType();
        public int CarCategoryID { get; set; }
        public CarCategory CarCategory { get; set; } = new CarCategory();
        public bool TrunkAvailable { get; set; }    // Disponibilità Bagagliaio
        public List<Ride> Rides { get; set; } = new List<Ride>();
    }

    public class UserDataRide {
        public int ID { get; set; }
        public int UserDataID { get; set; }  
        public int RideId { get; set; }
        public UserData UserData { get; set; }
        public Ride Ride { get; set; } 
    }
    public class FuelType
    {
        public int FuelTypeID { get; set; }
        public string FuelName { get; set; } = string.Empty;
        public List<Car> Users { get; set; } = new List<Car>();
    }
    public class CarCategory
    {
        public int CarCategoryID { get; set; }
        public string CarName { get; set; } = string.Empty;
        public List<Car> Users { get; set; } = new List<Car>();
    }
    public class Gender
    {
        public int GenderID { get; set; }
        public string GenderName { get; set; } = string.Empty;
        public List<UserData> Users { get; set; } = new List<UserData>();
    }
    public class Picture
    {
        public int PictureID { get; set; }
        public string PictureName { get; set; } = "";
        public byte[]? RawData { get; set; }
    }
}
