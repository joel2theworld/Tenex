using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TenexCars.DataAccess.Repositories.Interfaces;
using TenexCars.Models.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TenexCars.DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace TenexCars.Controllers.SetNewPassword_Controller
{
	public class AccountController : Controller
	{
		private readonly ILogger<AccountController> _logger;
		private readonly IAccountRepository _accountRepository;

		public AccountController(ILogger<AccountController> logger, IAccountRepository accountRepository)
		{
			_logger = logger;
			_accountRepository = accountRepository;
		}

		[HttpGet]
		public IActionResult SetNewPassword(string userId, string token)
		{
			var model = new SetNewPasswordViewModel
			{
				UserId = userId,
				Token = token
			};
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> SetNewPassword(SetNewPasswordViewModel model)
		{
			_logger.LogInformation("Attempting to set initial password for user: {UserId}", model.UserId);

			if (!ModelState.IsValid)
			{
				_logger.LogWarning("Model state is invalid for user: {UserId}", model.UserId);
				return View(model);
			}

			var result = await _accountRepository.SetInitialPasswordAsync(model.UserId, model.NewPassword);

			if (result)
			{
				_logger.LogInformation("Initial password set succeeded for user: {UserId}", model.UserId);
				return RedirectToAction("Login", "Account");
			}
			else
			{
				_logger.LogError("Failed to set initial password for user: {UserId}", model.UserId);
				ModelState.AddModelError("", "Failed to set new password or password is already set.");
			}

			return View(model);
		}
	}
}
