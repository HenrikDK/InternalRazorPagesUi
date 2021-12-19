using System.ComponentModel.DataAnnotations;

namespace InternalRazorPagesUi.Model;

public class Item
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = "";
    [Required]
    public string Location { get; set; } = "";
    [Required]
    public string Gender { get; set; } = "";
    [Required]
    public int Rating { get; set; }
    [Required]
    public string Color { get; set; } = "";
    
    public int Progress { get; set; }
    [Required]
    public string DateOfBirth { get; set; } = "";
    
    public bool Driver { get; set; }
    
    public bool isDelete { get; set; } = false;
}
