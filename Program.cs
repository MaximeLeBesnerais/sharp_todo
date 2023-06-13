using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using Nancy;
using System.Linq;
using System;
using Nancy.Hosting.Self;

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
    public static void Main(string[] args)
    {
        var uri = new Uri("http://localhost:1234");
        var hostConfiguration = new HostConfiguration
        {
            UrlReservations = new UrlReservations() { CreateAutomatically = true }
        };

        using (var host = new NancyHost(hostConfiguration, uri))
        {
            host.Start();

            Console.WriteLine("Running on " + uri);
            Console.WriteLine("Press [Enter] to close.");
            Console.ReadLine();
        }
    }
}
