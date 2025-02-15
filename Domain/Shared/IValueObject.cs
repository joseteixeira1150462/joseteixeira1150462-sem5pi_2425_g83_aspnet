namespace HealthCare.Domain.Shared
{
    public interface IValueObject
    {
        public bool Equals(object obj);
        public int GetHashCode();
    }
}