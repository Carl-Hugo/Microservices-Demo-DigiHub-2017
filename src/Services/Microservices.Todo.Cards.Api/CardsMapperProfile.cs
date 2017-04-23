using AutoMapper;
using Microservices.Todo.Cards.Api.Contracts;
using Microservices.Todo.Cards.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Todo.Cards.Api
{
    public class CardsMapperProfile : Profile
    {
        public CardsMapperProfile()
        {
            CreateMap<Card, CardEntity>().AfterMap((source, dest) =>
            {
                dest.PartitionKey = source.BoardId;
                dest.RowKey = source.Id;
            });
            CreateMap<CardEntity, Card>().AfterMap((source, dest) =>
            {
                dest.BoardId = source.PartitionKey;
                dest.Id = source.RowKey;
            });
        }
    }
}
