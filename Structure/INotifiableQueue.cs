namespace Monitoring.FluentAssertions.Structure
{
    public interface INotifiableQueue<TData>
    {
        Guid Id { get; }
        INotifier<TData> AttachNotifier(Func<TData, bool> notifyingCondition, string conditionName = null);
    }
}
