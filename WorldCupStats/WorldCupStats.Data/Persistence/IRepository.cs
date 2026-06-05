using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldCupStats.Data.Persistence
{
    public interface IRepository
    {
        // True when this path points to an existing file.
        bool Exists(string relativePath);
        // Writes UTF-8 text and replaces the file if it already exists.
        void WriteAllText(string relativePath, string content);
        // Reads the whole file as one string.
        string ReadAllText(string relativePath);
    }
}
