using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBrowser.Controls.BrowserTabStrip.Data
{
    [Table("Products")]
    public class ProductInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string SpecificationsJson { get; set; }

        public string Description { get; set; }


        public DateTime ScrapedAt { get; set; }


        public string ImagePathsJson { get; set; }

        [NotMapped]
        public List<string> ImagePaths
        {
            get => string.IsNullOrEmpty(ImagePathsJson)
                ? new List<string>()
                : System.Text.Json.JsonSerializer.Deserialize<List<string>>(ImagePathsJson);
            set => ImagePathsJson = System.Text.Json.JsonSerializer.Serialize(value);
        }

        public string ImageUrlsJson { get; set; }  // Backing store in DB

        [NotMapped]
        public List<string> ImageUrls
        {
            get
            {
                if (string.IsNullOrEmpty(ImageUrlsJson))
                    return new List<string>();
                try
                {
                    return System.Text.Json.JsonSerializer.Deserialize<List<string>>(ImageUrlsJson)
                        ?? new List<string>();
                }
                catch
                {
                    return new List<string>();
                }
            }
            set
            {
                ImageUrlsJson = System.Text.Json.JsonSerializer.Serialize(value ?? new List<string>());
            }
        }


        private Dictionary<string, string> _specs = new();

        [NotMapped]
        public Dictionary<string, string> Specifications
        {
            get
            {
                if (_specs == null || _specs.Count == 0 && !string.IsNullOrEmpty(SpecificationsJson))
                    _specs = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(SpecificationsJson)
                        ?? new Dictionary<string, string>();
                return _specs;
            }
            set
            {
                _specs = value;
                SpecificationsJson = System.Text.Json.JsonSerializer.Serialize(value ?? new Dictionary<string, string>());
            }
        }


    }

}
