namespace InternalRazorPagesUi.Model.Repositories;

public interface IItemRepository
{
    IList<Item> GetAll();
    void DeleteAll(IList<Item> items);
    void InsertAll(IList<Item> items);
    void UpdateAll(IList<Item> items);
    void RestoreAll(IList<Item> items);

    Item GetBy(int id);
    void Delete(Item item);
    void Update(Item item);
    void Insert(Item item);
    void Restore(Item item);
}

public class ItemRepository : IItemRepository
{
    List<Item> items = new()
    {
        new() {Id = 1, Name = "Oli Bob", Progress = 20, Location = "United Kingdom", Gender = "male", Rating = 1, Color = "red", DateOfBirth = "1990-01-01", Driver = true},
        new() {Id = 2, Name = "Mary May", Location = "Germany",  Gender = "female", Rating = 2, Color = "blue", DateOfBirth = "1986-11-05"},
        new() {Id = 3, Name = "Christine Lobowski", Location = "France",  Gender = "female", Rating = 0, Color = "green", DateOfBirth = "1974-08-10", isDelete = true},
        new() {Id = 4, Name = "Brendon Philips", Location = "USA", Gender = "male", Rating = 1, Color = "orange", DateOfBirth = "2001-02-21"},
        new() {Id = 5, Name = "Margret Marmajuke", Progress = 100, Location = "Canada", Gender = "female", Rating = 5, Color = "yellow", DateOfBirth = "1982-12-06", Driver = true},
        new() {Id = 6, Name = "Frank Harbours", Location = "Russia", Gender = "male", Rating = 4, Color = "red", DateOfBirth = "1995-10-14"},
        new() {Id = 7, Name = "Jamie Newhart", Location = "India", Gender = "male",  Rating = 3, Color = "green", DateOfBirth = "1987-04-10", isDelete = true},
        new() {Id = 8, Name = "Gemma Jane", Location = "China", Gender = "female",  Rating = 0, Color = "red", DateOfBirth = "2010-11-15", Driver = true, isDelete = true},
        new() {Id = 9, Name = "Emily Sykes", Progress = 45, Location = "South Korea", Gender = "female",  Rating = 1, Color = "maroon", DateOfBirth = "1993-10-11"},
        new() {Id = 10, Name = "James Newman", Location = "Japan", Gender = "male",  Rating = 5, Color = "red", DateOfBirth = "2005-04-20", Driver = true}
    };
 
    public IList<Item> GetAll()
    {
        return items;
    }

    public void DeleteAll(IList<Item> items)
    {
        // Place holder
    }

    public void InsertAll(IList<Item> items)
    {
        // Place holder
    }

    public void UpdateAll(IList<Item> items)
    {
        // Place holder
    }

    public void RestoreAll(IList<Item> items)
    {
        // Place holder
    }

    public Item GetBy(int id)
    {
        return items.FirstOrDefault(x => x.Id == id)!;
    }

    public void Delete(Item item)
    {
        DeleteAll(new List<Item> {item});
    }

    public void Insert(Item item)
    {
        InsertAll(new List<Item> {item});
    }

    public void Restore(Item item)                        
    {
        RestoreAll(new List<Item> {item});
    }                                                     

    public void Update(Item item)
    {
        UpdateAll(new List<Item> {item});
    }
}