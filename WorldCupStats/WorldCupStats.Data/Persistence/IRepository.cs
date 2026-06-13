using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldCupStats.Data.Persistence
{
    public interface IRepository
    {
        // Path may be absolute (shared user data) or relative to SharedUserDataRoot.
        bool Exists(string path);
        void WriteAllText(string path, string content);
        string ReadAllText(string path);
    }
}
