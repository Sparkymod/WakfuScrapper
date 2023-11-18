using HtmlAgilityPack;
using System.Text.RegularExpressions;
using WakfuScrapper.Api.Attributes;
using WakfuScrapper.Domain.Models;

namespace WakfuScrapper.Api.Features.SpellFeature;

[ServiceAvailable(Type = ServiceType.Scoped)]
public class SpellScrapperService
{
    private readonly HttpClient _httpClient;

    public SpellScrapperService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Spell>> GetSpellDetailsAsync(string url)
    {
        try
        {
            // es enciclopedia clases
            // fr encyclopedie classes
            // en encyclopedia classes
            // pt enciclopedia classes

            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var spellDetails = new List<Spell>();

            // Seleccionar el nodo que contiene los detalles del hechizo por ID
            var spellDetailsNodes = htmlDocument.DocumentNode.SelectNodes("//div[@id='ak-spell-details']/div[contains(@class, 'ak-level-selector-target')]");

            foreach (var spellLevelDetailsNode in spellDetailsNodes)
            {
                var spell = new Spell();

                // Extraer la URL de la imagen del hechizo
                var spellImageNode = spellLevelDetailsNode.SelectSingleNode(".//div[contains(@class, 'ak-spell-details-illu')]/span/img");
                var spellImageUrl = spellImageNode?.GetAttributeValue("src", string.Empty);

                spell.ImgUrl = spellImageUrl ?? "";

                var spellLevelNode = spellLevelDetailsNode.SelectSingleNode(".//span[@class='ak-spell-nb']");
                var spellLevel = spellLevelNode?.InnerText.Trim();

                spell.Level = spellLevel ?? "";

                var spellInfoNode = spellLevelDetailsNode.SelectSingleNode("//div[contains(@class, 'ak-spell-details-infos')]");

                var spellNameNode = spellInfoNode.SelectSingleNode(".//h2[@class='ak-spell-name']");
                var spellName = spellNameNode?.FirstChild.InnerText ?? "";

                spell.Name = spellName;

                var paNode = spellInfoNode.SelectSingleNode(".//span[@class='pa']");
                var paText = paNode?.InnerText.Trim();

                spell.PaText = paText ?? "";

                var paImageNode = paNode.SelectSingleNode(".//span[@class='picto']/img");
                var paImage = paImageNode?.GetAttributeValue("src", string.Empty);

                spell.PaImg = paImage ?? "";

                var costsRangeNode = spellInfoNode.SelectSingleNode(".//span[@class='costs_range']");
                var costsRange = costsRangeNode?.InnerText.Trim();

                spell.CostRangeText = costsRange ?? "";

                var costsRangeImageNode = costsRangeNode?.SelectSingleNode(".//span[@class='picto']/img");
                var costsRangeImage = costsRangeImageNode?.GetAttributeValue("src", string.Empty);

                spell.CostRangeImg = costsRangeImage ?? "";

                var sightLineNode = spellInfoNode.SelectSingleNode(".//span[@class='sight_line']");
                var sightLineImages = sightLineNode.SelectNodes(".//span[@class='picto']/img")
                    .Select(imgNode => imgNode.GetAttributeValue("src", string.Empty))
                    .ToList();

                spell.SightLineImg = sightLineImages;

                var descriptionNode = spellLevelDetailsNode.SelectSingleNode("//div/span[@class='ak-spell-description']");
                var description = descriptionNode != null ? descriptionNode.InnerText.Trim() : string.Empty;

                spell.Description = description;

                var effectsContainerNode = spellLevelDetailsNode.SelectSingleNode(".//div[@class='row ak-spell-details-effects-container']");
                var normalEffectsNode = effectsContainerNode.SelectSingleNode(".//div[contains(@class, 'ak-spell-details-effects')][1]");
                var criticalEffectsNode = effectsContainerNode.SelectSingleNode(".//div[contains(@class, 'ak-spell-details-effects')][2]");

                var normalEffects = ExtractSpellEffects(normalEffectsNode);
                var criticalEffects = ExtractSpellEffects(criticalEffectsNode);

                spell.NormalEffects = normalEffects;
                spell.CriticalEffects = criticalEffects;

                spellDetails.Add(spell);
            }

            return spellDetails;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new List<Spell>();
        }
    }

    private static List<SpellEffect> ExtractSpellEffects(HtmlNode effectsNode)
    {
        var effects = new List<SpellEffect>();
        var effectNodes = effectsNode.SelectNodes(".//div[@class='ak-list-element']");
        foreach (var node in effectNodes)
        {
            var titleNode = node.SelectSingleNode(".//div[@class='ak-title']");
            if (titleNode == null) continue;
            
            var effect = new SpellEffect
            {
                Description = titleNode.InnerText.Trim(),
                ImageUrls = new List<string>()
            };

            var pictoNodes = titleNode.SelectNodes(".//span[@class='picto']/img");
            if (pictoNodes != null)
            {
                foreach (var pictoNode in pictoNodes)
                {
                    var imageUrl = pictoNode.GetAttributeValue("src", string.Empty);
                    effect.ImageUrls.Add(imageUrl);
                }
            }

            effects.Add(effect);
        }
        return effects;
    }
}