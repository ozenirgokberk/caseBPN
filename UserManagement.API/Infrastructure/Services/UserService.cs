using AutoMapper;
using UserManagement.API.Core.Application.DTOs.User;
using UserManagement.API.Core.Application.Interfaces;
using UserManagement.API.Core.Domain.Entities;
using UserManagement.API.Infrastructure.Authentication;
using UserManagement.API.Infrastructure.MessageBroker;

namespace UserManagement.API.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly RabbitMQService _rabbitMQService;

    public UserService(IUserRepository userRepository, IMapper mapper, RabbitMQService rabbitMQService)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _rabbitMQService = rabbitMQService;
    }

    public async Task<UserDto> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return _mapper.Map<UserDto>(user);
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }

    public async Task<UserDto> CreateAsync(CreateUserDto createUserDto)
    {
        // Validate username and email uniqueness
        if (await _userRepository.UsernameExistsAsync(createUserDto.Username))
            throw new InvalidOperationException("Username already exists");

        if (await _userRepository.EmailExistsAsync(createUserDto.Email))
            throw new InvalidOperationException("Email already exists");

        // Create user entity and hash password
        var user = _mapper.Map<User>(createUserDto);
        var (hash, salt) = PasswordService.HashPassword(createUserDto.Password);
        user.PasswordHash = hash;
        user.PasswordSalt = salt;

        // Save user
        await _userRepository.CreateAsync(user);
        
        // Publish user created event
        _rabbitMQService.PublishUserCreated(user);
        
        return _mapper.Map<UserDto>(user);
    }

    public async Task UpdateAsync(int id, UpdateUserDto updateUserDto)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new InvalidOperationException("User not found");

        // Check email uniqueness if changed
        if (updateUserDto.Email != user.Email && await _userRepository.EmailExistsAsync(updateUserDto.Email))
            throw new InvalidOperationException("Email already exists");

        _mapper.Map(updateUserDto, user);
        await _userRepository.UpdateAsync(user);
        
        // Publish user updated event
        _rabbitMQService.PublishUserUpdated(user);
    }

    public async Task DeleteAsync(int id)
    {
        await _userRepository.DeleteAsync(id);
    }
} 