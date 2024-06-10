using System.Threading.Tasks;
using System;
using System.Threading;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Master.Microservice.Models;
using KafkaLib.Kafka;
using Confluent.Kafka;
using System.Text.Json;
using KafkaLib;

namespace Master.Microservice.Handler
{
  
    public class RegionCommandHandler : AsyncRequestHandler<RegionRequest>
    {
        private readonly MasterContext _dbContext;
        private readonly IKafkaMessageBus<string, KafkaMessage<RegionRequest>> _bus;
       
        public RegionCommandHandler(MasterContext dbContext, IKafkaMessageBus<string, KafkaMessage<RegionRequest>> bus)
        {
            _bus = bus;
            _dbContext = dbContext;
         
        }

        protected override async Task Handle(RegionRequest regionRequest, CancellationToken cancellationToken)
        {

            Region instance = _dbContext.Regions.Where(p => p.Id == regionRequest.Id).First();

            var obj = new KafkaMessage<RegionRequest>();
            obj.Type = regionRequest.messageType;
            obj.Message = regionRequest;

            await _bus.PublishAsync(instance.Id.ToString(), obj);
           
        }
    }

 
}
