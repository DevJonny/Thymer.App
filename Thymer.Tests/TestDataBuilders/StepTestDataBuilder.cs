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
            public int Hours { get; set; } = _random.Next(24);
            public int Minutes { get; set; } = _random.Next(59);
            public int Seconds { get; set; } = _random.Next(59);
            
            private static string RandomString => $"{Guid.NewGuid()}";
            
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

        public StepTestDataBuilder WithHours(int hours)
        {
            _stepSpecification.Hours = hours;
            return this;
        }

        public StepTestDataBuilder WithMinutes(int minutes)
        {
            _stepSpecification.Minutes = minutes;
            return this;
        }

        public StepTestDataBuilder WithSeconds(int seconds)
        {
            _stepSpecification.Seconds = seconds;
            return this;
        }

        public Step Build()
        {
            return new Step(
                _stepSpecification.Id,
                _stepSpecification.Name,
                _stepSpecification.Hours,
                _stepSpecification.Minutes,
                _stepSpecification.Seconds);
        }
    }
}