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

            if(entry.Detail.AdPictures != null)
            {
                
                foreach (var picture in entry.Detail.AdPictures)
                {
                    //Delete from storage first
                    JObject response = DeleteFromStorage(picture.Uuid);
                    int status = (int)response.Property("status").Value;

                    if(status == 400)
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
    }

}
