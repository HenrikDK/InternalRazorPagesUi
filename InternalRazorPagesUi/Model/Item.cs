namespace InternalRazorPagesUi.Model;

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Location { get; set; } = "";
    public string Gender { get; set; } = "";
    public int Rating { get; set; }
    public string Color { get; set; } = "";
    public int Progress { get; set; }
    public string DateOfBirth { get; set; } = "";
    public bool Driver { get; set; }
}
