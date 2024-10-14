using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TenexCars.DataAccess.DTOs;
using TenexCars.DataAccess.Enums;
using TenexCars.DataAccess.Models;

using TenexCars.DataAccess.Repositories.Interfaces;
using TenexCars.DataAccess.ViewModels;
using TenexCars.Interfaces;
using TenexCars.Models.ViewModels;

namespace TenexCars.Controllers.Operator_Controller
{
    //[Authorize(Roles = "Main_Operator")]
    public class OperatorController : Controller
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly ISubscriptionRepository _subscription;
        private readonly ISubscriberRepository _subscriberRepo;
        private readonly IOperatorRepository _operatorRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IPhotoService _photoService;
        private readonly ILogger<OperatorController> _logger;
        public OperatorController(IVehicleRepository vehicleRepository, ISubscriptionRepository subscription,
            IOperatorRepository operatorRepository, UserManager<AppUser> userManager, IEmailService emailService,
            ILogger<OperatorController> logger, ISubscriberRepository subscriberRepo, IPhotoService photoService)
        {
            _vehicleRepository = vehicleRepository;
            _subscription = subscription;
            _operatorRepository = operatorRepository;
            _userManager = userManager;
            _emailService = emailService;
            _logger = logger;
            _subscriberRepo = subscriberRepo;
            _photoService = photoService;
        }

        [HttpGet]
        public async Task<IActionResult> Home(QueryObject query)
        {
            var vehicles = await _vehicleRepository.GetAll(query);

            PopulateViewBags(query);

            return View(vehicles);
        }

        [HttpPost]
        public async Task<IActionResult> Filter(QueryObject query)
        {
            var vehicles = await _vehicleRepository.GetAll(query);

            PopulateViewBags(query);

            return View("Home", vehicles);
        }


        [HttpGet]
        [Route("/Operator/OperatorVehicles/{operatorId}")]
        public async Task<IActionResult> OperatorVehicles(string operatorId)
        {
            var vehicle = await _vehicleRepository.GetVehiclesByOperator(operatorId);
            ViewBag.Vehicle = vehicle;
            return View(vehicle);
        }

        private void PopulateViewBags(QueryObject query = null)
        {
            ViewBag.State = Enum.GetValues(typeof(State)).Cast<State>();
            ViewBag.CarModel = Enum.GetValues(typeof(CarModel)).Cast<CarModel>();
            ViewBag.CarName = Enum.GetValues(typeof(CarName)).Cast<CarName>();
            ViewBag.CarType = Enum.GetValues(typeof(CarType)).Cast<CarType>();

            if (query != null)
            {
                ViewBag.SelectedState = query.State;
                ViewBag.SelectedCarModel = query.CarModel;
                ViewBag.SelectedCarName = query.CarName;
                ViewBag.SelectedCarType = query.CarType;
            }
        }
        public IActionResult Operator()
        {
            return View();
        }

        // GET: Vehicle/Create
        [HttpGet]
        public async Task<IActionResult> CreateVehicle()
        {
            var user = await _userManager.GetUserAsync(User);

            var operatorUser = user is not null ? await _operatorRepository.GetOperatorByUserId(user.Id) : null;

            var viewModel = new CreateVehicleViewModel
            {
                OperatorId = operatorUser is not null ? operatorUser.Id : null,
                OperatorLogoUrl = operatorUser is not null ? operatorUser.CompanyLogo : null,
                CarDealerName = operatorUser is not null ? operatorUser.BusinessName: null

            };

            return View(viewModel);
        }

