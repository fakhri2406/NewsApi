using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsApi.Models;
using NewsApi.Repositories.Abstract;

namespace NewsApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class NewsController : ControllerBase
{
    private readonly INewsRepository _repo;
    
    public NewsController(INewsRepository repo)
    {
        _repo = repo;
    }
    
    #region GET
    
    /// <summary>
    /// Retrieve all news items
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<News>>> GetAllNews()
    {
        var newsItems = await _repo.GetAllAsync();
        return newsItems.Any() ? Ok(newsItems) : NoContent();
    }
    
    /// <summary>
    /// Retrieve one news item by ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<News>> GetNewsById(int id)
    {
        var news = await _repo.GetByIdAsync(id);
        if (news == null)
        {
            return NotFound();
        }
        
        return Ok(news);
    }
    
    #endregion
    
    #region POST
    
    /// <summary>
    /// Create a news item
    /// </summary>
    /// <param name="news"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<News>> PostNews(News news)
    {
        news.PublishedDate = DateTime.UtcNow;
        var createdNews = await _repo.CreateAsync(news);
        return CreatedAtAction(nameof(GetAllNews), new { id = createdNews.Id }, createdNews);
    }
    
    #endregion
    
    #region PUT
    
    /// <summary>
    /// Update a news item
    /// </summary>
    /// <param name="id"></param>
    /// <param name="news"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateNews(int id, News news)
    {
        if (id != news.Id)
        {
            return NotFound();
        }

        await _repo.UpdateAsync(news);
        return NoContent();
    }
    
    #endregion
    
    #region DELETE
    
    /// <summary>
    /// Delete a news item
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteNews(int id)
    {
        var existingNews = await _repo.GetByIdAsync(id);
        if (existingNews == null)
        {
            return NotFound();
        }
        
        await _repo.DeleteAsync(id);
        return NoContent();
    }
    
    #endregion
}
