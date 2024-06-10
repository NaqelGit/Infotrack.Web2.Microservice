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

    public class CityCommandHandler : AsyncRequestHandler<CityRequest>
    {
        private readonly MasterContext _dbContext;
        private readonly IKafkaMessageBus<string, KafkaMessage<CityRequest>> _bus;

        public CityCommandHandler(MasterContext dbContext, IKafkaMessageBus<string, KafkaMessage<CityRequest>> bus)
        {
            _bus = bus;
            _dbContext = dbContext;

        }

        protected override async Task Handle(CityRequest cityRequest, CancellationToken cancellationToken)
        {

            City instance = _dbContext.Cities.Where(p => p.Id == cityRequest.Id).First();

            var obj = new KafkaMessage<CityRequest>();
            obj.Type = cityRequest.messageType;
            obj.Message = cityRequest;

            await _bus.PublishAsync(instance.Id.ToString(), obj);

        }
    }


}
