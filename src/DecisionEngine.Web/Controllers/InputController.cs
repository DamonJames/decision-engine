using Microsoft.AspNetCore.Mvc;
using DecisionEngine.Services.Interfaces;
using System.Threading.Tasks;
using System;
using DecisionEngine.Web.Models;
using DecisionEngine.Models;

namespace DecisionEngine.Web.Controllers
{
    public class InputController : Controller
    {
        private readonly IUserService _userService;

        public InputController(IUserService userService)
        {
            _userService = userService;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> SubmitAsync(InputViewModel model)
        {
            var response = new ResponseViewModel();

            if (!ModelState.IsValid)
            {
                response.Status = (int)Status.UserError;

                return Json(response);
            }

            try
            {
                var result = await _userService.SubmitAsync(ParseToUserDataModel(model));

                response.Status = (int)result;

                return Json(response);
            }
            catch
            {
                response.Status = (int)Status.Errored;

                return Json(response);
            }
        }

        private User ParseToUserDataModel(InputViewModel model)
        {
            return new User
            {
                Id = Guid.NewGuid(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth,
                Result = (int)Status.Processing
            };
        }
    }
}
