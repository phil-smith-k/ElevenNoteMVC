using ElevenNote.Data;
using ElevenNote.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Services
{
    public class CategoryService
    {
        private Guid _userId;

        public CategoryService(Guid userId)
        {
            _userId = userId;
        }

        //CREATE___________________________________________
        public bool CreateCategory(CategoryCreate model)
        {
            var entity =
                new Category()
                {
                    OwnerId = _userId,
                    Name = model.Name
                };

            using (var ctx = new ApplicationDbContext())
            {
                ctx.Categories.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }
        //READ_____________________________________________
        public IEnumerable<CategoryList> GetCategories()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .Categories
                        .Where(e => e.OwnerId == _userId)
                        .Select(e =>
                            new CategoryList
                            {
                                CategoryId = e.CategoryId,
                                Name = e.Name
                            });
                return query.ToArray();
            }
        }
        public CategoryList GetCategoryById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Categories
                        .FirstOrDefault(cat => cat.CategoryId == id && cat.OwnerId == _userId);

                return new CategoryList
                {
                    CategoryId = entity.CategoryId,
                    Name = entity.Name
                };
            }
        }
    }
}
