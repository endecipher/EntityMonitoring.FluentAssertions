namespace EntityMonitoring.FluentAssertions.Structure
{
    internal class Notifier<TData> : INotifier<TData>
    {
        public Notifier()
        {
            IsConditionMatched = false;
        }

        public bool IsConditionMatched { get; protected set; }
        public bool RemovePostMatch { get; protected set; } = false;
        public Func<TData, bool> Condition { get; init; }
        AutoResetEvent Awaiter { get; set; } = new AutoResetEvent(false);
        public string ConditionName { get; init; }
        public IActivityMonitorSettings Settings { get; init; }


        public bool Wait(TimeSpan timeSpan)
        {
            Awaiter?.WaitOne(timeSpan);

            if (!IsConditionMatched && Settings.ThrowExceptionIfNotifierTimeOutElapses)
                throw new Exceptions.NotifierTimeOutException();

            return IsConditionMatched;
        }

        public INotifier<TData> RemoveOnceMatched()
        {
            RemovePostMatch = true;
            return this;
        }

        protected internal bool Verify(TData data)
        {
            if (Condition(data))
            {
                Awaiter?.Set();
                IsConditionMatched = true;
                return true;
            }

            return false;
        }

        internal void Reset()
        {
            IsConditionMatched = false;
            Awaiter?.Reset();
        }
    }
}
