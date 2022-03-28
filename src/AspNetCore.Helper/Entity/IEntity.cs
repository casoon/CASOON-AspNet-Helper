using System;

namespace AspNetCore.Extensions.Entity
{
    public interface IEntity
    {
        string Id { get; set; }

        DateTime Created { get; set; }

        DateTime LastUpdated { get; set; }
    }
}