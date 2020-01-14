using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Classifieds.Domain.Model;
using Classifieds.Service;
using Classifieds.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Classifieds.Web.Controllers
{
    public class UserController : Controller
    { 

        private IUserService userService;
        private IMapper mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Delete a user
        /// /User/Delete/id
        /// </summary>
        /// <param name="id">id of the user</param>
        /// <returns></returns>
        public IActionResult Delete(long id)
        {
            int changed = userService.Delete(id);

            if (changed > 0)
                return new JsonResult("Delete Successful!");
            else return new JsonResult("Delete Failed. Sorry.");
        }
        /// <summary>
        /// Return user details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        public IActionResult Detail(long id)
        {
            Expression<Func<User, object>>[] include =
            {
                a => a.UserDetail
            };
            UserViewModel model = mapper.Map<UserViewModel>(userService.Find(id, include));

            return PartialView(model);
        }
        /// <summary>
        /// Allows users to like adverts
        /// </summary>
        /// <param name="id"></param>
        /// <param name="like"></param>
        /// <returns>success/failure</returns>
        [Authorize]
        public bool Like(long id, bool like)
        {
            bool success = false;
            try
            {

                //get loggedin user
                long userId = long.Parse(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);

                Expression<Func<User, object>>[] include = { u => u.Likes };
                var user = userService.Find(userId,include);

                //add like
                if (like)
                {
                    //get advert
                    //var advert = advertService.Find(id);
                    if (user.Likes == null)
                    {
                        user.Likes = new List<Like>();
                    }
                    var newLike = new Like { AdvertID = id };
                    if (!user.Likes.Exists(a => a.AdvertID == newLike.AdvertID))
                    {
                        user.Likes.Add(newLike);
                    }
                    
                }
                else //remove like
                {
                    if (user.Likes != null)
                    {
                        var item = user.Likes.FirstOrDefault(x => x.AdvertID == id);

                        if (item != null)
                        {
                            user.Likes.Remove(item);

                        }
                    }

                }

                //update user
                userService.Update(user);
                userService.Save();

                success = true;
            }
            catch (Exception ex)
            {
                //log error here
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
            }
            return success;

        }
        /// <summary>
        /// Allows user to remove like from favourites
        /// </summary>
        /// <param name="id">the id of the like to remove</param>
        /// <returns>success/failure</returns>
        [Authorize]
        public bool DeleteLike(long id)
        {
            bool success = false;
            try
            {

                //get loggedin user
                long userId = long.Parse(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);

                Expression<Func<User, object>>[] include = { u => u.Likes };
                var user = userService.Find(userId, include);

                if (user.Likes != null)
                {
                    var item = user.Likes.FirstOrDefault(x => x.ID == id);

                    if (item != null)
                    {
                        user.Likes.Remove(item);

                    }
                }

                //update user
                userService.Update(user);
                userService.Save();

                success = true;
            }
            catch (Exception ex)
            {
                //log error here
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
            }
            return success;

        }
    }
}