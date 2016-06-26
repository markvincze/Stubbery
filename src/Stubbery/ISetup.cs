using Stubbery.RequestMatching;

namespace Stubbery
{
    /// <summary>
    /// Represents a stubbed endpoint on which further conditions or result parameters can be set.
    /// </summary>
    public interface ISetup : IConditionSetup, IResultSetup
    {
    }
}