namespace TFLRoutePlanner.ViewModels
{
    public class RoutePlanResponse
    {
        public List<string> route { get; set; }
        public List<string> line { get; set; }
        public List<string> naturalLanguageOutput { get; set; }
    }
}
