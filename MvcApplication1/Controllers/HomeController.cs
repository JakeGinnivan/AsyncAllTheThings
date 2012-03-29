using System;
using System.Threading;
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
    }

    public interface IUserService
    {
        User GetCurrentUser();
        void SendEmail(User user);
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

        public void SendEmail(User user)
        {
            Thread.Sleep(200);
        }
    }

    internal interface IUserRepository
    {
        User GetUser(int currentUserId);
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
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}