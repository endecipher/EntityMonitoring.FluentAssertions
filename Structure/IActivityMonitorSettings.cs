namespace Monitoring.FluentAssertions.Structure
{
    public interface IActivityMonitorSettings
    {
        bool AcceptWithoutActiveContextQueue { get; }
        bool AutoStart { get; }
        bool ThrowExceptionIfMonitorNotStarted { get; }
        bool ThrowExceptionIfNotifierTimeOutElapses { get; }
        int MaximumContextQueueInstances { get; }
    }
}
