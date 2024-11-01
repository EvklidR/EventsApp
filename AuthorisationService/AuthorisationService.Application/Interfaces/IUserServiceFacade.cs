namespace AuthorisationService.Application.Interfaces
{
    public interface IUserServiceFacade : IAddUser, IGetUserById, IFindUserByCredentials, IUpdateUser
    {
    }

}
