namespace Academy2023.Net.Models {
    public class Ride {
        public int RideId { get; set; }
        public string Departure { get; set; }
        public string Arrival { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int Duration { get; set; }

        public double Km { get; set; }
        public int AvailableSeat{ get; set; }


        public int CarID { get; set; }
        public Car Car { get; set; } = new Car();

        public List<UserData> Passengers { get; set; } = new List<UserData>();  
        //public List<UserDataRide> UserDataRides { get; } = new List<UserDataRide>();

        public List<UserDataRide> UserDataRides { get; set;} = new List<UserDataRide>();

        public TimeSpan GetTimeSpan {
            get
            {
                return TimeSpan.FromSeconds(Duration);
            }
        }
    }
}
