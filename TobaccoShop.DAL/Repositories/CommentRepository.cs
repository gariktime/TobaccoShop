using System;
using TobaccoShop.DAL.EF;
using TobaccoShop.DAL.Entities;
using TobaccoShop.DAL.Interfaces;

namespace TobaccoShop.DAL.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private ApplicationContext db;

        public CommentRepository(ApplicationContext context)
        {
            db = context;
        }

        public void Add(Comment item)
        {
            db.Comments.Add(item);
        }

        public void Update(Comment item)
        {
            db.Entry(item).State = System.Data.Entity.EntityState.Modified;
        }

        public void Delete(Guid commentId)
        {
            Comment comment = db.Comments.Find(commentId);
            if (comment != null)
                db.Comments.Remove(comment);
            else
                throw new ArgumentException("Комментарий с указанным Id не найден.");
        }
    }
}
