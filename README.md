# MockApi 

[![.NET](https://github.com/Jaxelr/MockApi/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/Jaxelr/MockApi/actions/workflows/ci.yml)

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

### Why?

Sometimes I need to add custom code related to random latency and degradation scenarios for repros and its easier to start with this template. 
