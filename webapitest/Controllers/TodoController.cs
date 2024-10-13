using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace webapitest.Controllers;

public class TaskRequest
{
    public required string Description { get; set; }
}

[ApiController]
[Route("[controller]")]
// class name determines route
public class TodoController(ILogger<TestDataController> logger) : ControllerBase
{
    private static readonly string[] Summaries =
    [
        "Make an app", "Make a server", "app", "make a new todo app", "use a database"
    ];

    private readonly ILogger<TestDataController> _logger = logger;


    [HttpGet("list", Name = "GetTodo")]
    public ActionResult<IEnumerable<object>> GetTasks()
    {
        using var context = new TaskContext();
        context.Database.EnsureCreated();

        var tasks = context.Tasks.ToList();
        if (tasks.Count == 0)
        {
            _logger.LogWarning("No tasks found on request");
            return NoContent();
        }
        var formattedTasks = tasks;
        _logger.LogInformation($"Returning {formattedTasks.Count} tasks");
        return Ok(formattedTasks);
    }

    [HttpPost("add", Name = "PostTodo")]
    public ActionResult<TaskItem> PostTask(TaskRequest newTask)
    {
        Console.WriteLine("this is a test");
        _logger.LogInformation("Post activated");

        using var context = new TaskContext();
        context.Database.EnsureCreated();
        var tasks = context.Tasks.ToList();

        var task = new TaskItem
        {
            ID = tasks.Count != 0 ? tasks.Last().ID + 1 : 1, // if no tasks, then use 1 as ID
            Description = newTask.Description,
            Date = DateTime.Now,
            Completed = false
        };
        context.Tasks.Add(task);
        context.SaveChanges();
        _logger.LogInformation($"Task {task.ID} added successfully: '{task.Description}'");
        return CreatedAtAction(nameof(GetTasks), new { id = task.ID }, task);
    }

    [HttpOptions("help", Name = "OptionsTodo")]
    public ActionResult<string> OptionTask(string? input)
    {
        if (input == null || input.Length == 0)
        {
            return Ok("To use this API, you can:\n" +
                   "1. GET /todo to retrieve all tasks.\n" +
                   "2. POST /todo/add with a JSON body containing 'Description' to add a new task.\n" +
                   "3. OPTIONS /todo/help to view this help message.\n" +
                   "Please refer to the API documentation for more information.");
        }
        return input switch
        {
            "list" => Ok("To use the GetTask method, simply send a GET request to /todo. This will return a list of all tasks. You can also specify an ID in the URL to retrieve a specific task."),
            "add" => Ok("To use the AddTask method, send a POST request to /todo/add with a JSON body containing 'Description' to add a new task. This will add a new task to the saved tasks."),
            "options" => Ok("To use the AddTask method, send a POST request to /todo/add with a JSON body containing 'Description' to add a new task. This will add a new task to the saved tasks."),
            _ => Ok("To use this API, you can:\n" +
                               "1. GET /todo to retrieve all tasks.\n" +
                               "2. POST /todo/add with a JSON body containing 'Description' to add a new task.\n" +
                               "3. OPTIONS /todo/help to view this help message.\n" +
                               "Please refer to the API documentation for more information."),
        };
    }
}
