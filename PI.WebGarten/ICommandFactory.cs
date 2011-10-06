using System.Collections.Generic;

namespace PI.WebGarten
{
    public interface ICommandFactory
    {
        IEnumerable<ICommand> Create();
    }
}