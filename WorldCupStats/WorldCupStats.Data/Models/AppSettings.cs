using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldCupStats.Data.Models
{
    public enum ChampionshipType
    {
        Men,
        Women
    }

    public enum DataSourceMode
    {
        Api,
        Json
    }

    public class AppSettings
    {
        public ChampionshipType Championship { get; set; }
        public DataSourceMode DataSource { get; set; }
        public bool IsFullScreen { get; set; }
        public string Resolution { get; set; } = "800x450";
    }
}
