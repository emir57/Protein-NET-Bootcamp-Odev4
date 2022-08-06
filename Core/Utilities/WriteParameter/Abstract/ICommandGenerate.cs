namespace WriteParameter.Abstract
{
    public interface ICommandGenerate
    {
        string GenerateInsertQuery();
        string GenerateUpdateQuery();
        string GenerateDeleteQuery();
    }
}
