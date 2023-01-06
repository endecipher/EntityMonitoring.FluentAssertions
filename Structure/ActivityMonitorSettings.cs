namespace EntityMonitoring.FluentAssertions.Structure
{
    public class ActivityMonitorSettings : IActivityMonitorSettings
    {
        /// <summary>
        /// Denotes the maximum number of ContextQueues held by the <see cref="ActivityMonitor{TData}"/>.
        /// <para>Default is <c>unlimited</c></para>
        /// </summary>
        public int MaximumContextQueueInstances { get; init; }

        /// <summary>
        /// Adds items to the BackgroundQueue if no ContextQueues registered.
        /// Default value is false, i.e ignore items which are being added via <see cref="IActivityMonitor{TData}.Add(TData)"/>.
        /// </summary>
        public bool AcceptWithoutActiveContextQueue { get; init; } = true;

        /// <summary>
        /// Starts the <see cref="ActivityMonitor{TData}"/> implicitly, during instance initialization.
        /// Default value is false.
        /// </summary>
        public bool AutoStart { get; init; } = false;

        /// <summary>
        /// Throws Exception if attempting to add items without the <see cref="ActivityMonitor{TData}"/> being started. 
        /// Default value is false, i.e ignore items which are being added via <see cref="IActivityMonitor{TData}.Add(TData)"/>.
        /// </summary>
        public bool ThrowExceptionIfMonitorNotStarted { get; } = false;

        /// <summary>
        /// Throws Exception from <see cref="INotifier{TData}.Wait(TimeSpan)"/> if <see cref="INotifier{TData}.IsConditionMatched"/> is false after waiting.
        /// Default value is false.
        /// </summary>
        public bool ThrowExceptionIfNotifierTimeOutElapses { get; } = false;
    }
}
