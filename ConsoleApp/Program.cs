using Common.Services;

namespace SA_TechTest
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var url = "http://www.bom.gov.au/fwo/IDS60901/IDS60901.94672.json";
            //would normally do this with DI
            var service = new MeterologyService();

            var avgTemp = await service.GetAverageTemp(url);

            Console.WriteLine($"Average Air Temperature at Adelaide Airport the last 72h: {avgTemp.ToString("#.#")}°C");
            Console.ReadKey();
        }
    }
}
