using System;
using KafkaLib.Kafka;
using Master.Microservice.Models;
using MediatR;

namespace Master.Microservice.Handler
{

    public class RegionRequest : IRequest
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name_en { get; set; }
        public string? Name_ar { get; set; }
        public int CountryID { get; set; }
        
        public KafkaMessageType messageType { get; set; }
    }
    public class CountryRequest : IRequest
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name_en { get; set; }
        public string? Name_ar { get; set; }
        public int CurrencyCode { get; set; }
        public int Status { get; set; }

        public KafkaMessageType messageType { get; set; }
    }

    public class CityRequest : IRequest
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name_en { get; set; }
        public string? Name_ar { get; set; }
        public int CountryID { get; set; }
        public int RegionID { get; set; }
        public int? Status { get; set; }
        public KafkaMessageType messageType { get; set; }
    }

}



