namespace Monitoring.FluentAssertions.Structure
{
    internal interface IContextQueueRegistrar<TData>
    {
        IActivityMonitorSettings Settings { get; }
        void Register(IAssertableQueue<TData> queue);
    }
}
