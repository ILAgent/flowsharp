using System;

namespace FlowSharp.Operators
{
    public static class Operators
    {
        public static IFlow<T> Filter<T>(this IFlow<T> source, Func<T, bool> predicate) =>
            FlowFactory.Flow<T>((collector, cancellationToken) =>
                source.Collect(item =>
                {
                    if (predicate(item))
                        collector.Emit(item);
                }, cancellationToken)
            );
    }
}
