using IdScanner.Domain.Model;
using IdScanner.UI.Application.Interface;
using IdScanner.UI.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace IdScanner.UI.Controllers
{
    public class UserDataController : Controller
    {
        private readonly IUserDataService _userDataService;
        public UserDataController(IUserDataService userDataService)
        {
            _userDataService = userDataService;
        }
        public async Task<IActionResult> UserDataList()
        {
            IList<UserData> UserDataList = await _userDataService.GetAllUserDetailsAsync();
            ViewBag.UserDataList = UserDataList;
            return View("~/Views/UserData/UserDataList.cshtml");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUserData(int id)
        {
            try
            {
                await _userDataService.DeleteUserAsync(id);
                return RedirectToAction("UserDataList");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"User with ID {id} not found: {ex.Message}";
                return View("Error");
            }
        }
    }
}
