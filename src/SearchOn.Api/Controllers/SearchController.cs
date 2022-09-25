using Microsoft.AspNetCore.Mvc;
using SearchOn.Services.Implementations;

namespace SearchOn.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly SearchService _searchService;

    public SearchController(SearchService searchService)
    {
        _searchService = searchService;
    }

    [HttpGet("{input}")]
    public IActionResult Get(string input)
    {
        var result = _searchService.Search(input);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Post(CancellationToken cancellationToken)
    {
        await _searchService.LoadDefaultDataAsync(cancellationToken);

        return Ok();
    }
}
