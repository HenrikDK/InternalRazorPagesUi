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
    
    protected bool Equals(Item other)
    {
        return Id == other.Id && Name == other.Name && Location == other.Location && Gender == other.Gender 
               && Rating == other.Rating && Color == other.Color && Progress == other.Progress 
               && DateOfBirth == other.DateOfBirth && Driver == other.Driver;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Item) obj);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(Id);
        hashCode.Add(Name);
        hashCode.Add(Location);
        hashCode.Add(Gender);
        hashCode.Add(Rating);
        hashCode.Add(Color);
        hashCode.Add(Progress);
        hashCode.Add(DateOfBirth);
        hashCode.Add(Driver);
        return hashCode.ToHashCode();
    }
}
