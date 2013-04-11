using mywebsite.backend.Entity;

namespace mywebsite.backend.Service
{
    public class LoginResponse
    {
        public bool NewProfileCreated { get; set; }

        public Profile Profile { get; set; }
    }
}
