using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace LowTideEnt.Domain.Entities.ResourceManager
{
    [Table("Resource", Schema = "ResourceManagement")]
    public class ResourceEntity : BaseEntity
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        private ResourceEntity() { }
        [SetsRequiredMembers]
        public ResourceEntity(int categoryId, string title, string mdContent, MetadataObject metadata)
        {
            var metadataJson = JsonSerializer.Serialize(metadata, JsonOptions);
            CategoryId = categoryId;
            Title = title ?? string.Empty;
            MdContent = mdContent ?? string.Empty;
            HtmlContent = ToHtml();
            Metadata = metadataJson ?? "{}";
        }
        public int CategoryId { get; set; }
        public required string Title { get; set; } = string.Empty;
        public required string MdContent { get; set; } = string.Empty;
        public required string HtmlContent { get; set; } = string.Empty;
        [Column(TypeName = "jsonb")]
        public required string Metadata { get; set; }
        public string ToHtml()
        {
            string html = MdContent;

            // Headers
            html = Regex.Replace(html, @"^#### (.*)$", "<h4>$1</h4>", RegexOptions.Multiline);
            html = Regex.Replace(html, @"^### (.*)$", "<h3>$1</h3>", RegexOptions.Multiline);
            html = Regex.Replace(html, @"^## (.*)$", "<h2>$1</h2>", RegexOptions.Multiline);
            html = Regex.Replace(html, @"^# (.*)$", "<h1>$1</h1>", RegexOptions.Multiline);

            // Bold and italic
            html = Regex.Replace(html, @"\*\*(.*?)\*\*", "<strong>$1</strong>");
            html = Regex.Replace(html, @"\*(.*?)\*", "<em>$1</em>");

            // Line breaks
            html = html.Replace("\n", "<br/>\n");

            return html;
        }
    }

    public class MetadataObject
    {
        public string Department { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
    }
}
