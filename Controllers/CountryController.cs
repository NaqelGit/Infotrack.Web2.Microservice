using Master.Microservice.Models;

using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using System.Net.Http;
using System.Net;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;
//using Master.Microservice.Migrations;
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Master.Microservice.Handler;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;

namespace Master.Microservice.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly MasterContext _context;
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IMediator _mediator;

        public CountryController(ILogger<WeatherForecastController> logger, IMediator mediator, MasterContext context)
        {
            _logger = logger;
            _context = context;
            _mediator = mediator;
        }

        [HttpGet(Name = "CountryList")]
        public IActionResult CountryList(DataSourceLoadOptions loadOptions)
        {

            var result = DataSourceLoader.Load(_context.Countries, loadOptions);
            return Ok(result);
        }
        [HttpGet(Name = "country-lookup")]
        public async Task<object> CountryLookup(DataSourceLoadOptions loadOptions)
        {
            var source = _context.Countries
                .OrderBy(c => c.Name_en)
                .Select(c => new {
                    Value = c.Id,
                    Text = $"{c.Code}:({c.Name_en})"
                });


            return await DataSourceLoader.LoadAsync(source, loadOptions);
        }

        [HttpGet(Name = "CountryByID")]
        public Country CountryByID(int id)
        {

            return _context.Countries.Where(p => p.Id == id).FirstOrDefault();
        }

        [HttpPost(Name = "CreateCountry")]
        public void CreateCountry(CountryRequest _country)
        {

            var country = new Country
            {
                Code = _country.Code,
                Name_en = _country.Name_en,
                Name_ar = _country.Name_ar,
                CurrencyCode = _context.Currencies.Where(p => p.ID == _country.CurrencyCode).FirstOrDefault(),
                Status = _country.Status,
            };
            _context.Countries.Add(country);


            _context.SaveChanges();

            //Producing
            _country.messageType = KafkaLib.Kafka.KafkaMessageType.Insert;
            _country.Id = country.Id;
            _mediator.Send(_country);

        }

        [HttpPut(Name = "UpdateCountry")]
        public void UpdateCountry(CountryRequest _country)
        {
            var obj = _context.Countries.Where(p => p.Id == _country.Id).FirstOrDefault();
            if (obj != null)
            {
                obj.Code = _country.Code;
                obj.Name_en = _country.Name_en;
                obj.Name_ar = _country.Name_ar;
                obj.CurrencyCode = _context.Currencies.Where(p => p.ID == _country.CurrencyCode).FirstOrDefault();
                obj.Status = _country.Status;

            }

            _context.SaveChanges();
            //Producing
            _country.messageType = KafkaLib.Kafka.KafkaMessageType.Update;
            _mediator.Send(_country);

        }
        



    }
}
