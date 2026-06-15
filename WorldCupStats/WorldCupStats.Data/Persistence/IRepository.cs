using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldCupStats.Data.Persistence
{
    public interface IRepository
    {
        // Absolute paths pass through; relative paths go under LocalAppData
        bool Exists(string path);
        void WriteAllText(string path, string content);
        string ReadAllText(string path);
    }
}
