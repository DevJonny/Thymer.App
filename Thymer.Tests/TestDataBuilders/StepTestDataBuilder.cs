using System;
using Thymer.Models;

namespace Thymer.Tests.TestDataBuilders
{
    public class StepTestDataBuilder
    {
        private class StepSpecification
        {
            public Guid Id { get; set; } = Guid.NewGuid();
            public string Name { get; set; } = RandomString;
            public long Duration { get; set; } = RandomLong;
            
            private static string RandomString => $"{Guid.NewGuid()}";
            private static long RandomLong => _random.Next();
            
            private static readonly Random _random = new Random();
        }

        private readonly StepSpecification _stepSpecification;

        public StepTestDataBuilder()
        {
            _stepSpecification = new StepSpecification();
        }

        public StepTestDataBuilder WithId(Guid id)
        {
            _stepSpecification.Id = id;
            return this;
        }

        public StepTestDataBuilder WithName(string name)
        {
            _stepSpecification.Name = name;
            return this;
        }

        public StepTestDataBuilder WithDuration(long duration)
        {
            _stepSpecification.Duration = duration;
            return this;
        }

        public Step Build()
        {
            return new Step(
                _stepSpecification.Id,
                _stepSpecification.Name,
                _stepSpecification.Duration);
        }
    }
}