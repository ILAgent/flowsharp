using System;

namespace FlowSharp.Operators
{
    public static class Operators
    {
        public static IFlow<T> Filter<T>(this IFlow<T> source, Func<T, bool> predicate)
        {
            return FlowFactory.Flow<T>((collector, cancellationToken) =>
                source.Collect(item =>
                {
                    if (predicate(item))
                        collector.Emit(item);
                }, cancellationToken)
            );
        }

        public static IFlow<R> Map<T, R>(this IFlow<T> source, Func<T, R> mapper)
        {
            return FlowFactory.Flow<R>((collector, cancellationToken) =>
                source.Collect(
                    item => collector.Emit(mapper(item)),
                    cancellationToken
                )
            );
        }

        public static IFlow<T> OnNext<T>(this IFlow<T> source, Action<T> action)
        {
            return FlowFactory.Flow<T>((collector, cancellationToken) =>
                source.Collect(item =>
                {
                    action(item);
                    collector.Emit(item);
                }, cancellationToken)
            );
        }
    }
}