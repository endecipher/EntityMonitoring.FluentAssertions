namespace EntityMonitoring.FluentAssertions.Structure
{
    public interface INotifier<TData>
    {
        Func<TData, bool> Condition { get; init; }
        string ConditionName { get; init; }

        bool IsConditionMatched { get; }

        /// <summary>
        /// Caller of this method would wait until either <see cref="IsConditionMatched"/> becomes true or the <paramref name="timeSpan"/> has elpased.
        /// Internally uses <see cref="AutoResetEvent"/> and <see cref="WaitHandle.WaitOne(TimeSpan)"/> to release lock and continue execution. 
        /// 
        /// Conditionally throws 
        /// </summary>
        /// <returns><see cref="IsConditionMatched"/></returns>
        bool Wait(TimeSpan timeSpan);

        /// <summary>
        /// Remove the Notifier from the ContextQueue once <see cref="IsConditionMatched"/> becomes true
        /// </summary>
        INotifier<TData> RemoveOnceMatched();
    }
}