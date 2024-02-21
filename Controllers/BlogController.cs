using HemoTrack.Models;
using HemoTrack.Services;
using Microsoft.AspNetCore.Mvc;
using HemoTrack.ViewModels;

namespace HemoTrack.Controllers;


public class BlogController : Controller
{
    private readonly BlogsService _blogsService;

    public BlogController(BlogsService blogsService) => 
    _blogsService = blogsService;

    public async Task<List<Blog>> Get() => 
        await _blogsService.GetAsync();

    
    [HttpGet]
    public async Task<Blog> Get(string id)
    {
        var blog = await _blogsService.GetAsync(id);
        return blog;
    }

    [HttpGet]
    public async Task<IActionResult> EditPost(string id)
    {
        var blog = await _blogsService.GetAsync(id);
        return View(blog);
    }


    [HttpPost]
    public async Task<IActionResult> EditPost(string id, Blog updatedBlog)
    {
        var blog = await _blogsService.GetAsync(id);

        if (blog is null)
        {
            return NotFound();
        }
        updatedBlog.Id = blog.Id;

        await _blogsService.UpdateAsync(id, updatedBlog);

        return NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        var blog = await _blogsService.GetAsync(id);

        if (blog is null)
        {
            return NotFound();
        }

        await _blogsService.RemoveAsync(id);
        return RedirectToAction("Index", "Blog");
    }


    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var blogs = await Get();
        return View(blogs);
    }

    [HttpGet]
    public async Task<IActionResult> ViewContent(string id)
    {
        var blog = await Get(id);
        return View(blog);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Blog newBlog)
    {
        if (ModelState.IsValid)
        {
            await _blogsService.CreateAsync(newBlog);
            return RedirectToAction(nameof(Index));
        }
        return View(newBlog);
    }

    [HttpPost]
    public async Task<IActionResult> Post(Blog newBlog)
    {
        await _blogsService.CreateAsync(newBlog);

        return CreatedAtAction(nameof(Get), new {id = newBlog.Id}, newBlog);
    }


}