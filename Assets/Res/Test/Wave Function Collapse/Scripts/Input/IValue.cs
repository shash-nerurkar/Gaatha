using System;
using System.Collections.Generic;

public interface IValue<T> : IEqualityComparer<IValue<T>>, IEquatable<IValue<T>>
{
    T Value { get; }
}
