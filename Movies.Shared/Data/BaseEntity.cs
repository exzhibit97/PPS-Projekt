using System;
using System.Collections.Generic;
using System.Text;

namespace Movies.Shared
{
    // This can be modified to BaseEntity<TId> to support multiple key types (e.g. Guid)
    public abstract class BaseEntity
    {
        public int Id { get; set; }
    }
}