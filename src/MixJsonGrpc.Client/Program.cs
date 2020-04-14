using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static MixJsonGrpc.WeatherForecasts;

namespace MixJsonGrpc.Client
{
    internal class Program
    {
        private static async Task Main()
        {
            var grpcTsk = WithGrpcAsync();
            var restTsk = WithRestAsync();

            await Task.WhenAll(grpcTsk, restTsk);

            Console.WriteLine("Press a key to exit");
            Console.ReadKey();
        }

        private static async Task WithGrpcAsync()
        {
            // This switch must be set before creating the GrpcChannel/HttpClient.
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            using var channel = GrpcChannel.ForAddress("http://localhost:5001");

            var client = new WeatherForecastsClient(channel);

            var reply = await client.GetWeatherAsync(new Empty());
            Console.WriteLine("From GRPC");
            foreach (var forecast in reply.WeatherData)
            {
                Console.WriteLine($"{forecast.DateTimeStamp.ToDateTime():s} | {forecast.Summary} | {forecast.TemperatureC} C");
            }
        }

        private static async Task WithRestAsync()
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5000");
            var response = await client.GetAsync("/WeatherForecast");
            using var content = await response.Content.ReadAsStreamAsync();
            var replyData = await System.Text.Json.JsonSerializer.DeserializeAsync<WeatherResponse>(content);
            Console.WriteLine("From REST");
            foreach (var forecast in replyData.weatherData)
            {
                Console.WriteLine($"{forecast.dateTimeStamp.ToDateTime():s} | {forecast.summary} | {forecast.temperatureC} C");
            }
        }
    }

    public class WeatherResponse
    {
        public Weatherdata[] weatherData { get; set; }
    }

    public class Weatherdata
    {
        public Timestamp dateTimeStamp { get; set; }
        public int temperatureC { get; set; }
        public int temperatureF { get; set; }
        public string summary { get; set; }
    }

  

}
