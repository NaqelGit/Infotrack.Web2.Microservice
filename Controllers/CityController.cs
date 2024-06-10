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
    public class CityController : ControllerBase
    {
        private readonly MasterContext _context;
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IMediator _mediator;

        public CityController(ILogger<WeatherForecastController> logger, IMediator mediator, MasterContext context)
        {
            _logger = logger;
            _context = context;
            _mediator = mediator;
        }

        [HttpGet(Name = "CityList")]
        public IActionResult CityList(DataSourceLoadOptions loadOptions)
        {

            var result = DataSourceLoader.Load(_context.Cities, loadOptions);
            return Ok(result);
        }
        [HttpGet(Name = "CityByID")]
        public City CityByID(int id)
        {
            return _context.Cities.Where(p => p.Id == id).FirstOrDefault();
        }

        //Create City
        [HttpPost(Name = "CreateCity")]
        public void CreateCity(CityRequest _city)
        {

            var city = new City
            {
                Code = _city.Code,
                Name_en = _city.Name_en,
                Name_ar = _city.Name_ar,
                CountryID = _context.Countries.Where(p => p.Id == _city.CountryID).FirstOrDefault(),
                RegionID = _context.Regions.Where(p => p.Id == _city.RegionID).FirstOrDefault(),
                Status = _city.Status,
            };
            _context.Cities.Add(city);


            _context.SaveChanges();

            //Producing
            _city.messageType = KafkaLib.Kafka.KafkaMessageType.Insert;
            _city.Id = city.Id;
            _mediator.Send(_city);

        }

        [HttpPut(Name = "UpdateCity")]
        public void UpdateCity(CityRequest _city)
        {
            var obj = _context.Cities.Where(p => p.Id == _city.Id).FirstOrDefault();
            if (obj != null)
            {
                obj.Code = _city.Code;
                obj.Name_en = _city.Name_en;
                obj.Name_ar = _city.Name_ar;
                obj.CountryID = _context.Countries.Where(p => p.Id == _city.CountryID).FirstOrDefault();
                obj.RegionID = _context.Regions.Where(p => p.Id == _city.RegionID).FirstOrDefault();
                obj.Status = _city.Status;
            }

            _context.SaveChanges();
            //Producing
            _city.messageType = KafkaLib.Kafka.KafkaMessageType.Update;
            _mediator.Send(_city);

        }
        [HttpDelete(Name = "DeleteCity")]
        public void DeleteCity(CityRequest _city)
        {
            var obj = _context.Cities.Where(p => p.Id == _city.Id).FirstOrDefault();
            if (obj != null)
            {
                obj.Status = 3;
            }
            _context.SaveChanges();

            //Producing
            _city.messageType = KafkaLib.Kafka.KafkaMessageType.Delete;
            _mediator.Send(_city);

        }

    }

}
