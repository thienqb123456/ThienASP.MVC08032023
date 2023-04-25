using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThienASPMVC08032023.Models;
using ThienASPMVC08032023.Repository.InterfaceRepo;

namespace ThienASPMVC08032023.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("Admin")]
    [Route("/{Controller}/{Action=Index}/{id?}")]
    public class CategoryController : Controller
    {
        private readonly IRepositoryWrapper _repo;
        public CategoryController(IRepositoryWrapper repo)
        {
            _repo = repo;
        }


        // GET: CategoryController
        public async Task<ActionResult> Index()
        {
            var categories = await _repo.CategoryRepo.GetAllCategoriesAsync();
            return View(categories);
        }

        // GET: CategoryController/Details/5
        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            var category = await _repo.CategoryRepo.GetCategoryByIdAsync(id);
            if (category == null) { return NotFound($"Not found category has id = {id}"); }
            return View(category);
        }

        // GET: CategoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id, Name, Description")] Category categoryVM)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            _repo.CategoryRepo.CreateCategory(categoryVM);
            await _repo.SaveAsync();
            return RedirectToAction("Index");
        }

        // GET: CategoryController/Edit/5
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var category = await _repo.CategoryRepo.GetCategoryByIdAsync(id);
            if (category == null) { return NotFound($"Not found category has id = {id}"); }
            return View(category);
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, [Bind("Id, Name, Description")] Category categoryVM)
        {
            if (id != categoryVM.Id) { return NotFound($"wrong id, {id} != {categoryVM.Id}"); }
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            _repo.CategoryRepo.UpdateCategory(categoryVM);
            await _repo.SaveAsync();
            return RedirectToAction("Index");
        }

        //GET: CategoryController/Delete/5
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var category = await _repo.CategoryRepo.GetCategoryByIdAsync(id);
            if (category == null) { return NotFound($"Not found category has id = {id}"); }
            return View(category);
        }

        // POST: CategoryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirm(int id)
        {
            var category = await _repo.CategoryRepo.GetCategoryByIdAsync(id);
            _repo.CategoryRepo.DeleteCategory(category);
            await _repo.SaveAsync();
            return RedirectToAction("Index");
        
        }
    }
}
