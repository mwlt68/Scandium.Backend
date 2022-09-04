using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scandium.Model.Settings
{
    public class JwtSettings
    {
        public string? Key { get; set; }
        public int ExpireInDay { get; set; }
    }
}