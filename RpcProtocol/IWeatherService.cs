using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpcProtocol
{
    public struct Weather
    {
        public int Temperature { get; set; }
        public string Description { get; set; }
    }
    public interface IWeatherService
    {
        Weather GetWeather(DateTime day);
    }
}
