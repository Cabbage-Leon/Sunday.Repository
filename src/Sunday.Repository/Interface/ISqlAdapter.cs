using Sunday.Repository.Impl.Adapter;

namespace Sunday.Repository
{
    public interface ISqlAdapter
    {
        string PagingBuild(ref PartedSql partedSql, object sqlArgs, long skip, long take);
    }
}
