using System;
using Stargate.API.Infrastructure;

namespace Stargate.API.Services
{
    public class BijectiveUriService : IUriShortener
    {
        public string GetShortUri(int databaseId)
        {
            var shortUri = Bijective.Encode(databaseId);
            return shortUri;
        }
    }
}