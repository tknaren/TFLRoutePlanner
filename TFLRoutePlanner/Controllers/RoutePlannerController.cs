using Microsoft.AspNetCore.Mvc;
using TFLRoutePlanner.Models;
using TFLRoutePlanner.Utilities;
using Microsoft.Extensions.Options;
using TFLRoutePlannerBL;
using TFLRoutePlannerData.Model;
using TFLRoutePlanner.ViewModels;

namespace TFLRoutePlanner.Controllers
{
    public class RoutePlannerController : Controller
    {
        private readonly IOptions<ConfigSettings> _configSettings;
        private readonly IRoutePlannerLogic _routePlannerLogic;
        private readonly ILogger _logger;

        public RoutePlannerController(IOptions<ConfigSettings> settings, ILogger<RoutePlannerController> logger)
        {
            _configSettings = settings;
            _logger = logger;
            _routePlannerLogic = new RoutePlannerLogic(_configSettings.Value);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(RoutePlan plan)
        {
            try
            {
                RouteRequest request = new RouteRequest()
                {
                    source = plan.request.source,
                    destination = plan.request.destination,
                    viaStation = plan.request.viaStation,
                    excludeStation = plan.request.excludeStation
                };

                RouteResponse routeResponse = _routePlannerLogic.GenerateRoute(request);

                plan.response = new RoutePlanResponse()
                {
                    route = routeResponse.route,
                    line = routeResponse.line
                };

                ViewBag.naturalLanguageOutput = ConvertToNaturalLanguage(plan.response);

                
            }
            catch(Exception ex)
            {
                _logger.LogError(string.Format("Exception -- {0} ---- Stack Trace -- {1}", ex.Message,ex.StackTrace));

                ViewBag.exception = "An error occurred. Kindly try with a different source / destination.";
            }

            return View(plan);

        }

        private List<String> ConvertToNaturalLanguage(RoutePlanResponse response)
        {
            List<string> nlResponse = new List<string>();

            if (response.route != null && response.line != null)
            {
                for(int i=0; i < response.route.Count; i++)
                {
                    if (i == 0)
                    {
                        nlResponse.Insert(0, string.Format("Start @  {0}  on  {1} line", response.route[i].ToString(), response.line[i].ToString()));
                    }
                    else
                    {
                        if (i < response.line.Count && !(response.line[i - 1].Contains(response.line[i]) || response.line[i].Contains(response.line[i-1])))
                        {
                            nlResponse.Insert(0, string.Format("Alight @ {0} and Change line from {1} to {2}", response.route[i], response.line[i-1], response.line[i]));
                        }
                        else if (response.route.Count - 1 == i)
                        {
                            nlResponse.Insert(0, string.Format("Alight @ {0}. You have reached your destination. ", response.route[i]));
                        }
                        else
                        {
                            nlResponse.Insert(0, string.Format("..Continue journey towards {0} ", response.route[i]));
                        }
                    }
                }
            }

            nlResponse.Reverse();

            return nlResponse;
        }
    }
}
