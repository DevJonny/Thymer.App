using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Thymer.Tests.TestDoubles
{
    public class FakeMessagingCenter : IMessagingCenter
    {
        public List<(object subscriber, string message, bool hasCallback)> Subscribers = new List<(object, string, bool)>();
        
        public void Send<TSender, TArgs>(TSender sender, string message, TArgs args) where TSender : class
        {
            throw new NotImplementedException();
        }

        public void Send<TSender>(TSender sender, string message) where TSender : class
        {
            throw new NotImplementedException();
        }

        public void Subscribe<TSender, TArgs>(object subscriber, string message, Action<TSender, TArgs> callback, TSender source = default(TSender)) where TSender : class
        {
            Subscribers.Add((subscriber, message, !(callback is null)));
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
    }
}