namespace DSK.Domain.Guards
{
    public static class EntitiesValidator
    {
        public static void ValidateAmount(decimal amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount cannot be negative.");
            }
        }
    }
}
