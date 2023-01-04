using Monitoring.FluentAssertions.Exceptions;
using System.Collections.Concurrent;

namespace Monitoring.FluentAssertions.Structure
{
    public sealed class ActivityMonitor<TData> : IContextQueueRegistrar<TData>, IActivityMonitor<TData>
    {
        object _lock = new object();
        public IActivityMonitorSettings Settings { get; }
        bool IsStarted { get; set; } = false;
        ConcurrentDictionary<Guid, ContextQueue<TData>> ContextQueues { get; }
        ActivityQueue<TData> BackgroundQueue { get; }

        public ActivityMonitor(IActivityMonitorSettings activityMonitorSettings)
        {
            Settings = activityMonitorSettings;

            lock (_lock)
            {
                BackgroundQueue = new ActivityQueue<TData>(Guid.Empty)
                {
                    Settings = activityMonitorSettings
                };

                ContextQueues = new ConcurrentDictionary<Guid, ContextQueue<TData>>();

                if (Settings.AutoStart)
                {
                    IsStarted = true;
                }
            }
        }

        public void Start()
        {
            lock (_lock)
            {
                if (IsStarted) throw new MonitorAlreadyStartedException();

                IsStarted = true;
            }
        }

        public void Stop()
        {
            lock (_lock)
            {
                if (!IsStarted) throw new MonitorAlreadyStoppedException();

                IsStarted = false;
            }
        }

        public void ClearBackgroundQueue()
        {
            BackgroundQueue.Clear();
        }

        public void RemoveContextQueue(IAssertableQueue<TData> contextQueue)
        {
            RemoveContextQueue(contextQueue.Id);
        }

        public void RemoveContextQueue(Guid contextQueueId)
        {
            ContextQueues.TryRemove(contextQueueId, out var queue);
        }

        public void ClearAllQueues()
        {
            ClearBackgroundQueue();
            ContextQueues.Clear();
        }

        public void Add(TData item)
        {
            if (!IsStarted)
            {
                if (Settings.ThrowExceptionIfMonitorNotStarted) throw new MonitorNotStartedException();

                return;
            }

            if (ContextQueues.IsEmpty)
            {
                if (Settings.AcceptWithoutActiveContextQueue)
                {
                    BackgroundQueue.Add(item);
                }

                return;
            }

            Parallel.ForEach(ContextQueues.Values, queue => queue.Add(item));
        }

        public ContextQueueBuilder<TData> Capture()
        {
            return new ContextQueueBuilder<TData>(this as IContextQueueRegistrar<TData>);
        }

        public IAssertableQueue<TData> EndCapture(Guid contextQueueId)
        {
            if (ContextQueues.TryGetValue(contextQueueId, out var contextQueue))
            {
                contextQueue.DisallowCaptures();

                return contextQueue;
            }

            throw new QueueIdNotFoundException(contextQueueId);
        }

        public void Register(IAssertableQueue<TData> queue)
        {
            var maxQueueInstances = Settings.MaximumContextQueueInstances;

            if (maxQueueInstances > 0 && ContextQueues.Count >= maxQueueInstances)
            {
                throw new ReachedMaximumContextQueueInstancesException();
            }

            var queueId = queue.Id;

            ContextQueues.AddOrUpdate(queueId, addValue: queue as ContextQueue<TData>, updateValueFactory: (key, existingQueue) => existingQueue);
        }
    }
}
