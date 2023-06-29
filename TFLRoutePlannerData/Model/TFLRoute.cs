namespace TFLRoutePlannerData.Model
{
    public class TFLRoutes
    {
        public List<TFLRoute> routes { get; set; }
    }

    public class TFLRoute
    {
        public string start { get; set; }
        public string end { get; set; }
        public double weight { get; set; }
        public string line { get; set; }
    }


}