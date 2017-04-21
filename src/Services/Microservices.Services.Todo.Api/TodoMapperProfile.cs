using AutoMapper;
using Microservices.Services.Todo.Api.Contracts;
using Microservices.Services.Todo.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Services.Todo.Api
{
    public class TodoMapperProfile : Profile
    {
        public TodoMapperProfile()
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
            CreateMap<CardData, Card>();
        }
    }
}
