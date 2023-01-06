
# EntityMonitoring.FluentAssertions

Capture, Monitor and "Fluently" Assert over Events/Log Messages/Application Activities/Any Collection of ```TData``` Entities.





## Features
- Single-entry-point capture of all data by the `ActivityMonitor<TData>` during application processing
- Control creation of ContextQueues
    - Create multiple filtered subsets of captured entities as per use-case
- Notifying Capabilities
    - Wait for a specific `TData` entity/a timeout to be captured
    - Conditionally continue execution of test-case
- Assert over ContextQueues using in-built extensions of FluentAssertions



## Documentation

ActivityMonitor is the gate through which all similar messages/activities/entities go through.
Register your ActivityMonitor via DI as a singleton, and use the below interface and class from this library. 

```C# 
IActivityMonitor<TData> 
ActivityMonitor<TData> //Implementation
```

Your incoming supply of data consisting of ```TData``` entities should be linked to
```C#
void IActivityMonitor<TData>.Add(TData item)
```

You may then call ```IActivityMonitor<TData>.Start()``` and ```IActivityMonitor<TData>.Stop()```, before the start, and at the end of your test case respectively for memory deallocation.

### How it works

The ActivityMonitor internally has a system-created BackgroundQueue and multiple user-created ContextQueues where it captured entities are stored and segregated.
Entities are added to the BackgroundQueue only if there are no ContextQueues created for capturing data.

A ContextQueue of type ```TData``` is an assertable queue of entities which a test-case may use for capturing data with an optional filter.
```C#
ContextQueueBuilder<TData> IActivityMonitor<TData>.Capture();
```
You may build the ContextQueue with matching predicates.

Once the ActivityMonitor appends the ContextQueue with ```TData``` datas, the test may call the below to end captures and begin assertions.
```C#
IAssertableQueue<TData> IActivityMonitor<TData>.EndCapture(Guid contextQueueId); //Passing in ContextQueue
```

Assertions are provided as extensions to the `IAssertableQueue<TData>`, and may be used linked
```C#
Queue.Search().UntilItContains(..) 
Queue.Search().UntilItSatisfies(..)
Queue.Dig().UntilItContains(..)
Queue.Dig().UntilItSatisfies(..)
Queue.Is().HavingCount(..)
```

Note: `Dig()` removes non-matching elements from the `IAssertableQueue` unlike `Search()`.  


### Configuration Settings

Configure `IActivityMonitorSettings` before `ActivityMonitor` initialization during startup:

```C# Interface
IActivityMonitorSettings
ActivityMonitorSettings //Implementation
```

#### `IActivityMonitorSettings.MaximumContextQueueInstances`
- Integer - Denotes the maximum number of ContextQueues held by the ActivityMonitor<TData>. Default value is unlimited.


#### `IActivityMonitorSettings.AcceptWithoutActiveContextQueue`
- Boolean - Adds items to the BackgroundQueue if no ContextQueues registered. Default value is false, i.e ignore items which are being added via IActivityMonitor<TData>.Add(TData)

#### `IActivityMonitorSettings.AutoStart`
- Boolean - Starts the ActivityMonitor<TData> implicitly, during instance initialization. Default value is false.

#### `IActivityMonitorSettings.ThrowExceptionIfMonitorNotStarted`
- Boolean - Throws Exception if attempting to add items without the ActivityMonitor<TData> being started. Default value is false, i.e ignore items which are being added via IActivityMonitor<TData>.Add(TData).

#### `IActivityMonitorSettings.ThrowExceptionIfNotifierTimeOutElapses`
- Boolean - Throws Exception from INotifier<TData>.Wait(TimeSpan) if INotifier<Data>.IsConditionMatched is false after waiting. Default value is false.

## Example Usage 

```C#
//Get from DI
IActivityMonitor<Data> Monitor; 

//Register a ContextQueue with no filter and begin capturing
INotifiableQueue<Data> contextQueue = Monitor.Capture().All(); 

//You may attach a notifier to this contextQueue, so that if an element does get added with this predicate, it will alert all waiting threads
INotifier<Data> condition = contextQueue
    .AttachNotifier(x => predicate(x), "IsElementMatched").RemoveOnceMatched(); 
.
.


/*
 * Wait for either 
 *  1) Data to be appended to ActivityMonitor such that it's satisfied by the predicate() or 
 *  2) the timeOut
 * to continue the execution of the test-case
 */
condition.Wait(TimeSpan); 
.
.


//Appropriate Data gets appended to ContextQueue which satisfies predicate(), releasing waiting thread.
//Execution continues..
.
.

//End ContextQueue Capture, so that assertions can begin
IAssertableQueue<Data> assertableQueue = 
    monitor.EndCapture(contextQueue.Id);

//Assert over captured data
assertableQueue
    .Search() //Enumerate elements without removing
    .UntilItContains(
        expectedData: someData,
        comparer: IComparer<Data>,
        because: "",
        becauseArgs: null
    );

assertableQueue
    .Dig() //Enumerate while removing elements if not matched
    .UntilItSatisfies(
        matchingCondition: _ => dataPredicate2(_),
        because: "",
        becauseArgs: null
    );
```



## Contributing

Contributions are always welcome!



## Authors

- [@endecipher](https://www.github.com/endecipher)


## License

[MIT](https://choosealicense.com/licenses/mit/)