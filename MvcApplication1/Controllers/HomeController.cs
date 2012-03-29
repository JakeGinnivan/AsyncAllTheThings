using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MvcApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService userService;

        public HomeController(IUserService userService)
        {
            this.userService = userService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SendEmail()
        {
            var user = userService.GetCurrentUser();

            userService.SendEmail(user);

            return Content(Guid.NewGuid().ToString());
        }

        public async Task<ActionResult> SendEmailAsync()
        {
            var user = await userService.GetCurrentUserAsync();

            await userService.SendEmailAsync(user);

            return Content(Guid.NewGuid().ToString());
        }
    }

    public interface IUserService
    {
        User GetCurrentUser();
        void SendEmail(User user);
        Task<User> GetCurrentUserAsync();
        Task SendEmailAsync(User user);
    }

    class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public User GetCurrentUser()
        {
            const int currentUserId = 10;
            return userRepository.GetUser(currentUserId);
        }

        public Task<User> GetCurrentUserAsync()
        {
            const int currentUserId = 10;
            return userRepository.GetUserAsync(currentUserId);
        }

        public void SendEmail(User user)
        {
            Thread.Sleep(200);
        }

        public Task SendEmailAsync(User user)
        {
            return Task.Delay(200);
        }
    }

    internal interface IUserRepository
    {
        User GetUser(int currentUserId);
        Task<User> GetUserAsync(int currentUserId);
    }

    class UserRepository : IUserRepository
    {
        public User GetUser(int currentUserId)
        {
            Thread.Sleep(300);

            return new User
            {
                Id = currentUserId,
                Name = Guid.NewGuid().ToString(),
                Email = Guid.NewGuid().ToString()
            };
        }

        public Task<User> GetUserAsync(int currentUserId)
        {
            return Task.Delay(300)
                .ContinueWith(t => new User
                {
                    Id = currentUserId,
                    Name = Guid.NewGuid().ToString(),
                    Email = Guid.NewGuid().ToString()
                });
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}