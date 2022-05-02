using BlogLab.Models.BlogComment;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogLab.Repository.IRepository
{
    public interface IBlogCommentRepository
    {
        public Task<BlogComment> UpsertAsync(BlogCommentCreate blogCommentCreate, int applicationUserId);
        public Task<List<BlogComment>> GetAllAsync(int blogId);
        public Task<BlogComment> GetAsync(int blogCommentId);
        public Task<int> DeleteAsync(int blogCommentId);
    }
}
