using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AspNetCore.Extensions.Entity
{
    [Serializable]
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public abstract class BaseEntity : IEntity
    {
        protected BaseEntity()
        {
            Id = Guid.NewGuid().ToString();
            Created = DateTime.Now;
            LastUpdated = DateTime.Now;
        }

        [Key] public string Id { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}