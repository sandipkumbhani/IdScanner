using IdScanner.Domain.Model;
using IdScanner.UI.Application.Interface;
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
    }
}
