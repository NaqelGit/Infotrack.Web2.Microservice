using Master.Microservice.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Master.Microservice.Handler;
using MediatR;
using Newtonsoft.Json;
 
namespace Master.Microservice.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        private readonly MasterContext _context;
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IMediator _mediator;

        public RegionController(ILogger<WeatherForecastController> logger, IMediator mediator, MasterContext context)
        {
            _logger = logger;
            _context = context;
            _mediator= mediator;
        }

        [HttpGet(Name = "regions")]
        public async Task<IActionResult> Regions(DataSourceLoadOptions loadOptions)
        {

            var result = _context.Regions.Select(o => new
            {
                o.Id,
                o.Code,
                o.Name_en,
                o.Name_ar,
                CountryCode=o.CountryID.Code,
                CountryID=o.CountryID.Id,
                o.StatusId
            });
            loadOptions.PrimaryKey = new[] { "Id" };
            loadOptions.PaginateViaPrimaryKey = true;   

            return  Ok(await DataSourceLoader.LoadAsync(result,loadOptions));
        }

        [HttpGet(Name = "region-details")]
        public async Task<IActionResult> RegionDetails(int regionId, DataSourceLoadOptions loadOptions)
        {

            var result = _context.Regions
                .Where (p => p.Id == regionId)
                .Select(o => new
                {
                    o.Id,
                    o.Code,
                    o.Name_en,
                    o.Name_ar,
                    CountryCode = o.CountryID.Code,
                    CountryID = o.CountryID.Id,
                    o.StatusId
                });
           
            return Ok(await DataSourceLoader.LoadAsync(result, loadOptions));
        }

       
        [HttpGet(Name = "region-lookup")]
        public async Task<object> RegionLookup(DataSourceLoadOptions loadOptions)
        {
            var source = _context.Regions
                .OrderBy(c => c.Name_en)
                .Select(c => new {
                    Value = c.Id,
                    Text = $"{c.Code}:({c.Name_en})"
                });


            return await DataSourceLoader.LoadAsync(source, loadOptions);
        }
        
        [HttpPost(Name = "insert-region")]
        public async Task<IActionResult> InsertRegion(RegionRequest values)
        {
            
            var region = new Region();

            try
            {
               
                region.Code = values.Code;
                region.Name_ar = values.Name_ar;
                region.Name_en = values.Name_en;
                region.CountryID = _context.Countries.FirstOrDefault(p => p.Id == values.CountryID);

            }
            catch (Exception ex)
            {
            }
            if (!TryValidateModel(region))
                return BadRequest(ModelState.ToString());
 
            _context.Regions.Add(region);
            await _context.SaveChangesAsync();

            //Producing
            //region.messageType = KafkaLib.Kafka.KafkaMessageType.Insert;
            //region.Id=region.Id;
            //_mediator.Send(_region);

            return Ok(region.Id);

        }

        [HttpPost(Name = "update-region")]
        public async Task<IActionResult> UpdateRegion(int key,string values)
        {
            var region = await _context.Regions.FirstOrDefaultAsync(o=>o.Id==key );
            if(region == null)
                return StatusCode(409,"Region Not Found !");
            
            JsonConvert.PopulateObject(values, region);
            if (!TryValidateModel(region))
                return BadRequest(ModelState.Values.Select(p=>p.Errors).ToList());

           
            await _context.SaveChangesAsync();

            //Producing
            //_region.messageType = KafkaLib.Kafka.KafkaMessageType.Insert;
            //_region.Id=region.Id;
            //_mediator.Send(_region);

            return Ok();

        }
 
        [HttpDelete(Name = "delete-region")]
        public async Task<IActionResult> DeleteRegion(int key)
        {
            var obj =await _context.Regions.FirstOrDefaultAsync(p => p.Id == key);

            if (obj == null)
                return StatusCode(409, " Region Not Found !");
            
            obj.StatusId = 3;
            await _context.SaveChangesAsync();

            //Producing
            //_region.messageType = KafkaLib.Kafka.KafkaMessageType.Delete;
            //_mediator.Send(_region);
            return Ok();
        }



    }
}
