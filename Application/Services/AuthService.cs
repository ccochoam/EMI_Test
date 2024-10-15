using Domain.Interfaces;
using Application.Common;
using Domain.Entities;
using Infrastructure.Security;

namespace Application.Services
{
    public class AuthService
    {
        private readonly IAuthRepository _authRepository;
        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        //servicio creado para la autenticación del usuario. Valida que el usuario exista en base de datos y luego compara la contraseña que recibió contra la de la base de datos
        public async Task<ServicesResult<User>> Authenticate(string userName, string password)
        {
            try
            {
                var res = await _authRepository.Authenticate(userName);
                if (res == null)
                    return ServicesResult<User>.FailedOperation(404, "User not found");

                bool valitePass = ValidatePass.ValidatePassword(password, res.Password, res.HashKey);
                if (valitePass)
                {
                    return ServicesResult<User>.SuccessfulOperation(res);
                }
                return ServicesResult<User>.FailedOperation(401, "unauthorized");
            }
            catch (Exception ex)
            {
                return ServicesResult<User>.FailedOperation(500, "Error in Authenticate", ex);
            }
        }
    }
}
