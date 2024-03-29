﻿namespace Application.UnitTests.Common.Utilities
{
    internal static class FnvHash
    {
        internal static int Compute(params byte[] data)
        {
            unchecked
            {
                const int p = 16777619;
                var hash = data.Aggregate((int)2166136261, (current, t) => (current ^ t) * p);

                hash += hash << 13;
                hash ^= hash >> 7;
                hash += hash << 3;
                hash ^= hash >> 17;
                hash += hash << 5;
                return hash;
            }
        }
    }
}