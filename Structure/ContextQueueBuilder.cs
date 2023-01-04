namespace Monitoring.FluentAssertions.Structure
{
    public class ContextQueueBuilder<TData> : IDisposable
    {
        IContextQueueRegistrar<TData> Monitor { get; set; }

        internal ContextQueueBuilder(IContextQueueRegistrar<TData> monitor)
        {
            Monitor = monitor;
        }

        public INotifiableQueue<TData> All()
        {
            var contextQueueId = Guid.NewGuid();

            var queue = new ContextQueue<TData>(contextQueueId, _ => true)
            {
                Settings = Monitor.Settings
            };

            Monitor.Register(queue);

            Dispose();

            return queue as INotifiableQueue<TData>;
        }

        public INotifiableQueue<TData> WithCondition(Func<TData, bool> captureCondition)
        {
            var contextQueueId = Guid.NewGuid();

            var queue = new ContextQueue<TData>(contextQueueId, captureCondition)
            {
                Settings = Monitor.Settings
            };

            Monitor.Register(queue);

            Dispose();

            return queue as INotifiableQueue<TData>;
        }

        public void Dispose()
        {
            Monitor = null;
        }
    }
}
