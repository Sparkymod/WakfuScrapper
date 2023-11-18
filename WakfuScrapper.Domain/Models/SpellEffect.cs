namespace WakfuScrapper.Domain.Models;

public class SpellEffect
{
    public string Description { get; set; } = string.Empty;
    public List<string> ImageUrls { get; set; } = new();
}