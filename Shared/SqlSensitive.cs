using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSanitize.Shared
{
    public class SqlSensitive
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Filter { get; set; }
    }
}
