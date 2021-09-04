using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using u_market.DAL;
using u_market.Exceptions;
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

        public IList<Tag> GetAll(string? filter)
        {
            var query = this.Ctx.Tags.AsQueryable();

            if (filter != null)
            {
                query = query.Where(t => t.Name.Contains(filter));
            }

            return query.OrderBy(t => t.Id).ToList();
        }

        public void DeleteById(int tagId)
        {
            this.Ctx.Tags.Remove(this.Ctx.Tags.Find(tagId));
            this.Ctx.SaveChanges();
        }

        public void UpdateTag(Tag tag)
        {
            EnsureTag(tag);
            this.Ctx.Tags.Update(tag);
            this.Ctx.SaveChanges();
        }

        public void AddTag(Tag tag)
        {
            EnsureTag(tag);
            this.Ctx.Tags.Add(tag);
            this.Ctx.SaveChanges();
        }

        private void EnsureTag(Tag tag)
        {
            if (string.IsNullOrWhiteSpace(tag.Name))
            {
                throw new ModelValidationException("Tag name cannot be empty");
            }
            else if(tag.Name.Length > 15)
            {
                throw new ModelValidationException("Tag name cannot be more than 15 sign");
            }
        }
    }
}
