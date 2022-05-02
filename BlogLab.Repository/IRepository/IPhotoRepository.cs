using BlogLab.Models.Photo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogLab.Repository.IRepository
{
    public interface IPhotoRepository
    {
        public Task<Photo> InsertAsync(PhotoCreate photoCreate, int applicationUserId);
        public Task<Photo> GetAsync(int photoid);
        public Task<List<Photo>> GetAllByUserIdAsync(int applicationUserId);
        public Task<int> DeleteAsync(int photoid);
    }
}
