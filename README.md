# MockApi

A mock api server for testing and prototyping, it returns an item DTO that can be replaced by any type of model.

Its as easy as replacing the item DTO

```csharp
public class Item
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
```
