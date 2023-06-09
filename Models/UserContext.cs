using Microsoft.EntityFrameworkCore;

namespace Academy2023.Net.Models
{
    public partial class DataContext : DbContext
    {
        public virtual DbSet<UserData> usersData { get; set; }
        public virtual DbSet<Car> cars { get; set; }
        public virtual DbSet<Ride> Rides { get; set; }
        public virtual DbSet<UserDataRide> userDataRides { get;set; }
        public virtual DbSet<FuelType> FuelTypes { get; set; }
        public virtual DbSet<CarCategory> CarCategories { get; set; }
        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<Picture> Pictures { get; set; }
    }
}
