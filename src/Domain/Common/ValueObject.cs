namespace ResourcePackerGUI.Domain.Common
{
    public abstract class ValueObject
    {
        public static bool operator !=(ValueObject? one, ValueObject? two)
        {
            return !EqualOperator(one, two);
        }

        public static bool operator ==(ValueObject? one, ValueObject? two)
        {
            return EqualOperator(one, two);
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            var other = (ValueObject)obj;
            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Select(x => x.GetHashCode())
                .Aggregate((x, y) => x ^ y);
        }

        protected static bool EqualOperator(ValueObject? left, ValueObject? right)
        {
            if (left is null ^ right is null)
            {
                return false;
            }

            return left?.Equals(right) != false;
        }

        protected abstract IEnumerable<object> GetEqualityComponents();
    }
}