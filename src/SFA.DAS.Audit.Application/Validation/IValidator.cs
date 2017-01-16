using System.Threading.Tasks;

namespace SFA.DAS.Audit.Application.Validation
{
    public interface IValidator<T>
    {
        Task<ValidationResult> ValidateAsync(T message);
    }
}
