using LayMa.WebApp.Models;

namespace LayMa.WebApp.Service
{
    public static class UserService
    {
        private static User ValidUser = new User() { Name = "huseyin", Password = "password" };
        public static bool IsValid(User user)
        {
            if (user == null) { return false; }
            return ValidUser.Name == user.Name && ValidUser.Password == user.Password;
        }
    }
}
