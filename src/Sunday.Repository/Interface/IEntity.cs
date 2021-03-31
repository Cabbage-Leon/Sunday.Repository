namespace Sunday.Repository
{
    public interface IEntity : IEntity<string>
    {
    }

    public interface IEntity<TPrimaryKey> : ITrack
    {
        TPrimaryKey Id { get; }
    }
}
