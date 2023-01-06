using System.Collections.Concurrent;

namespace EntityMonitoring.FluentAssertions.Structure
{
    internal class ActivityQueue<TData> : IAssertableQueue<TData>, INotifiableQueue<TData>, IDisposable
    {
        public ConcurrentQueue<TData> Queue { get; }

        public ConcurrentDictionary<int, Notifier<TData>> Notifiers { get; }

        public Guid Id { get; }

        public IActivityMonitorSettings Settings { get; init; }

        internal ActivityQueue(Guid instanceId)
        {
            Id = instanceId;

            Queue = new ConcurrentQueue<TData>();
            Notifiers = new ConcurrentDictionary<int, Notifier<TData>>();
        }

        protected internal void Clear()
        {
            Queue.Clear();
        }

        protected internal virtual void Add(TData item)
        {
            Queue.Enqueue(item);

            foreach (var (hashId, notifier) in Notifiers)
            {
                if (notifier.Verify(item))
                {
                    if (notifier.RemovePostMatch)
                    {
                        Notifiers.TryRemove(hashId, out var removedNotifier);
                    }
                }
            }
        }

        public void Dispose()
        {
            Clear();
        }

        public INotifier<TData> AttachNotifier(Func<TData, bool> notifyingCondition, string conditionName = null)
        {
            Notifier<TData> notifier = new Notifier<TData>
            {
                Condition = notifyingCondition,
                ConditionName = conditionName,
                Settings = Settings
            };

            int hashCode = notifier.GetHashCode();

            Notifiers.AddOrUpdate(hashCode, notifier, (hash, n) => n);

            return notifier;
        }
    }
}
