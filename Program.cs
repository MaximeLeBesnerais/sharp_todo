using System.Text.Json;
using Nancy;
using Nancy.Hosting.Self;
using NDesk.Options;

public class Activity
{
    public string? title { get; set; }
    public string? description { get; set; }
    public string? dueDate { get; set; }
    public bool done { get; set; }
}

public class ActivitiesModule : NancyModule
{
    private List<Activity> _activities;

    public ActivitiesModule() : base("/API")
    {
        _activities = LoadActivitiesFromFile();

        Get("/", _ => "no data");

        Get("/all", _ => Response.AsJson(_activities.Select(a => a.title)));

        Get("/t_search/{title}", parameters =>
        {
            var activities = _activities.Where(a => a.title != null && a.title.Contains((string)parameters.title, StringComparison.OrdinalIgnoreCase));
            return Response.AsJson(activities.Select(a => a.title));
        });

        Get("/d_search/{description}", parameters =>
        {
            var activities = _activities.Where(a => a.description != null && a.description.Contains((string)parameters.description, StringComparison.OrdinalIgnoreCase));
            return Response.AsJson(activities.Select(a => a.title));
        });

        Get("/see/{title}", parameters =>
        {
            var title = ((string)parameters.title).Replace('_', ' ');
            var activity = _activities.FirstOrDefault(a => a.title != null && a.title.Equals(title, StringComparison.OrdinalIgnoreCase));
            return activity != null
                ? Response.AsJson(activity)
                : HttpStatusCode.NotFound;
        });


    }

    private List<Activity> LoadActivitiesFromFile()
    {
        var jsonString = File.ReadAllText("todos.json");
        var activities = JsonSerializer.Deserialize<List<Activity>>(jsonString);
        return activities ?? new List<Activity>();
    }
}


public class Program
{

        static bool showHelp = false;
        static int port = 1234;

    public static void Main(string[] args)
    {
        OptionSet options = CreateOptionSet();
        options.Parse(args);

        if (showHelp) {
            showHelpMethod(options);
            return;
        }

        if (port < 1024 || port > 65535) {
            Console.WriteLine("Invalid port number or root privileges required.");
            Console.WriteLine("Use --help for more information.");
            return;
        }

        var uri = new Uri($"http://localhost:{port}");
        var hostConfiguration = new HostConfiguration {
            UrlReservations = new UrlReservations() { CreateAutomatically = true }
        };

        using (var host = new NancyHost(hostConfiguration, uri)) {
            host.Start();

            Console.WriteLine($"Running on {uri}");
            Console.WriteLine("Press [Enter] to close.");
            Console.ReadLine();
        }
    }

    static OptionSet CreateOptionSet()
    {
        return new OptionSet()
        {
            { "p|port=", "the {PORT} to listen on", (int p) => port = p },
            { "h|help", "show this message and exit", h => showHelp = h != null },
        };
    }

    static void showHelpMethod(OptionSet options) {
        Console.WriteLine("Usage: todos [OPTIONS]");
        Console.WriteLine("Starts a web server on the specified port to interact with the todos API.");
        Console.WriteLine("This program runs only on port equal or greater than 1024 due to permission issues.");
        options.WriteOptionDescriptions(Console.Out);
    }
}
