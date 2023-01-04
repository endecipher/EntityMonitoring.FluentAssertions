using FluentAssertions.Execution;
using Monitoring.FluentAssertions.Structure;

namespace Monitoring.FluentAssertions.Extensions
{
    /// <summary>
    /// PropertyAssertions - where the properties of <see cref="IAssertableQueue{TData}"/> are asserted
    /// </summary>
    /// <typeparam name="TData">Type of Element</typeparam>
    public class PropertyAssertions<TData>
    {
        internal PropertyAssertions(IAssertableQueue<TData> assertableQueue, string callerExp = null)
        {
            AssertableQueue = assertableQueue as ActivityQueue<TData>;
            CallerExp = callerExp;
        }

        internal string CallerExp { get; }
        internal ActivityQueue<TData> AssertableQueue { get; }


        /// <summary>
        /// Asserts the queue size and compares with <paramref name="expectedCount"/>.
        /// </summary>
        /// <param name="expectedCount">Expected Count of Elements</param>
        /// <param name="because">
        ///     A formatted phrase as is supported by <see cref="string.Format(string,object[])"/>
        ///     explaining why the assertion is needed. If the phrase does not start with the
        ///     word because, it is prepended automatically. 
        /// </param>
        /// <param name="becauseArgs">Zero or more objects to format using the placeholders in because.</param>
        public void HavingCount(int expectedCount, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .Given(() => AssertableQueue.Queue)
                .ForCondition((queue) =>
                {
                    return queue.Count.Equals(expectedCount);
                })
                .FailWith(message: "Expected {0} to be{1}{reason}, but found {2}", CallerExp, expectedCount, AssertableQueue.Queue.Count);
        }
    }
}
