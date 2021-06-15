using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using u_market.DAL;
using u_market.Models;

namespace u_market.Controllers.Tags
{
    public class TagLogic
    {
        private MarketContext Ctx { get; }
        public TagLogic(MarketContext Ctx)
        {
            this.Ctx = Ctx;
        }

        public IList<Tag> GetAll()
        {
            return this.Ctx.Tags.OrderBy(t => t.Id).ToList();
        }

        public void DeleteById(int tagId)
        {
            this.Ctx.Tags.Remove(this.Ctx.Tags.Find(tagId));
            this.Ctx.SaveChanges();
        }

        public void UpdateTag(Tag tag)
        {
            this.Ctx.Tags.Update(tag);
            this.Ctx.SaveChanges();
        }
    }
}
