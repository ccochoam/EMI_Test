using Application.Common;
using Application.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Security;

namespace Application.Services
{
    /// <summary>
    /// CRUD para la clase User. Donde se administran los usuarios 
    /// </summary>
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ServicesResult<List<UserDto>>> GetAllUsersAsync()
        {
            try
            {
                var res = await _userRepository.GetAllAsync();

                var userDto = _mapper.Map<List<UserDto>>(res);
                if (userDto != null)
                    return ServicesResult<List<UserDto>>.SuccessfulOperation(userDto);
                return ServicesResult<List<UserDto>>.FailedOperation(404, "Users not found");

            }
            catch (Exception ex)
            {
                return ServicesResult<List<UserDto>>.FailedOperation(500, "Internal Server Error", ex);
            }
        }

        public async Task<ServicesResult<UserDto>> GetUserByIdAsync(int id)
        {
            try
            {
                var res = await _userRepository.GetByIdAsync(id);
                if (res.Id == 0)
                    return ServicesResult<UserDto>.FailedOperation(404, "User not found");

                var resDto = _mapper.Map<UserDto>(res);
                return ServicesResult<UserDto>.SuccessfulOperation(resDto);
            }
            catch (Exception ex)
            {
                return ServicesResult<UserDto>.FailedOperation(500, "Internal Server Error", ex);
            }
        }

        public async Task<ServicesResult<UserDto>> CreateUserAsync(UserDto userDto)
        {
            try
            {
                var user = _mapper.Map<User>(userDto);
                user.HashKey = SaltGenerator.GenerateSalt();
                user.Password = PasswordHasher.EncryptKey(userDto.GetPassword(), user.HashKey);
                await _userRepository.AddAsync(user);
                if (user.Id == 0)
                    return ServicesResult<UserDto>.FailedOperation(409, "User already exist");

                return ServicesResult<UserDto>.SuccessfulOperation(userDto);
            }
            catch (Exception ex)
            {
                return ServicesResult<UserDto>.FailedOperation(500, "Internal Server Error", ex);
            }
        }

        public async Task<ServicesResult<UserDto>> UpdateUserAsync(int id, UserDto userDto)
        {
            try
            {
                var existingUser = await _userRepository.GetByIdAsync(id);

                if (existingUser == null)
                    return ServicesResult<UserDto>.FailedOperation(404, $"User with Id: {id}, not found");

                existingUser = GetUserModel(userDto, existingUser);
                await _userRepository.UpdateAsync(existingUser);

                var modelDto = _mapper.Map<UserDto>(existingUser);
                if (modelDto != null)
                    return ServicesResult<UserDto>.SuccessfulOperation(modelDto);
                return ServicesResult<UserDto>.FailedOperation(400, "User was not updated");
            }
            catch (Exception ex)
            {
                return ServicesResult<UserDto>.FailedOperation(500, "Error in update: UserService", ex);
            }
        }
        public async Task<ServicesResult<bool>> DeleteUserAsync(int id)
        {
            try
            {
                await _userRepository.DeleteAsync(id);
                return ServicesResult<bool>.SuccessfulOperation(true);
            }
            catch (Exception ex)
            {
                return ServicesResult<bool>.FailedOperation(500, "Error in Delete: UserService", ex);
            }
        }

        private User GetUserModel(UserDto userDto, User existingUser)
        {

            if (!string.IsNullOrEmpty(userDto.Username) && userDto.Username != existingUser.Username)
                existingUser.Username = userDto.Username;
            if (!string.IsNullOrEmpty(userDto.GetPassword()) && userDto.GetPassword() != existingUser.Password)
            {
                existingUser.HashKey = SaltGenerator.GenerateSalt();
                existingUser.Password = PasswordHasher.EncryptKey(userDto.GetPassword(), existingUser.HashKey);
            }
            if (userDto.Role != 0 && userDto.Role != existingUser.Role)
                existingUser.Role = userDto.Role;

            return existingUser;
        }
    }
}
