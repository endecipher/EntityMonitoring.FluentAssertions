using EntityMonitoring.FluentAssertions.Structure;

namespace EntityMonitoring.FluentAssertions.Extensions
{
    public static class AssertableQueueExtensions
    {
        public static SearchAssertions<TData> Search<TData>(this IAssertableQueue<TData> queue, [System.Runtime.CompilerServices.CallerArgumentExpression("queue")] string callerExp = "")
        {
            return new SearchAssertions<TData>(queue, callerExp);
        }

        /// <summary>
        /// Search while discarding entries which did not qualify
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="queue"></param>
        /// <returns></returns>
        public static DigAssertions<TData> Dig<TData>(this IAssertableQueue<TData> queue, [System.Runtime.CompilerServices.CallerArgumentExpression("queue")] string callerExp = "")
        {
            return new DigAssertions<TData>(queue, callerExp);
        }


        public static PropertyAssertions<TData> Is<TData>(this IAssertableQueue<TData> queue, [System.Runtime.CompilerServices.CallerArgumentExpression("queue")] string callerExp = "")
        {
            return new PropertyAssertions<TData>(queue, callerExp);
        }
    }
}
