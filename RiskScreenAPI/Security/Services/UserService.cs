using AutoMapper;
using BCryptNet = BCrypt.Net.BCrypt;

using RiskScreenAPI.Security.Authorization.Handlers.Interfaces;
using RiskScreenAPI.Security.Domain.Models;
using RiskScreenAPI.Security.Domain.Repositories;
using RiskScreenAPI.Security.Domain.Services;
using RiskScreenAPI.Security.Domain.Services.Communication;
using RiskScreenAPI.Security.Exceptions;
using RiskScreenAPI.Shared.Domain.Repositories;

namespace RiskScreenAPI.Security.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly IJwtHandler _jwtHandler;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, IJwtHandler jwtHandler, IMapper mapper)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _jwtHandler = jwtHandler;
        _mapper = mapper;
    }

    public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest request)
    {
        var user = await _userRepository.FindByUsernameAsync(request.Username);
        Console.WriteLine($"Request: {request.Username}, {request.Password}");
        Console.WriteLine($"User: {user.Id}, {user.Username}, {user.PasswordHash}");

        // validate
        if (!BCryptNet.Verify(request.Password,
                user.PasswordHash))
        {
            Console.WriteLine("Authentication Error");
            throw new AppException("Username or password is incorrect");
        }

        Console.WriteLine("Authentication successful. About to generate token");
        // authentication successful
        var response = _mapper.Map<AuthenticateResponse>(user);
        Console.WriteLine($"Response: {response.Id}, , {response.Username}");
        response.Token = _jwtHandler.GenerateToken(user, user.Role);
        Console.WriteLine($"Generated token is {response.Token}");
        return response;
    }

    public async Task<IEnumerable<User>> ListAsync()
    {
        return await _userRepository.ListAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        var user = await _userRepository.FindByIdAsync(id);
        if (user == null) throw new KeyNotFoundException("User not found");
        return user;
    }

    public async Task RegisterAsync(RegisterRequest request)
    {
        // validate
        if (_userRepository.ExistsByUsername(request.Username))
            throw new AppException("Username '" + request.Username + "' is already taken");
        // map model to new user object
        var user = _mapper.Map<User>(request);
        
        // set role to user
        if (user.Role == UserRole.User) 
        {
            user.Role = UserRole.User;
        }
        
        // hash password
        user.PasswordHash = BCryptNet.HashPassword(request.Password);
        // save user
        try
        {
            await _userRepository.AddAsync(user);
            await _unitOfWork.CompleteAsync();
        }
        catch (Exception e)
        {
            throw new AppException($"An error occurred while saving the user: {e.Message}");
        }
    }
    // helper methods
    private User GetById(int id)
    {
        var user = _userRepository.FindById(id);
        if (user == null) throw new KeyNotFoundException("User not found");
        return user;
    }
    public async Task UpdateAsync(int id, UpdateRequest request)
    {
        var user = GetById(id);
        // Validate
        if (_userRepository.ExistsByUsername(request.Username))
            throw new AppException("Username '" + request.Username + "' is already taken");
        // update role if it was entered
        user.Role = request.Role;
        // Hash password if it was entered
        if (!string.IsNullOrEmpty(request.Password))
            user.PasswordHash = BCryptNet.HashPassword(request.Password);
        // Copy model to user and save
        _mapper.Map(request, user);
        try
        {
            _userRepository.Update(user);
            await _unitOfWork.CompleteAsync();
        }
        catch (Exception e)
        {
            throw new AppException($"An error occurred while updating the user: {e.Message}");
        }
    }

    public async Task DeleteAsync(int id)
    {
        var user = GetById(id);
        try
        {
            _userRepository.Remove(user);
            await _unitOfWork.CompleteAsync();
        }
        catch (Exception e)
        {
            throw new AppException($"An error occurred while deleting the user: {e.Message}");
        }

    }
}