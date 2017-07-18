using System.Linq;
using AddressBookDomain.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AddressBook.Controllers
{
    public class CallsController : BaseController<CallsRepository>
    {
        public CallsController(CallsRepository callsRepo) : base(callsRepo)
        {
        }

        [Authorize]
        public IActionResult Index()
        {
            return View(Repository.GetAllUserCalls().OrderBy(x => x.Created));
        }

        [Authorize]
        public IActionResult Remove(int id)
        {
            Repository.Remove(Repository.GetCallByid(id));
            return RedirectToAction("Index");
        }
    }
}