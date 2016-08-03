# FakeBus
A fake implementation of the NServiceBus's IBus interface.  It's an easy way to assert Inputs and Outputs from handlers and to help reduce testing scope.

## How do I use it?
Setup your bus / handler:
```c#
var bus = new FakeBus();
var myHandler = new Handler(bus);  // injects the fake bus
```

Run your scenario:
```c#
myHandler.Handle(someMessage);
```

Extract any sent messages from the bus and assert as needed
```c#
// Get all messages sent to Bus.SendLocal(...) 
var localSentMessages = Bus.SentLocalMessages.ToList();

// Get messages of a specific type sent to Bus.SendLocal(...)
var localSentMessages = Bus.SentLocalMessages.OfType<SomeOtherMessage>();  
  
// Get messages of a specific type sent to Bus.Send(...)
var sentMessages = Bus.SentMessages.OfType<CommandToAnotherSystem>();  

// Get messages of a specific type sent to Bus.Publish(...)
var sentMessages = Bus.PublishedMessages.OfType<SomeEvent>();  
```
