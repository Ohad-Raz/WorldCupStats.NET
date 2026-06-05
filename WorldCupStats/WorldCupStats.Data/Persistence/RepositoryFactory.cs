using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldCupStats.Data.Persistence
{
    public static class RepositoryFactory
    {
        private static IRepository? repo;

        // Single shared file repository for the whole app.
        public static IRepository GetInstance()
        {
            repo ??= new FileRepository();           
            return repo;
        }
    }
}
