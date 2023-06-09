using Microsoft.EntityFrameworkCore;

namespace Academy2023.Net.Models {
    public partial class DataContext : DbContext{

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    }
}
