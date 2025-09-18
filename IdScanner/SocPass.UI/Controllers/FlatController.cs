using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SocPass.Domain.Model;
using SocPass.UI.Application.Interface;
using SocPass.UI.Domain.Model;

namespace SocPass.UI.Controllers
{
    public class FlatController : Controller
    {
        private readonly IFlatService _flatRepository;
        private readonly ISocietyService _societyService;
        private GlobalClass _globalClass;
        public FlatController(IFlatService flatRepository, GlobalClass globalClass, ISocietyService societyService)
        {
            _flatRepository = flatRepository
                ?? throw new ArgumentNullException(nameof(flatRepository));

            _societyService = societyService
                ?? throw new ArgumentNullException(nameof(societyService));
            _globalClass = globalClass;
        }

        public async Task<IActionResult> FlatList()
        {
            //if (string.IsNullOrEmpty(_globalClass.Token))
            //{
            //    return RedirectToAction("Login", "Login");
            //}
            IList<Flat> FlatList = await _flatRepository.GetAllFlatAsync();
            ViewBag.FlatList = FlatList;
            return View("~/Views/Flat/FlatList.cshtml");
        }
        [HttpGet]
        public async Task<IActionResult> AddFlat(int? societyId, int blockId)
        {
            //if (string.IsNullOrEmpty(_globalClass.Token))
            //{
            //    return RedirectToAction("Login", "Login");
            //}

            if (societyId == null || blockId == 0)
            {
                return View(new Flat());
            }
            var flat = await _flatRepository.GetFlatByIdAsync(societyId, blockId);
            return View(flat);
        }
        [HttpPost]
        public async Task<IActionResult> AddFlat(Flat flat)
        {
            //if (string.IsNullOrEmpty(_globalClass.Token))
            //{
            //    return RedirectToAction("Login", "Login");
            //}

            if (flat.SocietyId == 0 || flat.BlockId == 0)
            {
                await _flatRepository.AddFlatAsync(flat);
            }
            else
            {
                await _flatRepository.UpdateFlatAsync(flat);
            }
            return RedirectToAction("FlatList");
        }

    }
}
