using System;
using Newtonsoft.Json;

namespace Thymer.Models
{
    public class Step
    {
        public Guid Id { get; }
        public string Name { get; }
        public int Hours { get; }
        public int Minutes { get; }
        public int Seconds { get; }
        [JsonIgnore]
        public string Duration => new TimeSpan(Hours, Minutes, Seconds).ToString();

        [JsonConstructor]
        public Step(Guid id, string name, int hours, int minutes, int seconds)
            => (Id, Name, Hours, Minutes, Seconds) = (id, name, hours, minutes, seconds);

        public Step(string name = "", int hours = 0, int minutes = 0, int seconds = 0)
            => (Id, Name, Hours, Minutes, Seconds) = (Guid.NewGuid(), name, hours, minutes, seconds);
    }
}