        // POST: Vehicle/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVehicle(CreateVehicleViewModel vehicleViewModel)
        {
            _logger.LogInformation($"Creating Vehicle ...");
            if (!ModelState.IsValid)
            {
                return View(vehicleViewModel);
            }
            try
            {
                var user = await _userManager.GetUserAsync(User);

                var operatorUser = user is not null ? await _operatorRepository.GetOperatorByUserId(user.Id) : null;

                var vehicleImage = await _photoService.AddPhotoAsync(vehicleViewModel.ImageUrl);

                var newVehicle = new Vehicle
                {
                    Operator = operatorUser,
                    OperatorId = operatorUser.Id,
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
                   
                    CarDealerName = operatorUser.BusinessName,
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
                    ImageUrl = vehicleImage.Url.ToString()


                };

                await _vehicleRepository.AddVehicleAsync(newVehicle);

                return RedirectToAction("OperatorInventoryPage");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, "Something went wrong");
                return View(vehicleViewModel);

            }
           
        }

        [HttpGet]
        public async Task<IActionResult> CarDetails(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var vehicle = await _vehicleRepository.GetVehicleById(id);

            if (vehicle == null)
            {
                return NotFound();
            }
            var carDetailsViewModel = new CarDetailsViewModel
            {
                Id = vehicle.Id,
                Make = vehicle.Make,
                Model = vehicle.Model,
                MarketValue = vehicle.MarketValue.ToString(),
                ImageUrl = vehicle.ImageUrl,
                Color = vehicle.Color,
                SeatNumbers = vehicle.SeatNumbers,
                TrunkSize = vehicle.TrunkSize,
                DcFastCharging = vehicle.DcFastCharging,
                HomeCharger = vehicle.HomeCharger,
                RangeOfFullCharge = vehicle.RangeOfFullCharge,
                CarDescription = vehicle.CarDescription,
                TouchScreenDisplay = vehicle.TouchScreenDisplay,
                WheelDrive = vehicle.WheelDrive,
                DriverLumbarSupport = vehicle.DriverLumbarSupport,
                SeatHeater = vehicle.SeatHeater,
                Radio = vehicle.Radio,
                AndroidAuto_AppleCarPlay = vehicle.AndroidAuto_AppleCarPlay,
                NumberOfSpeakers = vehicle.NumberOfSpeakers,
                TouchScreenNavSystem = vehicle.TouchScreenNavSystem,
                BluetoothSystem = vehicle.BluetoothSystem,
                ProjectorBeamLedHeadlight = vehicle.ProjectorBeamLedHeadlight,
                RemoteKeylessEntry = vehicle.RemoteKeylessEntry,
                StandardLowTirePressureWarning = vehicle.StandardLowTirePressureWarning,
                Torque = vehicle.Torque,
                Horsepower = vehicle.Horsepower,
                EngineType = vehicle.EngineType,
                DiscBrakes = vehicle.DiscBrakes,
                StabilityControl = vehicle.StabilityControl,
                TransmissionSpeed = vehicle.TransmissionSpeed,
                TurningDiameter = vehicle.TurningDiameter,
                CurbWeight = vehicle.CurbWeight,
                TransmissionType = vehicle.TransmissionType,
                DriveAxleRatio = vehicle.DriveAxleRatio,
                CarWarrantyOverview = vehicle.CarWarrantyOverview
            };
            return View(carDetailsViewModel);

        }

        public async Task<IActionResult> Vehicle(QueryObject query)
        {
            var vehicles = await _vehicleRepository.GetAll(query);
            return View(vehicles);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _vehicleRepository.GetVehicleById(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            var vehicleViewModel = new VehicleViewModel
            {
                Id = vehicle.Id,
                Cartype = vehicle.Cartype,
                CarName = vehicle.Make,
                CarModel = vehicle.Model,
                ChasisNumber = vehicle.ChasisNumber,
                SeatNumbers = vehicle.SeatNumbers,
                Mileage = vehicle.Mileage,
                TrunkSize = vehicle.TrunkSize,
                InsuranceStartDate = vehicle.InsuranceStartDate,
                InsuranceEndDate = vehicle.InsuranceEndDate,
                Color = vehicle.Color,
                VIN = vehicle.VIN,
                PickupAddress = vehicle.PickupAddress,
                State = vehicle.State,
                City = vehicle.City,
                ZIP = vehicle.ZIP,
                CarDealerName = vehicle.CarDealerName,
                FinacialStartDate = vehicle.FinacialStartDate,
                FinacialEndDate = vehicle.FinacialEndDate,
                Torque = vehicle.Torque,
                TransmissionType = vehicle.TransmissionType,
                Horsepower = vehicle.Horsepower,
                TurningDiameter = vehicle.TurningDiameter,
                CurbWeight = vehicle.CurbWeight,
                DiscBrakes = vehicle.DiscBrakes,
                TransmissionSpeed = vehicle.TransmissionSpeed,
                WheelDrive = vehicle.WheelDrive,
                CargoSpace = vehicle.CargoSpace,
                DcFastCharging = vehicle.DcFastCharging,
                HomeCharger = vehicle.HomeCharger,
                SeatHeater = vehicle.SeatHeater,
                NumberOfSpeakers = vehicle.NumberOfSpeakers,
                RangeOfFullCharge = vehicle.RangeOfFullCharge,
                Radio = vehicle.Radio ?? false,
                DriverLumbarSupport = vehicle.DriverLumbarSupport ?? false,
                TouchScreenDisplay = vehicle.TouchScreenDisplay ?? false,
                RemoteKeylessEntry = vehicle.RemoteKeylessEntry ?? false,
                StandardLowTirePressureWarning = vehicle.StandardLowTirePressureWarning ?? false,
                BluetoothSystem = vehicle.BluetoothSystem ?? false,
                CarPictures = vehicle.ImageUrl
            };

            return View(vehicleViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VehicleViewModel vehicleViewModel)
        {
            if (vehicleViewModel == null || string.IsNullOrEmpty(vehicleViewModel.Id))
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var vehicle = await _vehicleRepository.GetVehicleById(vehicleViewModel.Id);
                    if (vehicle == null)
                    {
                        return NotFound();
                    }

                    // Update the existing vehicle entity with details from the viewmodel
                    vehicle.Cartype = vehicleViewModel.Cartype;
                    vehicle.Make = vehicleViewModel.CarName;
                    vehicle.Model = vehicleViewModel.CarModel;
                    vehicle.ChasisNumber = vehicleViewModel.ChasisNumber;
                    vehicle.SeatNumbers = vehicleViewModel.SeatNumbers;
                    vehicle.Mileage = vehicleViewModel.Mileage;
                    vehicle.TrunkSize = vehicleViewModel.TrunkSize;
                    vehicle.InsuranceStartDate = vehicleViewModel.InsuranceStartDate;
                    vehicle.InsuranceEndDate = vehicleViewModel.InsuranceEndDate;
                    vehicle.Color = vehicleViewModel.Color;
                    vehicle.VIN = vehicleViewModel.VIN;
                    vehicle.PickupAddress = vehicleViewModel.PickupAddress;
                    vehicle.State = vehicleViewModel.State;
                    vehicle.City = vehicleViewModel.City;
                    vehicle.ZIP = vehicleViewModel.ZIP;
                    vehicle.CarDealerName = vehicleViewModel.CarDealerName;
                    vehicle.FinacialStartDate = vehicleViewModel.FinacialStartDate;
                    vehicle.FinacialEndDate = vehicleViewModel.FinacialEndDate;
                    vehicle.Torque = vehicleViewModel.Torque;
                    vehicle.TransmissionType = vehicleViewModel.TransmissionType;
                    vehicle.Horsepower = vehicleViewModel.Horsepower;
                    vehicle.TurningDiameter = vehicleViewModel.TurningDiameter;
                    vehicle.CurbWeight = vehicleViewModel.CurbWeight;
                    vehicle.DiscBrakes = vehicleViewModel.DiscBrakes;
                    vehicle.TransmissionSpeed = vehicleViewModel.TransmissionSpeed;
                    vehicle.WheelDrive = vehicleViewModel.WheelDrive;
                    vehicle.CargoSpace = vehicleViewModel.CargoSpace;
                    vehicle.DcFastCharging = vehicleViewModel.DcFastCharging;
                    vehicle.HomeCharger = vehicleViewModel.HomeCharger;
                    vehicle.SeatHeater = vehicleViewModel.SeatHeater;
                    vehicle.NumberOfSpeakers = vehicleViewModel.NumberOfSpeakers;
                    vehicle.RangeOfFullCharge = vehicleViewModel.RangeOfFullCharge;
                    vehicle.Radio = vehicleViewModel.Radio;
                    vehicle.DriverLumbarSupport = vehicleViewModel.DriverLumbarSupport;
                    vehicle.TouchScreenDisplay = vehicleViewModel.TouchScreenDisplay;
                    vehicle.RemoteKeylessEntry = vehicleViewModel.RemoteKeylessEntry;
                    vehicle.StandardLowTirePressureWarning = vehicleViewModel.StandardLowTirePressureWarning;
                    vehicle.BluetoothSystem = vehicleViewModel.BluetoothSystem;

                    _vehicleRepository.UpdateVehicle(vehicle);
                }
                catch (DbUpdateException )
                {
                    if (!_vehicleRepository.VehicleExists(vehicleViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
               /* return RedirectToAction(nameof(CarDetails), new { id = vehicle.Id });*/
            }
            return View(vehicleViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ManageOperatorMembers()
        {
            var user = await _userManager.GetUserAsync(User);
            var operatorUser = user is not null ? await _operatorRepository.GetOperatorByUserId(user.Id) : null;
            var model = new OperatorMemberViewModel
            {
                OperatorMembers = operatorUser is not null ? (await _operatorRepository.GetAllMembersForOperatorAsync(operatorUser.Id)).ToList() : null
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddOperatorMembers(OperatorMemberViewModel model)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);
            var operatorUser = loggedInUser is not null ? await _operatorRepository.GetOperatorByUserId(loggedInUser.Id) : null;

            var user = new AppUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Type = model.Role == "Admin" ? "Main_Operator" : "Operator_Team_Member"
            };
            var result = await _userManager.CreateAsync(user);

            if (result.Succeeded)
            {
                if(model.Role == "Admin")
                {
                    await _userManager.AddToRoleAsync(user, "Main_Operator");
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, " Operator_Team_Member");
                }
                var newMember = new OperatorMember
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    AppUserId = user.Id,
                    Role = model.Role,
                    OperatorId = operatorUser.Id,
                    Operator = operatorUser
                };
                await _operatorRepository.AddOperatorMemberAsync(newMember);

                // Generate password reset token
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var encodedToken = Uri.EscapeDataString(token);
                var resetPasswordUrl = Url.Action("SetNewPassword", "Account", new { userId = user.Id, token = encodedToken }, protocol: HttpContext.Request.Scheme);

                var emailBody = $"<div style=\"color: black;\">" +
                                $"Hello {model.FirstName},<br><br>" +
                                $"Welcome to Tenex! We are excited to have you join us as an Operator {model.Role}. " +
                                "To get started, please set your password by clicking the link below:" +
                                "<br><br>" +
                                $"<a href=\"{resetPasswordUrl}\" style=\"color: blue; text-decoration: none;\">Set Password Link</a><br><br>" +
                                "This link will take you to a secure page where you can create your password and complete your account setup. " +
                                "<br><br>" +
                                "Welcome aboard, we look forward to working with you!" +
                                "<br><br>" +
                                "Best regards,<br>" +
                                "The Tenex Team" +
                                "</div>";

                var emailContent = new EmailDto
                {
                    To = user.Email,
                    Subject = "Welcome to Tenex! Set Your Password to Get Started",
                    Body = emailBody
                };
                await _emailService.SendOperatorSetPasswordEmailAsync(emailContent);

                return RedirectToAction("ManageOperatorMembers");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    _logger.LogError($"Error: {error.Code} - {error.Description}");
                }

                ModelState.AddModelError(string.Empty, "Failed to create user. Please check the errors and try again.");
            }
            model.OperatorMembers = (await _operatorRepository.GetAllOperatorMembersAsync()).ToList();
            return View("ManageOperatorMembers", model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOperatorMember(string email)
        {
            await _operatorRepository.DeleteOperatorMemberAsync(email);
            return RedirectToAction("ManageOperatorMembers");
        }

        public async Task<IActionResult> OperatorInventoryPage()
        {
            var loggedInUser = await _userManager.GetUserAsync(User);
            var operatorUser = loggedInUser is not null ? await _operatorRepository.GetOperatorByUserId(loggedInUser.Id) : null;
            var vehicles = operatorUser is not null ? await _vehicleRepository.GetVehiclesByOperator(operatorUser.Id) : null;



            // Fetch all subscriptions asynchronously
            

            // Create a list of view models
            var viewModelList = new List<OperatorInventoryViewModel>();

            foreach (var vehicle in vehicles)
            {
                // Find the subscription related to the current vehicle
                var subscription = await _subscription.GetSubscriptionForVehicle(vehicle.Id);

                var subscriber = subscription is not null ? await _subscriberRepo.GetSubscriberByIdAsync(subscription.SubscriberId) : null;

                var viewModel = new OperatorInventoryViewModel
                {
                    VehicleId = vehicle.Id,
                    VehicleName = $"{vehicle.Make} {vehicle.Model}",
                    Status = subscription is not null ? subscription.SubscriptionStatus: "",
                    TrackerStatus = subscription is not null ? "Active" : "Inactive",
                    SubscriberId = subscriber is not null ? subscriber.Id : null,
                    SubscriberName = subscriber is not null ? $"{subscriber.FirstName} {subscriber.LastName}" : ""

                    
                };

                viewModelList.Add(viewModel);
            }

            // Pass the view model list to the view
            return View(viewModelList);
        }

        [HttpGet]
        public async Task<IActionResult> OperatorDashboard(string operatorId )
        {
            var loggedInUser = await _userManager.GetUserAsync(User);
            var operatorUser = loggedInUser is not null ? await _operatorRepository.GetOperatorByUserId(loggedInUser.Id) : null;

            var totalVehicles = await _operatorRepository.GetTotalNumberOfCars(operatorUser.Id);
            var totalSubscribers = await _operatorRepository.GetTotalNumberOfSubscribers(operatorUser.Id);
            var totalReservedCars = await _operatorRepository.GetTotalNumberOfReservedCars(operatorUser.Id);
            var totalActiveCars = await _operatorRepository.GetTotalNumberOfActiveCars(operatorUser.Id);
            // var operatorEntity = await _operatorRepository.GetOperatorById(operatorId);

            var viewModel = new OperatorDashboardViewModel
            {
                OperatorId = operatorUser.Id,
                TotalNumberOfVehicles = totalVehicles,
                TotalNumberOfSubscribers = totalSubscribers,
                TotalNumberOfReservedCars = totalReservedCars,
                TotalNumberOfActiveCars = totalActiveCars,
                CurrentMonthStats = new List<int> { 500, 700, 1200, 1500, 2000, 2500, 3000, 3500, 4000, 4500 },
                PastMonthStats = new List<int> { 300, 400, 500, 600, 700, 800, 900, 1000, 1100, 1200 }
                // OperatorLogo = operatorEntity.CompanyLogo,
                // OperatorImage = operatorEntity.HeroGraphics

            };
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> OperatorProfileSettings()
        {
            var user = await _userManager.GetUserAsync(User);
            var existingOperator = await _operatorRepository.GetOperatorByUserId(user.Id);

            var operatorDetails = new OperatorProfileSettingsViewModel
            {
                Name = $"{existingOperator.FirstName} {existingOperator.LastName}",
                Email = existingOperator.Email
            };

            return View(operatorDetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(OperatorProfileSettingsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    _logger.LogError("User may not exist");
                    TempData["error"] = "User may not exist";
                    return RedirectToAction("Login", "Account");
                }

                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    await _userManager.UpdateSecurityStampAsync(user);
                    _logger.LogInformation("Password changed successfully!!!");
                    TempData["success"] = "Password changed successfully!!!";
                    return RedirectToAction("OperatorProfileSettings");
                }
            }
            _logger.LogError("Invalid Form entry");
            TempData["error"] = "Invalid Form entry";
            return RedirectToAction("OperatorProfileSettings", model);
        }

    }
}
