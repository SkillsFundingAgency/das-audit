namespace SFA.DAS.Audit.Application.Mapping
{
    public interface IMapper
    {
        TDest Map<TDest>(object source);
    }
}