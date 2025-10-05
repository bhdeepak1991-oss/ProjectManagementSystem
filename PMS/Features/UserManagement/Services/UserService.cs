
using OtpNet;
using PMS.Features.UserManagement.Respositories;
using PMS.Features.UserManagement.ViewModels;

namespace PMS.Features.UserManagement.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly EmailService _emailService;
        private readonly PasswordService _passwordService;
        private readonly IEmployeeService _employeeService;

        public UserService(IUserRepository userRepository, EmailService emailService, PasswordService passwordService, IEmployeeService employeeService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _passwordService = passwordService;
            _employeeService = employeeService;
        }

        public async Task<Domain.UserManagement> Authenticate(Domain.UserManagement model)
        {
            model.Password = _passwordService.Encrypt(model.Password);
            var responseModel = await _userRepository.Authenticate(model);
            return responseModel;
        }

        public async Task<(string message, bool isSuccess)> CreateUser(Domain.UserManagement model, CancellationToken cancellationToken)
        {
            model.AuthenticatorKey = Base32Encoding.ToString(KeyGeneration.GenerateRandomKey(20));

            var tempPassword= _passwordService.GenerateTempPassword(7);

            model.Password= _passwordService.Encrypt(tempPassword);

            model.IsTempPassword = true;

            var response = await _userRepository.CreateUser(model, default);

            var empDetail = await _employeeService.GetEmployeeById(Convert.ToInt32(model.EmployeeId));

            _emailService.RegistrationEmail(empDetail.model?.EmailId ?? string.Empty, "PMS Registration",$"{model.UserName}", tempPassword);

            return ("User created successfully", true);
           
        }

        public async Task<(string message, bool isSuccess)> DeleteUser(int userId, CancellationToken cancellationToken)
        {
            return await _userRepository.DeleteUser(userId, cancellationToken);
        }

        public async Task<(string message, bool isSuccess, Domain.UserManagement model)> GetUserById(int userId, CancellationToken cancellationToken)
        {
            return await _userRepository.GetUserById(userId, cancellationToken);
        }

        public async Task<(string message, bool isSuccess, IEnumerable<UserViewModel> models)> GetUserList(CancellationToken cancellationToken)
        {
            return await _userRepository.GetUserList( cancellationToken);
        }

        public async Task<(string message, bool isSuccess)> LockedUser(int userId, CancellationToken cancellationToken)
        {
            return await _userRepository.LockedUser(userId,cancellationToken);
        }

        public async Task<(string message, bool isSuccess)> UpdateUser(Domain.UserManagement model, CancellationToken cancellationToken)
        {
            return await _userRepository.UpdateUser(model, cancellationToken);
        }
    }
}
