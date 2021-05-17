using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace PotenciaRadio.Dependencies
{
    public interface IAppService<T>
    {
        Task<IEnumerable<T>> ReadAll();
        Task<T> Read();
        
    }
}
