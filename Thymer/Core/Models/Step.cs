using System;
using Newtonsoft.Json;

namespace Thymer.Models
{
    public class Step
    {
        public Guid Id { get; }
        public string Name { get; }
        public long Duration { get; }

        [JsonConstructor]
        public Step(Guid id, string name, long duration)
            => (Id, Name, Duration) = (id, name, duration);

        public Step(string name = "", long duration = 0)
            => (Id, Name, Duration) = (Guid.NewGuid(), name, duration);

    }
}