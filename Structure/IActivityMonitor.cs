namespace EntityMonitoring.FluentAssertions.Structure
{
    public interface IActivityMonitor<TData>
    {
        /// <summary>
        /// Singular entry point for all items to be captured, monitored or asserted.
        /// </summary>
        /// <param name="item">Data to be added to underlying queues</param>
        void Add(TData item);

        /// <summary>
        /// Create a ContextQueue for capturing a subset of data from the <see cref="IActivityMonitor{TData}"/>
        /// </summary>
        /// <returns>Builder for ContextQueue</returns>
        ContextQueueBuilder<TData> Capture();

        /// <summary>
        /// Clears the BackgroundQueue and all created ContextQueues
        /// </summary>
        void ClearAllQueues();

        /// <summary>
        /// Clears the BackgroundQueue
        /// </summary>
        void ClearBackgroundQueue();

        /// <summary>
        /// Inactivate the ContextQueue; disallowing further capturing of data
        /// </summary>
        /// <param name="contextQueueId">A unique <see cref="Guid"/> resembling the ContextQueue that had been earlier registered</param>
        /// <returns>A collection over which assertions can operate</returns>
        IAssertableQueue<TData> EndCapture(Guid contextQueueId);

        /// <summary>
        /// Remove the reference of the ContextQueue from the Monitor
        /// </summary>
        void RemoveContextQueue(IAssertableQueue<TData> contextQueue);

        /// <summary>
        /// Remove the reference of the ContextQueue having id as <paramref name="contextQueueId"/> from the Monitor
        /// </summary>
        void RemoveContextQueue(Guid contextQueueId);

        /// <summary>
        /// Start the ActivityMonitor for global capturing of data
        /// </summary>
        void Start();

        /// <summary>
        /// Stop the ActivityMonitor. Disallow all captures.
        /// </summary>
        void Stop();
    }
}