using System.Collections.Generic;
using Newtonsoft.Json;

namespace Fabricam.UserApi.Models
{
    // TODO: consider moving these models to a common assembly consumed by both sides

    /// <summary>
    /// The base class of all responses
    /// </summary>
    public class ValidationResponse
    {
        public string Message { get; set; }
        public List<ValidationError> Errors { get; set; } = new List<ValidationError>();
        public bool Success => this.Errors == null || this.Errors.Count < 1;
    }

    public class ValidationError
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] // exclude from JSON if null
        public string Field { get; set; }
        public string Message { get; set; }
    }
}
