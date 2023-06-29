namespace TFLRoutePlanner.ViewModels
{
    public class RoutePlan
    {
        public List<string>? availableStations { get; set; }
        public RoutePlanRequest? request { get; set; }
        public RoutePlanResponse? response { get; set; }
    }
}
