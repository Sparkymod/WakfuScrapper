namespace WakfuScrapper.Domain.Models;

public class Spell
{
    public string ImgUrl { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string PaText { get; set; } = string.Empty;
    public string PaImg { get; set; } = string.Empty;
    public string CostRangeText { get; set; } = string.Empty;
    public string CostRangeImg { get; set; } = string.Empty;
    public List<string> SightLineImg { get; set; } = new();
    public string Description { get; set; } = string.Empty;
    public List<SpellEffect> NormalEffects { get; set; } = new();
    public List<SpellEffect> CriticalEffects { get; set; } = new();
}