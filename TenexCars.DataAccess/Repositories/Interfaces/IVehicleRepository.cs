﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TenexCars.DataAccess.Enums;
using TenexCars.DataAccess.Models;
using TenexCars.DataAccess.ViewModels;

namespace TenexCars.DataAccess.Repositories.Interfaces
{
    public interface IVehicleRepository
    {
        Task CreateVehicleAsync(CreateVehicleViewModel vehicleViewModel);
        Task<Vehicle> GetVehicleById(string Id);
        Task<IEnumerable<Vehicle>> GetAll(QueryObject query);
        void UpdateVehicle(Vehicle vehicle);
        public bool VehicleExists(string id);
        Task<Vehicle> AddVehicleAsync(Vehicle vehicle);
        Task<List<Vehicle>> GetVehiclesByOperator(string operatorId);
        /*Task<IEnumerable<Vehicle>> GetFilteredVehicles(State state, CarType carType, CarName carName, CarModel carModel);*/
    }
}