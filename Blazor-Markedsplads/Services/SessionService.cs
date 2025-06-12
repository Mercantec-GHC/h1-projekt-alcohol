using Blazor_Markedsplads.Models;

namespace Blazor_Markedsplads.Services
{
    public class SessionService
    {
       
        public CustomerModel? CurrentUser { get; private set; }

        public bool IsLoggedIn => CurrentUser != null;

        public void Login(CustomerModel user)
        {
            CurrentUser = user;
        }

        public void Logout()
        {
            CurrentUser = null;
        }
    }
}