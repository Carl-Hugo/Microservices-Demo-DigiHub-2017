using ForEvolve.Azure.Storage.Table;
using ForEvolve.Azure.Storage.Queue;
using System;
using Microservices.Users.Api.Contracts;
using System.Threading.Tasks;
using AutoMapper;
using ForEvolve.Azure.Storage.Queue.MessageType;

namespace Microservices.Users.Write.Api
{
    public class UserService : IUserService
    {
        private readonly ITableStorageRepository<UserEntity> _userRepository;
        private readonly IUserOperationQueuesService _queuesService;
        private readonly IMapper _mapper;

        public UserService(ITableStorageRepository<UserEntity> userRepository, IUserOperationQueuesService queuesService, IMapper mapper)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _queuesService = queuesService ?? throw new ArgumentNullException(nameof(queuesService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<User> DeleteAsync(string userId)
        {
            // Delete the user
            var removedEntity = await _userRepository.RemoveAsync(UserEntity.DefaultPartitionKey, userId);

            // Queue the "user deleted" message
            await _queuesService.UserDeletedQueue.AddMessageAsync(new TableMessage
            {
                PartitionKey = removedEntity.PartitionKey,
                RowKey = removedEntity.RowKey
            });

            // Return the deleted user
            var removedUser = _mapper.Map<User>(removedEntity);
            return removedUser;
        }

        public async Task<User> InsertAsync(User user)
        {
            // Generate an arbitrary Id if the user id was not provided.
            if (string.IsNullOrWhiteSpace(user.Id))
            {
                user.Id = Guid.NewGuid().ToString();
            }

            // Convert the user to its entity equivalent
            var entity = _mapper.Map<UserEntity>(user);

            // Create the user
            var insertedEntity = await _userRepository.InsertOrReplaceAsync(entity);

            // Queue the "user created" message
            await _queuesService.UserCreatedQueue.AddMessageAsync(new TableMessage
            {
                PartitionKey = insertedEntity.PartitionKey,
                RowKey = insertedEntity.RowKey
            });

            // Return the newly created user
            var insertedUser = _mapper.Map<User>(insertedEntity);
            return insertedUser;
        }

        public async Task<User> UpdateAsync(User user)
        {
            // Convert the user to its entity equivalent
            var entity = _mapper.Map<UserEntity>(user);

            // Update the user
            var updatedEntity = await _userRepository.InsertOrMergeAsync(entity);

            // Queue the "user updated" message
            await _queuesService.UserUpdatedQueue.AddMessageAsync(new TableMessage
            {
                PartitionKey = updatedEntity.PartitionKey,
                RowKey = updatedEntity.RowKey
            });

            // Return the updated user
            var updatedUser = _mapper.Map<User>(updatedEntity);
            return updatedUser;
        }
    }
}