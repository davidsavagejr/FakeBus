using System;
using System.Collections.Generic;
using NServiceBus;

namespace FakeBus
{
    public class FakeBus : IBus
    {
        public FakeBus()
        {
            PublishedMessages = new List<object>();
            SubscribedMessages = new List<Type>();
            UnsubscribedMessages = new List<Type>();
            SentLocalMessages = new List<object>();
            SentMessages = new List<Tuple<string, string, object>>();
            DeferredMessages = new List<Tuple<DateTime, object>>();
        }

        public virtual T CreateInstance<T>()
        {
            return (T)CreateInstance(typeof (T));
        }

        public virtual T CreateInstance<T>(Action<T> action)
        {
            var obj = CreateInstance<T>();
            action(obj);
            return obj;
        }

        public virtual object CreateInstance(Type messageType)
        {
            return Activator.CreateInstance(messageType);
        }

        public List<object> PublishedMessages { get; private set; } 

        public void Publish<T>(params T[] messages)
        {
            if(messages == null)
                return;

            foreach (var message in messages)
                PublishedMessages.Add(message);
        }

        public void Publish<T>(Action<T> messageConstructor)
        {
            if (messageConstructor == null)
                return;

            var message = CreateInstance<T>();
            messageConstructor(message);

            PublishedMessages.Add(message);
        }

        public List<Type> SubscribedMessages { get; private set; } 

        public void Subscribe(Type messageType)
        {
            SubscribedMessages.Add(messageType);
        }

        public void Subscribe<T>()
        {
            SubscribedMessages.Add(typeof(T));
        }

        public void Subscribe(Type messageType, Predicate<object> condition)
        {
            if(condition(CreateInstance(messageType)))
                Subscribe(messageType);
        }

        public void Subscribe<T>(Predicate<T> condition)
        {
            if(condition(CreateInstance<T>()))
                Subscribe<T>();
        }

        public List<Type> UnsubscribedMessages { get; private set; }

        public void Unsubscribe(Type messageType)
        {
            UnsubscribedMessages.Add(messageType);
        }

        public void Unsubscribe<T>()
        {
            UnsubscribedMessages.Add(typeof(T));
        }

        public List<object> SentLocalMessages { get; private set; }

        public ICallback SendLocal(params object[] messages)
        {
            if (messages == null)
                return null;

            foreach (var message in messages)
                SentLocalMessages.Add(message);

            return null;
        }

        public ICallback SendLocal<T>(Action<T> messageConstructor)
        {
            if (messageConstructor == null)
                return null;

            var message = CreateInstance<T>();
            messageConstructor(message);

            SentLocalMessages.Add(message);
            return null;
        }

        public List<Tuple<string, string, object>> SentMessages { get; private set; }

        public ICallback Send(params object[] messages)
        {
            foreach (var message in messages)
                SentMessages.Add(new Tuple<string, string, object>(null, null, message));

            return null;
        }

        public ICallback Send<T>(Action<T> messageConstructor)
        {
            var message = CreateInstance<T>();
            messageConstructor(message);

            SentMessages.Add(new Tuple<string, string, object>(null, null, message));
            return null;
        }

        public ICallback Send(string destination, params object[] messages)
        {
            return Send(Address.Parse(destination), messages);
        }

        public ICallback Send(Address address, params object[] messages)
        {
            return Send(address, null, messages);
        }

        public ICallback Send<T>(string destination, Action<T> messageConstructor)
        {
            return Send(Address.Parse(destination), messageConstructor);
        }

        public ICallback Send<T>(Address address, Action<T> messageConstructor)
        {
            return Send(address, null, messageConstructor);
        }

        public ICallback Send(string destination, string correlationId, params object[] messages)
        {
            return Send(Address.Parse(destination), correlationId, messages);
        }

        public ICallback Send(Address address, string correlationId, params object[] messages)
        {
            foreach (var message in messages)
                SentMessages.Add(new Tuple<string, string, object>(address.ToString(), correlationId, message));

            return null;
        }

        public ICallback Send<T>(string destination, string correlationId, Action<T> messageConstructor)
        {
            return Send(Address.Parse(destination), correlationId, messageConstructor);
        }

        public ICallback Send<T>(Address address, string correlationId, Action<T> messageConstructor)
        {
            var message = CreateInstance<T>();
            messageConstructor(message);

            SentMessages.Add(new Tuple<string, string, object>(address.ToString(), correlationId, message));
            return null;
        }

        public ICallback SendToSites(IEnumerable<string> siteKeys, params object[] messages)
        {
            throw new NotImplementedException();
        }

        public List<Tuple<DateTime, object>> DeferredMessages { get; private set; } 

        public ICallback Defer(TimeSpan delay, params object[] messages)
        {
            return Defer(DateTime.Now.Add(delay), messages);
        }

        public ICallback Defer(DateTime processAt, params object[] messages)
        {
            foreach (var message in messages)
                DeferredMessages.Add(new Tuple<DateTime, object>(processAt, message));

            return null;    
        }

        public List<object> Replies { get; private set; } 

        public void Reply(params object[] messages)
        {
            foreach(var message in messages)
                Replies.Add(message);
        }

        public void Reply<T>(Action<T> messageConstructor)
        {
            var message = CreateInstance<T>();
            messageConstructor(message);

            Replies.Add(message);
        }

        public void Return<T>(T errorEnum)
        {
            throw new NotImplementedException();
        }

        public void HandleCurrentMessageLater()
        {
            throw new NotImplementedException();
        }

        public void ForwardCurrentMessageTo(string destination)
        {
            throw new NotImplementedException();
        }

        public void DoNotContinueDispatchingCurrentMessageToHandlers()
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, string> OutgoingHeaders { get; private set; }

        public IMessageContext CurrentMessageContext { get; private set; }

        public IInMemoryOperations InMemory { get; private set; }
    }
}
