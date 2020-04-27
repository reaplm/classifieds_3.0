using Classifieds.Domain.Data;
using Classifieds.Domain.Model;
using Classifieds.Domain.Uploadcare;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Classifieds.Repository.Impl
{
    public class AdvertRepo : GenericRepo<Advert>, IAdvertRepo
    {
        private ApplicationContext context;

        public AdvertRepo(ApplicationContext context) : base(context)
        {
            this.context = context;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<Advert> FindByCategory(int id)
        {
            var adverts = context.Adverts.Where(x => x.Category.ParentID == id)
                .Include(x => x.Detail);

            return adverts;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<Advert> FindBySubCategory(int id)
        {
            var adverts = context.Adverts.Where(x => x.Category.ID == id)
                .Include(x => x.Detail);

            return adverts;
        }
        public new void Update(Advert advert)
        {

            var entry = context.Adverts.Single(ad => ad.ID == advert.ID);
            context.Entry(entry).Reference(p => p.Detail).Load();

            context.Entry(entry).CurrentValues.SetValues(advert);
            context.Entry(entry.Detail).CurrentValues.SetValues(advert.Detail);

            context.SaveChanges();

        }
        /// <summary>
        /// Remove all pictures belonging to advert with pk {id}
        /// </summary>
        /// <param name="id">Primary key of the advert</param>
        /// <returns></returns>
        public int RemoveAllPictures(long id)
        {
            Uploadcare ucare = new Uploadcare();
            int changed = 0;

            var entry = context.Adverts.Single(ad => ad.ID == id);
            context.Entry(entry).Reference(p => p.Detail).Load();
            context.Entry(entry.Detail).Collection(p => p.AdPictures).Load();

            if (entry.Detail.AdPictures != null)
            {

                foreach (var picture in entry.Detail.AdPictures)
                {
                    //Delete from storage first
                    JObject response = DeleteFromStorage(picture.Uuid);
                    int status = (int)response.Property("status").Value;

                    if (status == 400)
                    {
                        //log this info to delete later
                    }

                    //delete from database
                    context.Entry(picture).State = EntityState.Deleted;

                }
                changed = context.SaveChanges();
            }
            return changed;
        }
        public JObject DeleteFromStorage(string uuid)
        {
            Uploadcare ucare = new Uploadcare();

            return JObject.Parse(ucare.DeleteFile(uuid));
        }
        public JObject DeleteFromStorage(List<string> uuidGroup)
        {
            Uploadcare ucare = new Uploadcare();

            return JObject.Parse(ucare.DeleteBatch(uuidGroup));
        }
        /// <summary>
        /// Count adverts based on status. 
        /// If status is string.Empty then count all adverts
        /// </summary>
        /// <returns>Number of registered users</returns>
        public int CountAdverts(string status)
        {
            if (status == string.Empty)
                return context.Adverts.Count();
            else
                return context.Adverts.Count(x => x.Status == status.ToUpper());
        }
        public int CountAdvertsByUser(long id)
        {
            return context.Adverts.Count(x => x.UserID == id);
        }
        public int CountAdvertsByUserByStatus(long id, string status)
        {
            return context.Adverts.Count(x => x.UserID == id && x.Status == status.ToUpper());
        }
        /// <summary>
        /// Summary of adverts grouped by status
        /// </summary>
        /// <returns>Summary List</returns>
        public List<CountPercentSummary> AdvertCountByStatus()
        {

            var query = context.Adverts.GroupBy(a => new { a.Status })
                        .Select(g => new CountPercentSummary
                        {
                            Column = g.Key.Status,
                            Count = g.Count(),
                            Percent = Math.Round(g.Count() * 100.0 / context.Adverts.Count(),2)
                        })
                        .OrderBy(a => a.Column);
            return query.ToList();
        }
        /// <summary>
        /// Summary of adverts pers advert status for given user
        /// </summary>
        /// <param name="id">PK/ID of the user</param>
        /// <returns></returns>
        public List<CountPercentSummary> AdvertCountByStatusByUser(long id)
        {

            var query = context.Adverts.GroupBy(a => new { a.Status })
                        .Select(g => new CountPercentSummary
                        {
                            Column = g.Key.Status,
                            Count = g.Count(x => x.UserID == id),
                            Percent = Math.Round(g.Count(x => x.UserID == id) * 100.0 / context.Adverts.Count(x => x.UserID == id), 2)
                        })
                        .OrderBy(a => a.Column);
            return query.ToList();
        }
        /// <summary>
        /// Summary of adverts grouped by location
        /// </summary>
        /// <returns>summary list</returns>
        public List<CountPercentSummary> AdvertCountByLocation()
        {

            var query = context.AdvertDetails.GroupBy(a => new { a.Location })
                        .Select(g => new CountPercentSummary
                        {
                            Column = g.Key.Location,
                            Count = g.Count(),
                            Percent = Math.Round(g.Count() * 100.0 / context.AdvertDetails.Count(), 2)
                        })
                        .OrderBy(a => a.Column);
            return query.ToList();
        }
        /// <summary>
        /// Count and percentage of adverts grouped by category
        /// </summary>
        /// <returns>Summary list</returns>
        public List<CountPercentSummary> AdvertCountByCategory()
        {
            //Get a list of adverts and their categories
            var adverts = context.Adverts
                .Join(
                     context.Categories,
                     advert => advert.CategoryID,
                     category => category.ID,
                     (advert, category) => new
                     {
                         AdvertID = advert.ID,
                         CategoryID = category.ID,
                         ParentID = category.ParentID
                     }
                )
                .Join(
                    context.Categories,
                     category => category.ParentID,
                     parent => parent.ID,
                     (category, parent) => new
                     {
                         AdvertID = category.AdvertID,
                         CategoryID = parent.ID,
                         CaregoryName = parent.Name
                     }
                ).ToList();
                           
            //Perform summary
            var query = adverts.GroupBy(x => new { x.CaregoryName})
                        .Select(g => new CountPercentSummary
                        {
                            Column = g.Key.CaregoryName,
                            Count = g.Count(),
                            Percent = Math.Round(g.Count() * 100.0 / context.Adverts.Count(), 2)
                        })
                        .OrderBy(a => a.Column);
            return query.ToList();
        }
    }

}
