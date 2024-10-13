using Microsoft.AspNetCore.Mvc;

namespace webapitest.Controllers;

[ApiController]
[Route("[controller]")]
// class name determines route
public class TestDataController(ILogger<TestDataController> logger) : ControllerBase
{
    private static readonly string[] Summaries =
    [
        "Make an app", "Make a server", "app"
    ];

    private readonly ILogger<TestDataController> _logger = logger;

    [HttpGet(Name = "GetTestData")]
    public IEnumerable<TestData> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new TestData
        {
            ID = index,
            Description = Summaries[Random.Shared.Next(Summaries.Length)],
            Date = DateTime.Now.AddDays(index),
            Completed = false,
        })
        .ToArray();
    }
}
