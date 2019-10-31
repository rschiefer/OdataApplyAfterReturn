using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace OdataApplyAfterReturn.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }

    public class PeopleController : ODataController
    {
        //[EnableQuery]
        public IEnumerable<Person> Get(ODataQueryOptions<Person> options)
        {
            var source = new[]
            {
                new Person { Id = 1, Name = "Bob", Age = 45, Children = new [] {
                    new Person { Id = 2,Name = "Dan", Age = 15}
                }},
                new Person { Id = 3, Name = "Sue", Age = 40}
            };

            var output = source.AsQueryable();
            var settings = new ODataQuerySettings { HandleReferenceNavigationPropertyExpandFilter = false,  };
            output = options.ApplyTo(output, settings, AllowedQueryOptions.Expand).Cast<Person>().AsQueryable();


            return output;
        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public ICollection<Person> Children { get; set; }
    }
}
