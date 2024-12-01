using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Query.AdminData.GetAdminData
{
    public class GetAdminDataQueryResponse
    {
        public int TotalEventCount { get; set; }
        public int UnApprovedEventCount { get; set; }
        public int TotalUser {  get; set; }
        public int UnReadedFeedbacks { get; set; }
    }
}
