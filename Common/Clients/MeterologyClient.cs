using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Clients
{
    public class MeterologyClient : BaseClient
    {
        public MeterologyClient() : base()
        {

        }
        public MeterologyClient(string apiUrl) : base(apiUrl)
        {
        }
        public async Task<StationWeatherData> GetMeteorologyData(string uri)
        {
            return await CallApi<StationWeatherData>(uri);
        }
    }
}
