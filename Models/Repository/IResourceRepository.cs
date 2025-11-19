using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Repository
{
    public interface IResourceRepository
    {
        IEnumerable<Resource> GetAll();
        int Add(Resource resource);
    }
}
