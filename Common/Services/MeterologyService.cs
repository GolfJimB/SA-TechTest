using Common.Clients;
using Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services
{
    public class MeterologyService
    {
        private MeterologyClient client;
        public MeterologyService()
        {
            //would normally do this with DI
            client = new MeterologyClient();
        }

        /// <summary>
        ///  Calculates the average temperature from this data for the previous 72 hours.
        /// </summary>
        public async Task<double> GetAverageTemp(string url)
        {
            ///Notes:
            ///a more thorough implementation would require taking the timezone into account and 
            ///using the local timezone to calculate the last 72h 
            ///e.g.
            ///TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("Australia/Adelaide");
            //DateTimeOffset currentTimeInSweden = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, tzi);
            //Though I've had trouble with timezone id's being different on different operating systems... requires testing

            var data = await client.GetMeteorologyData(url);

            //Get the most recent data point and then minus 72h
            var lastReadingDateTimeToTake = data.observations.data
                .Where(x => x.TimeOfReading.HasValue)
                .Max(x => x.TimeOfReading)
                ?.AddHours(-72);

            //Return average 
            return data.observations.data
                .Where(x => x.TimeOfReading >= lastReadingDateTimeToTake)
                .Average(x => x.apparent_t);
        }

        public async Task<string> GetStationDatum(string uri, DataTypeEnum dataType)
        {
            var data = await client.GetMeteorologyData(uri);

            switch (dataType)
            {
                case DataTypeEnum.AppTemp:
                    return data.observations.data.OrderByDescending(x => x.TimeOfReading).Select(x => x.apparent_t).ToString();
                case DataTypeEnum.DewPoint:
                    return data.observations.data.OrderByDescending(x => x.TimeOfReading).Select(x => x.dewpt).ToString();
                case DataTypeEnum.Temp:
                    return data.observations.data.OrderByDescending(x => x.TimeOfReading).Select(x => x.air_temp).ToString();
                default:
                    return String.Empty;
            }
        }
    }
}
