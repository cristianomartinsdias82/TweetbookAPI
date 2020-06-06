namespace TweetbookAPI.Domain
{
    public abstract class Entity<TKey>
    {
        public TKey Id { get; set; }

        public override int GetHashCode()
        {
            return Id.GetHashCode() * 97;
        }
    }
}
