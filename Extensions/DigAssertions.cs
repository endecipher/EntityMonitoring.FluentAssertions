using EntityMonitoring.FluentAssertions.Structure;
using FluentAssertions.Execution;

namespace EntityMonitoring.FluentAssertions.Extensions
{
    /// <summary>
    /// DigAssertions - where the <see cref="IAssertableQueue{TData}"/> is enumerated while removing elements from the queue; for assertions
    /// </summary>
    /// <typeparam name="TData">Type of Element</typeparam>
    public class DigAssertions<TData>
    {
        internal DigAssertions(IAssertableQueue<TData> assertableQueue, string callerExp = null)
        {
            AssertableQueue = assertableQueue as ActivityQueue<TData>;
            CallerExp = callerExp;
        }

        internal string CallerExp { get; }
        internal ActivityQueue<TData> AssertableQueue { get; }

        /// <summary>
        /// Searches the collection while removing elements in order, until it finds <paramref name="expectedData"/>.
        /// If <paramref name="comparer"/> is not supplied, checks using <see cref="object.Equals(object)"/>.
        /// </summary>
        /// <param name="expectedData">Entity expected to be found</param>
        /// <param name="comparer">Equality comparer</param>
        /// <param name="because">
        ///     A formatted phrase as is supported by <see cref="string.Format(string,object[])"/>
        ///     explaining why the assertion is needed. If the phrase does not start with the
        ///     word because, it is prepended automatically. 
        /// </param>
        /// <param name="becauseArgs">Zero or more objects to format using the placeholders in because.</param>
        public void UntilItContains(TData expectedData, IComparer<TData> comparer = null, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .Given(() => AssertableQueue.Queue)
                .ForCondition((queue) =>
                {
                    bool IsExpectedElementFound = false;

                    while (queue.TryDequeue(out TData item))
                    {
                        if (comparer != null)
                        {
                            IsExpectedElementFound = comparer.Compare(expectedData, item) == 0;
                        }
                        else
                        {
                            IsExpectedElementFound = item.Equals(expectedData);
                        }

                        if (IsExpectedElementFound)
                        {
                            break;
                        }
                    }

                    return IsExpectedElementFound;
                })
                .FailWith(message: "Expected {0} to have at-least one element which matches the expected{reason}, but found none", CallerExp);
        }

        /// <summary>
        /// Searches the collection (removing elements in order) until the <paramref name="matchingCondition"/> satisifies.
        /// </summary>
        /// <param name="matchingCondition">Predicate which returns true if condition satisifes</param>
        /// <param name="because">
        ///     A formatted phrase as is supported by <see cref="string.Format(string,object[])"/>
        ///     explaining why the assertion is needed. If the phrase does not start with the
        ///     word because, it is prepended automatically. 
        /// </param>
        /// <param name="becauseArgs">Zero or more objects to format using the placeholders in because.</param>
        public void UntilItSatisfies(Func<TData, bool> matchingCondition, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .Given(() => AssertableQueue.Queue)
                .ForCondition((queue) =>
                {
                    bool IsExpectedElementFound = false;

                    while (queue.TryDequeue(out TData item))
                    {
                        IsExpectedElementFound = matchingCondition.Invoke(item);

                        if (IsExpectedElementFound)
                        {
                            break;
                        }
                    }

                    return IsExpectedElementFound;
                })
                .FailWith(message: "Expected {0} to have at-least one element which matches the input condition{reason}, but found none", args: CallerExp);
        }
    }
}
