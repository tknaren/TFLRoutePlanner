using TFLRoutePlanner.Utilities;
using TFLRoutePlannerData;
using TFLRoutePlannerData.Model;

namespace TFLRoutePlannerBL
{
    public interface IRoutePlannerLogic
    {
        RouteResponse GenerateRoute(RouteRequest routePlanRequest);
    }

    public class RoutePlannerLogic : IRoutePlannerLogic
    {

        private Dictionary<string, Dictionary<string, double>> routeGraph;
        private Dictionary<string, Dictionary<string, string>> linesGraph;

        private readonly ConfigSettings _configSettings;
        private readonly ITFLRoutePlannerDB _routesDB;

        public RoutePlannerLogic(ConfigSettings configSettings)
        {
            // Initialize the Dictionaries
            routeGraph = new Dictionary<string, Dictionary<string, double>>();
            linesGraph = new Dictionary<string, Dictionary<string, string>>();

            _configSettings = configSettings;
            _routesDB = new TFLRoutePlannerDB(_configSettings);
        }

        // REPLACE WEIGHT WITH TIME OR DISTANCE TO DETERMINE FASTEST AND SHORTEST

        private void AddRoute(TFLRoute route)
        {
            if (!routeGraph.ContainsKey(route.start))
            {
                routeGraph[route.start] = new Dictionary<string, double>();
            }

            if (!routeGraph.ContainsKey(route.end))
            {
                routeGraph[route.end] = new Dictionary<string, double>();
            }

            routeGraph[route.start][route.end] = route.weight;
            routeGraph[route.end][route.start] = route.weight;


            if (!linesGraph.ContainsKey(route.start))
            {
                linesGraph[route.start] = new Dictionary<string, string>();
            }

            if (!linesGraph.ContainsKey(route.end))
            {
                linesGraph[route.end] = new Dictionary<string, string>();
            }

            linesGraph[route.start][route.end] = route.line;
            linesGraph[route.end][route.start] = route.line;
        }

        private double FindRoute(RouteRequest routePlanRequest, Dictionary<string, string> previousStops)
        {
            Dictionary<string, double> routes = new Dictionary<string, double>();
            Dictionary<string, bool> visited = new Dictionary<string, bool>();


            foreach (var stop in routeGraph.Keys)
            {
                routes[stop] = double.MaxValue;
                visited[stop] = false;
            }

            routes[routePlanRequest.source] = 0;

            while (true)
            {
                string currentStop = null;
                double minDistance = double.MaxValue;

                foreach (var stop in routeGraph.Keys)
                {
                    if (!visited[stop] && routes[stop] < minDistance)
                    {
                        minDistance = routes[stop];
                        currentStop = stop;
                    }
                }

                if (currentStop == null)
                    break;

                visited[currentStop] = true;

                foreach (var neighbor in routeGraph[currentStop])
                {
                    //exclude the station
                    if (!string.IsNullOrEmpty(routePlanRequest.excludeStation) && neighbor.Key == routePlanRequest.excludeStation) continue;

                    double distance = routes[currentStop] + neighbor.Value;
                    if (distance < routes[neighbor.Key])
                    {
                        routes[neighbor.Key] = distance;
                        previousStops[neighbor.Key] = currentStop;
                    }
                }
            }

            return routes[routePlanRequest.destination];
        }

        private void LoadRoutes()
        {
            if (routeGraph.Count > 0)
                return;

            TFLRoutes tflRoutes = _routesDB.LoadRoutesFromJson();

            if (tflRoutes != null)
            {
                foreach (TFLRoute route in tflRoutes.routes)
                {
                    AddRoute(route);
                }
            }

        }

        public RouteResponse GenerateRoute(RouteRequest routePlanRequest)
        {
            RouteResponse response = new RouteResponse();

            Dictionary<string, string> previousStops = new Dictionary<string, string>();

            LoadRoutes();

            FindRoute(routePlanRequest, previousStops);

            List<string> route = new List<string>();
            List<string> line = new List<string>();
            string currentStop = routePlanRequest.destination;

            while (currentStop != routePlanRequest.source)
            {
                route.Insert(0, currentStop);
                currentStop = previousStops[currentStop];
            }

            route.Insert(0, routePlanRequest.source);

            if (routePlanRequest.viaStation != null && !route.Exists(x => x.Equals(routePlanRequest.viaStation)))
            {
                //Add the first stop to the exclusion station to ignore that route
                routePlanRequest.excludeStation = route[1];
                previousStops.Clear();
                FindRoute(routePlanRequest, previousStops);

                route.Clear();
                line.Clear();

                currentStop = routePlanRequest.destination;

                while (currentStop != routePlanRequest.source)
                {
                    route.Insert(0, currentStop);
                    currentStop = previousStops[currentStop];
                }

                route.Insert(0, routePlanRequest.source);
            }

            for (int i = 0; i < route.Count; i++)
            {
                if (i + 1 < route.Count)
                    line.Insert(0, linesGraph[route[i]][route[i + 1]]);
            }

            line.Reverse();

            response.route = route;
            response.line = line;

            return response;
        }

    }
}