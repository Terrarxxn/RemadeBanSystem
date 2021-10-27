using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanSystem
{
    public class ExpiredData
    {
        public ExpiredData(bool isExpired, BanInformation ban)
        {
            this.isExpired = isExpired;
            this.ban = ban;
        }
        public bool isExpired;
        public BanInformation ban;
    }
}
