using AutoMapper;
using Microservices.Users.Api.Contracts;

namespace Microservices.Users.Write.Api
{
    public class UsersMapperProfile : Profile
    {
        public UsersMapperProfile()
        {
            CreateMap<UserEntity, User>().AfterMap((source, dest) =>
            {
                dest.Id = source.RowKey;
            });
            CreateMap<User, UserEntity>().AfterMap((source, dest) =>
            {
                dest.PartitionKey = UserEntity.DefaultPartitionKey;
                dest.RowKey = source.Id;
            });
        }
    }
}