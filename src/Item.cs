namespace MockApi;

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; } = "Placeholder Name";
    public string Description { get; set; } = "Placeholder Description";

    public Item()
    {
        Id = Random.Shared.Next(1, 100);
    }

    public Item(int id)
    {
        Id = id;
    }

    public Item(int id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
}
