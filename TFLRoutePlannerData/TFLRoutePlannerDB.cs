using Newtonsoft.Json;
using TFLRoutePlannerData.Model;
using TFLRoutePlanner.Utilities;

namespace TFLRoutePlannerData
{
    public interface ITFLRoutePlannerDB
    {
        TFLRoutes LoadRoutesFromJson();
    }

    public class TFLRoutePlannerDB : ITFLRoutePlannerDB
    {
        private readonly ConfigSettings _configSettings;

        public TFLRoutePlannerDB(ConfigSettings configSettings)
        {
            _configSettings = configSettings;
        }

        public TFLRoutes LoadRoutesFromJson()
        {
            // Load the route from the json file and return the collection 

            string jsonFilePath = Path.Join(Directory.GetCurrentDirectory(), _configSettings.RouteDataFilePath);

            string jsonString = File.ReadAllText(jsonFilePath);

            // Deserialize the JSON string to your desired object
            TFLRoutes routes = JsonConvert.DeserializeObject<TFLRoutes>(jsonString);

            return routes;
        }
    }
}
