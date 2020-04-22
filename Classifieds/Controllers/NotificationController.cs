using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Classifieds.Domain.Model;
using Classifieds.Service;
using Classifieds.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Classifieds.Web.Controllers
{
    /// <summary>
    /// Handles notification settings
    /// </summary>
   [Authorize]
    public class NotificationController : Controller
    {
        private IMapper mapper;
        private IUserService userService;
        private INotificationService notificationService;

        public NotificationController(IMapper mapper, IUserService userService, INotificationService notificationService)
        {
            this.mapper = mapper;
            this.userService = userService;
            this.notificationService = notificationService;
        }
        /// <summary>
        /// Add/Remove notification settings
        /// </summary>
        /// <param name="add">true/false</param>
        /// <param name="deviceId">device to set notifications for (email/phone)</param>
        /// <param name="categoryId">notification category to set settings for</param>
        /// <param name="typeId">notification type (sms/pnone)</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Add(bool add, long deviceId, long categoryId, long typeId)
        {
            bool success = false;

            try
            {

                //get loggedin user
                long userId = long.Parse(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);

                Expression<Func<User, object>>[] include = { u => u.Notifications};
                var user = userService.Find(userId, include);

                //add notification
                if (add)
                {
                    if (user.Notifications == null)
                    {
                        user.Notifications = new List<Notification>();
                    }
                    var notification = new Notification
                    {
                        DeviceID = deviceId,
                        NotificationCatID = categoryId,
                        NotificationTypeID = typeId
                    };
                    user.Notifications.Add(notification);
                }
                else //remove notification
                {
                    if (user.Notifications != null)
                    {
                        var item = user.Notifications.FirstOrDefault(
                            x => x.NotificationCatID == categoryId 
                            && x.NotificationTypeID == typeId
                            && x.DeviceID == deviceId
                            && x.UserID == userId);

                        if (item != null)
                        {
                            user.Notifications.Remove(item);

                        }
                    }

                }

                //update user
                userService.Update(user);
                userService.Save();

                //Update Claims
                var claim = new Claim("Notifications", JsonConvert.SerializeObject(user.Notifications, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }));

                //Remove Notifications Claim
                var principal = User;
                var customIdentity = principal.Identities.FirstOrDefault(x => x.Claims.Any(y => y.Type == "Notifications"));
                if (customIdentity != null)
                {
                    customIdentity.RemoveClaim(customIdentity.Claims.First(x => x.Type == "Notifications"));
                    customIdentity.AddClaim(claim);

                }
                var claimsIdentity = new ClaimsIdentity(principal.Claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true
                };
                await HttpContext.SignInAsync(
                           CookieAuthenticationDefaults.AuthenticationScheme,
                           new ClaimsPrincipal(principal), authProperties);

                success = true;
            }
            catch (Exception ex)
            {
                //log error here
                Console.WriteLine(ex.StackTrace);
            }

            if (success) { return new JsonResult("Done!"); }
            else { return new JsonResult("Failed to update notification. Sorry."); }
        }
}

}