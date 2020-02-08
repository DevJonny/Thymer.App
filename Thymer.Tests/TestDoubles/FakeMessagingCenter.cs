using System;
using System.Collections.Generic;
using FluentAssertions;
using Xamarin.Forms;

namespace Thymer.Tests.TestDoubles
{
    public class FakeMessagingCenter : IMessagingCenter
    {
        private readonly List<(object subscriber, string message, bool hasCallback)> _subscribers = new List<(object, string, bool)>();
        private readonly List<(object sender, string message, object args)> _sentMessages = new List<(object sender, string message, object args)>();
        
        public void Send<TSender, TArgs>(TSender sender, string message, TArgs args) where TSender : class
        {
            _sentMessages.Add((sender, message, args));
        }

        public void Send<TSender>(TSender sender, string message) where TSender : class
        {
            _sentMessages.Add((sender, message, null));
        }

        public void Subscribe<TSender, TArgs>(object subscriber, string message, Action<TSender, TArgs> callback, TSender source = default(TSender)) where TSender : class
        {
            _subscribers.Add((subscriber, message, !(callback is null)));
        }

        public void Subscribe<TSender>(object subscriber, string message, Action<TSender> callback, TSender source = default(TSender)) where TSender : class
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe<TSender, TArgs>(object subscriber, string message) where TSender : class
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe<TSender>(object subscriber, string message) where TSender : class
        {
            throw new NotImplementedException();
        }

        public void WasSent(object sender, string message, object args)
        {
            var sentMessage = (sender, message, args);
            
            _sentMessages.Should().BeEquivalentTo(sentMessage);
        }

        public void NothingSent()
        {
            _sentMessages.Should().BeEmpty();
        }
    }
}