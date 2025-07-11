namespace Intellias.Template.Api.Options
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;

#pragma warning disable CA1710 // Identifiers should have correct suffix
    public class CacheProfileOptions : Dictionary<string, CacheProfile>
#pragma warning restore CA1710 // Identifiers should have correct suffix
    {
    }
}
