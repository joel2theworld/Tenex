using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TenexCars.DataAccess.Enums;
using TenexCars.DataAccess.Models;
using TenexCars.DataAccess.Repositories.Interfaces;
using TenexCars.DataAccess.ViewModels;

namespace TenexCars.DataAccess.Repositories.Implementations
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly ApplicationDbContext _context;
        public VehicleRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Vehicle> GetVehicleById(string Id)
        {
            return await _context.Vehicles.FirstOrDefaultAsync(x => x.Id == Id);
        }
        public async Task<IEnumerable<Vehicle>> GetAll(QueryObject query)
        {
            var vehicles = _context.Vehicles.AsQueryable();

            if (query.CarModel != null)
            {
                vehicles = vehicles.Where(v => v.CarModel == query.CarModel);
            }
            if (query.CarName != null)
            {
                vehicles = vehicles.Where(v => v.CarName == query.CarName);
            }
            if (query.CarType != null)
            {
                vehicles = vehicles.Where(v => v.Cartype == query.CarType);
            }
            if (query.State != null)
            {
                vehicles = vehicles.Where(v => v.State == query.State);
            }

            return await vehicles.ToListAsync();
        }
        public async Task CreateVehicleAsync(CreateVehicleViewModel vehicleViewModel)
        {
            var vehicle = new Vehicle
            {
                Id = Guid.NewGuid().ToString(),
                Make = vehicleViewModel.Make,
                Model = vehicleViewModel.Model,
                PlateNumber = vehicleViewModel.PlateNumber,
                CarDescription = vehicleViewModel.CarDescription,
                ChasisNumber = vehicleViewModel.ChasisNumber,
                SeatNumbers = vehicleViewModel.SeatNumbers,
                Mileage = vehicleViewModel.Mileage,
                TrunkSize = vehicleViewModel.TrunkSize,
                Color = vehicleViewModel.Color,
                VIN = vehicleViewModel.VIN,
                PickupAddress = vehicleViewModel.PickupAddress,
                City = vehicleViewModel.City,
                ZIP = vehicleViewModel.ZIP,
                OperatorId = vehicleViewModel.OperatorId,
                CarDealerName = vehicleViewModel.CarDealerName,
                FinacialStartDate = vehicleViewModel.FinacialStartDate,
                FinacialEndDate = vehicleViewModel.FinacialEndDate,
                CarWarrantyOverview = vehicleViewModel.CarWarrantyOverview,
                Torque = vehicleViewModel.Torque,
                TransmissionType = vehicleViewModel.TransmissionType,
                Horsepower = vehicleViewModel.Horsepower,
                TurningDiameter = vehicleViewModel.TurningDiameter,
                CurbWeight = vehicleViewModel.CurbWeight,
                DiscBrakes = vehicleViewModel.DiscBrakes,
                TransmissionSpeed = vehicleViewModel.TransmissionSpeed,
                DriveAxleRatio = vehicleViewModel.DriveAxleRatio,
                StabilityControl = vehicleViewModel.StabilityControl,
                RangeOfFullCharge = vehicleViewModel.RangeOfFullCharge,
                CargoSpace = vehicleViewModel.CargoSpace,
                TouchScreenDisplay = vehicleViewModel.TouchScreenDisplay,
                DriverLumbarSupport = vehicleViewModel.DriverLumbarSupport,
                NumberOfSpeakers = vehicleViewModel.NumberOfSpeakers,
                Radio = vehicleViewModel.Radio,
                AndroidAuto_AppleCarPlay = vehicleViewModel.AndroidAuto_AppleCarPlay,
                TouchScreenNavSystem = vehicleViewModel.TouchScreenNavSystem,
                ProjectorBeamLedHeadlight = vehicleViewModel.ProjectorBeamLedHeadlight,
                RemoteKeylessEntry = vehicleViewModel.RemoteKeylessEntry,
                StandardLowTirePressureWarning = vehicleViewModel.StandardLowTirePressureWarning,
                BluetoothSystem = vehicleViewModel.BluetoothSystem,
                WheelDrive = vehicleViewModel.WheelDrive,
                EngineType = vehicleViewModel.EngineType,
                CarName = vehicleViewModel.CarName,
                CarModel = vehicleViewModel.CarModel,
                State = vehicleViewModel.State,
                DcFastCharging = vehicleViewModel.DcFastCharging,
                HomeCharger = vehicleViewModel.HomeCharger,
                SeatHeater = vehicleViewModel.SeatHeater,
                Cartype = vehicleViewModel.Cartype,
                ReservationFee = vehicleViewModel.ReservationFee,
                TotalCostOfCar = vehicleViewModel.TotalCostOfCar,
                ActivationFee = vehicleViewModel.ActivationFee,
                MonthlyCost = vehicleViewModel.MonthlyCost,
                MarketValue = vehicleViewModel.MarketValue,
                DecrementMarketValue = vehicleViewModel.DecrementMarketValue,
                //ImageUrl = vehicleViewModel.ImageUrl,
                //PublicId = vehicleViewModel.PublicId
            };

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();
        }
        public void UpdateVehicle(Vehicle vehicle)
        {
            _context.Vehicles.Update(vehicle);
            _context.SaveChanges();
        }
        public bool VehicleExists(string id)
        {
            return _context.Vehicles.Any(v => v.Id == id);
        }

        public async Task<Vehicle> AddVehicleAsync(Vehicle vehicle)
        {
            var newVehicle = await _context.Vehicles.AddAsync(vehicle);
            await _context.SaveChangesAsync();

            return newVehicle.Entity;
        }

        public async Task<List<Vehicle>> GetVehiclesByOperator(string operatorId)
        {
            return await _context.Vehicles.Where(v => v.OperatorId == operatorId).ToListAsync();
        }
        /*public async Task<IEnumerable<Vehicle>> GetFilteredVehicles(State state, CarType carType, CarName carName, CarModel carModel)
        {
            return await _context.Vehicles
                .Where(c => c.State == state && c.CarName == carName && c.Cartype == carType && c.CarModel == carModel)
                .ToListAsync();
        }*/
    }
}