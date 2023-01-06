namespace EntityMonitoring.FluentAssertions.Exceptions
{
    public class QueueIdNotFoundException : Exception
    {
        Guid ContextQueueId;

        public QueueIdNotFoundException(Guid contextQueueId) : base()
        {
            ContextQueueId = contextQueueId;
        }
    }
}