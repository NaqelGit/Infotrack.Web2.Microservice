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

    public class CountryCommandHandler : AsyncRequestHandler<CountryRequest>
    {
        private readonly MasterContext _dbContext;
        private readonly IKafkaMessageBus<string, KafkaMessage<CountryRequest>> _bus;

        public CountryCommandHandler(MasterContext dbContext, IKafkaMessageBus<string, KafkaMessage<CountryRequest>> bus)
        {
            _bus = bus;
            _dbContext = dbContext;

        }

        protected override async Task Handle(CountryRequest countryRequest, CancellationToken cancellationToken)
        {

            Country instance = _dbContext.Countries.Where(p => p.Id == countryRequest.Id).First();

            var obj = new KafkaMessage<CountryRequest>();
            obj.Type = countryRequest.messageType;
            obj.Message = countryRequest;

            await _bus.PublishAsync(instance.Id.ToString(), obj);

        }
    }


}
