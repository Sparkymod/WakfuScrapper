using System.Text.RegularExpressions;
using HtmlAgilityPack;
using WakfuScrapper.Api.Attributes;
using WakfuScrapper.Domain.Models;

namespace WakfuScrapper.Api.Features.ArmorFeature;

[ServiceAvailable(Type = ServiceType.Scoped)]
public class ArmorScrapperService
{
    private readonly HttpClient _httpClient;

    public ArmorScrapperService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Armor> GetEquipmentDetailsAsync(string url)
    {
        var html = await _httpClient.GetStringAsync(url);

        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);

        var titleNode = htmlDocument.DocumentNode.SelectSingleNode("//h1[@class='ak-return-link']");
        var title = titleNode != null ? titleNode.InnerText.Trim() : string.Empty;

        var imageNode = htmlDocument.DocumentNode.SelectSingleNode("(//div[@class='ak-encyclo-detail-illu'])[1]/img");
        var imageUrl = imageNode != null ? imageNode.GetAttributeValue("src", string.Empty).Trim() : string.Empty;

        var typeNode = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='ak-encyclo-detail-type col-xs-6']");
        var typeImage = typeNode.SelectSingleNode(".//span/img").GetAttributeValue("src", "").Trim();
        var typeName = typeNode.SelectSingleNode(".//span").InnerText.Trim();

        var levelNode = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='ak-encyclo-detail-level col-xs-6 text-right']");
        var levelText = levelNode.InnerText.Trim();
        var levelMatch = Regex.Match(levelText, @"\d+");
        var level = levelMatch.Success ? levelMatch.Value : string.Empty;

        var descriptionNode = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='ak-encyclo-detail-right ak-nocontentpadding']//div[@class='ak-container ak-panel'][1]//div[@class='ak-panel-content']");
        var description = descriptionNode.InnerText.Trim() ?? "";

        var rarityNode = htmlDocument.DocumentNode.SelectSingleNode("//div[contains(@class, 'ak-object-rarity')]");
        var raritySpan = rarityNode.SelectSingleNode(".//span[contains(@class, 'ak-rarity')]");

        var rarityClass = raritySpan.GetAttributeValue("class", "").Split(' ').FirstOrDefault(c => c.StartsWith("ak-rarity-"));
        var rarityName = rarityNode.InnerText.Trim();

        var equipment = new Armor
        {
            Title = title,
            ImgSrc = imageUrl,
            TypeImage = typeImage,
            TypeName = typeName,
            Level = level,
            Description = description,
            RarityClass = rarityClass,
            RarityName = rarityName,
            Characteristics = new List<Stats>()
        };

        var characteristicsNodes = htmlDocument.DocumentNode.SelectNodes("//div[@class='ak-container ak-content-list ak-displaymode-col']/div[@class='ak-list-element']");
        foreach (var characteristicNode in characteristicsNodes)
        {
            var characteristicNameNode = characteristicNode.SelectSingleNode(".//div[@class='ak-title']") ?? null!;
            var characteristicName = characteristicNameNode.InnerText.Trim();

            var characteristicImageSpan = characteristicNode.SelectSingleNode(".//div[@class='ak-aside']/span[contains(@class, 'ak-tags-action')]");

            var characteristicImageClass = characteristicImageSpan is not null ? characteristicImageSpan.GetAttributeValue("class", "") : string.Empty;

            equipment.Characteristics.Add(new Stats
            {
                Name = characteristicName,
                ImageClass = characteristicImageClass
            });
        }

        return equipment;
    }
}