namespace WakfuScrapper.Domain.Models;

public class Armor : Entity
{
    public List<Stats> Characteristics { get; set; } = new();
    // Recetas...

    public override string ToString()
    {
        var characteristicsString = string.Join(", ", Characteristics.Select(c => $"{c.Name}: {c.ImageClass}"));
        return $"Title: {Title}\nImgSrc: {ImgSrc}\nTypeImage: {TypeImage}\nTypeName: {TypeName}\n" +
               $"Level: {Level}\nDescription: {Description}\nRarityClass: {RarityClass}\n" +
               $"RarityName: {RarityName}\nCharacteristics: {characteristicsString}";
    }
}