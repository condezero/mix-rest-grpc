using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System.Threading.Tasks;

namespace MixJsonGrpc.Services
{
    public interface IWeatherService
    {
        Task GetTownWeatherStream(IAsyncStreamReader<TownWeatherRequest> requestStream, IServerStreamWriter<TownWeatherForecast> responseStream, ServerCallContext context);
        Task<WeatherReply> GetWeather(Empty _, ServerCallContext context);
        Task GetWeatherStream(Empty _, IServerStreamWriter<WeatherData> responseStream, ServerCallContext context);
    }
}