using HemoTrack.Models;
using HemoTrack.Services;
using Microsoft.AspNetCore.Mvc;

namespace HemoTrack.Controllers;


public class BlogController : Controller
{
    private readonly BlogsService _blogsService;

    public BlogController(BlogsService blogsService) => 
    _blogsService = blogsService;

    [HttpGet]
    public async Task<List<Blog>> Get() => 
        await _blogsService.GetAsync();

    
    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Blog>> Get(string id)
    {
        var blog = await _blogsService.GetAsync(id);

        if(blog is null)
        {
            return NotFound();
        }
        return blog;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Blog newBlog)
    {
        await _blogsService.CreateAsync(newBlog);

        return CreatedAtAction(nameof(Get), new {id = newBlog.Id}, newBlog);
    }

    [HttpPost]
    public async Task<IActionResult> Update(string id, Blog updatedBlog)
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

    public async Task<IActionResult> Delete(string id)
    {
        var blog = await _blogsService.GetAsync(id);

        if (blog is null)
        {
            return NotFound();
        }

        await _blogsService.RemoveAsync(id);
        return NoContent();
    }
}