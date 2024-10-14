using TenexCars.DataAccess.Enums;

namespace TenexCars.DataAccess.ViewModels
{
    public class QueryObject
    {
        public State? State { get; set; }
        public CarName? CarName { get; set; } 
        public CarModel? CarModel { get; set; }
        public CarType? CarType {get; set; } 

    }
}
