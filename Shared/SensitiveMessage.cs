using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSanitize.Shared
{
    public class SensitiveMessage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Message { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastModifiedDate { get; set; }
    }
}
