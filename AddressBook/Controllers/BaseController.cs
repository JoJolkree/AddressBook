using Microsoft.AspNetCore.Mvc;

namespace AddressBook.Controllers
{
    public abstract class BaseController<TRepository> : Controller
    {
        protected TRepository Repository;

        protected BaseController(TRepository repository)
        {
            Repository = repository;
        }
    }
}