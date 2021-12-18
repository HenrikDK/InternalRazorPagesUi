using InternalRazorPagesUi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InternalRazorPagesUi.Pages;

public class MultiModel : PageModel
{
    private readonly ILogger<MultiModel> _logger;

    public MultiModel(ILogger<MultiModel> logger)
    {
        _logger = logger;
    }

    public JsonResult OnGetPirates()
    {
        var items = new List<Item>
            {
                new() {Id = 1, Name = "Oli Bob", Progress = 20, Location = "United Kingdom", Gender = "male", Rating = 1, Color = "red", DateOfBirth = "1990-01-01", Driver = true},
                new() {Id = 2, Name = "Mary May", Location = "Germany",  Gender = "female", Rating = 2, Color = "blue", DateOfBirth = "1986-11-05"},
                new() {Id = 3, Name = "Christine Lobowski", Location = "France",  Gender = "female", Rating = 0, Color = "green", DateOfBirth = "1974-08-10"},
                new() {Id = 4, Name = "Brendon Philips", Location = "USA", Gender = "male", Rating = 1, Color = "orange", DateOfBirth = "2001-02-21"},
                new() {Id = 5, Name = "Margret Marmajuke", Progress = 100, Location = "Canada", Gender = "female", Rating = 5, Color = "yellow", DateOfBirth = "1982-12-06", Driver = true},
                new() {Id = 6, Name = "Frank Harbours", Location = "Russia", Gender = "male", Rating = 4, Color = "red", DateOfBirth = "1995-10-14"},
                new() {Id = 7, Name = "Jamie Newhart", Location = "India", Gender = "male",  Rating = 3, Color = "green", DateOfBirth = "1987-04-10"},
                new() {Id = 8, Name = "Gemma Jane", Location = "China", Gender = "female",  Rating = 0, Color = "red", DateOfBirth = "2010-11-15", Driver = true},
                new() {Id = 9, Name = "Emily Sykes", Progress = 45, Location = "South Korea", Gender = "female",  Rating = 1, Color = "maroon", DateOfBirth = "1993-10-11"},
                new() {Id = 10, Name = "James Newman", Location = "Japan", Gender = "male",  Rating = 5, Color = "red", DateOfBirth = "2005-04-20", Driver = true}
            };

        return new JsonResult(items);
    }
    
    public JsonResult OnGetLobsters()
    {
        var items = new List<Item>
        {
            new() {Id = 1, Name = "Oli Bob", Progress = 20, Location = "United Kingdom", Gender = "male", Rating = 1, Color = "red", DateOfBirth = "1990-01-01", Driver = true},
            new() {Id = 2, Name = "Mary May", Location = "Germany",  Gender = "female", Rating = 2, Color = "blue", DateOfBirth = "1986-11-05"},
            new() {Id = 3, Name = "Christine Lobowski", Location = "France",  Gender = "female", Rating = 0, Color = "green", DateOfBirth = "1974-08-10"},
            new() {Id = 4, Name = "Brendon Philips", Location = "USA", Gender = "male", Rating = 1, Color = "orange", DateOfBirth = "2001-02-21"},
            new() {Id = 5, Name = "Margret Marmajuke", Progress = 100, Location = "Canada", Gender = "female", Rating = 5, Color = "yellow", DateOfBirth = "1982-12-06", Driver = true},
            new() {Id = 6, Name = "Frank Harbours", Location = "Russia", Gender = "male", Rating = 4, Color = "red", DateOfBirth = "1995-10-14"},
            new() {Id = 7, Name = "Jamie Newhart", Location = "India", Gender = "male",  Rating = 3, Color = "green", DateOfBirth = "1987-04-10"},
            new() {Id = 8, Name = "Gemma Jane", Location = "China", Gender = "female",  Rating = 0, Color = "red", DateOfBirth = "2010-11-15", Driver = true},
            new() {Id = 9, Name = "Emily Sykes", Progress = 45, Location = "South Korea", Gender = "female",  Rating = 1, Color = "maroon", DateOfBirth = "1993-10-11"},
            new() {Id = 10, Name = "James Newman", Location = "Japan", Gender = "male",  Rating = 5, Color = "red", DateOfBirth = "2005-04-20", Driver = true}
        };

        return new JsonResult(items);
    }
    
    public void OnGet()
    {

    }
}
