using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TobaccoShop.DAL.Entities;

namespace TobaccoShop.DAL.Interfaces
{
    public interface ICommentRepository
    {
        void Add(Comment comment);
        void Update(Comment comment);
        void Delete(Guid commentId);
    }
}
