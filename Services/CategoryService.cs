using review.Common.Entities;
using review.Common.ReqModels;
using review.Common.ResModels;
using review.Data;

namespace review.Services
{
    public interface ICategoryService
    {
        Task<ApiResModel> Add(CategoryReqModel req);

        Task<ApiResModel> Update(CategoryReqModel req, string id);

        Task<ApiResModel> Delete(string id);

        Task<ApiResModel> GetSelect(string id);

        Task<ApiResModel> GetAll();

    }   
    public class CategoryService : ICategoryService
    {
        private readonly DataContext _dataContext;

        public CategoryService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<ApiResModel> Add(CategoryReqModel req)
        {
            try
            {
                var category = _dataContext.CategoryEntitys.FirstOrDefault(p => p.Name == req.Name);
                if (category != null)
                {
                    return new ApiResModel()
                    {
                        StatusCode = 400,
                        MessageText = "This Category is exits! Please try again other Category!"
                    };
                }
                CategoryEntity cate = new CategoryEntity()
                {
                    ID = Guid.NewGuid().ToString(),
                    Name = req.Name
                };
                _dataContext.CategoryEntitys.Add(cate);
                await _dataContext.SaveChangesAsync();
                return new ApiResModel()
                {
                    StatusCode = 200,
                    MessageText = "Category added successfully!",
                    Data = cate
                };
            }
            catch (Exception ex)
            {
                return new ApiResModel()
                {
                    StatusCode = 400,
                    MessageText = "Some thing went wrong! Please try again!",
                    ErrorText = ex.Message
                };
            }
        }

        public async Task<ApiResModel> Delete(string id)
        {
            try
            {
                var category = _dataContext.CategoryEntitys.FirstOrDefault(p => p.ID == id);
                if (category == null)
                {
                    return new ApiResModel()
                    {
                        StatusCode = 400,
                        MessageText = "This Category is not exits!"
                    };
                }
                _dataContext.CategoryEntitys.Remove(category);
                await _dataContext.SaveChangesAsync();
                return new ApiResModel()
                {
                    StatusCode = 200,
                    MessageText = "Deleted successfully!",
                    Data = category
                };
            }
            catch (Exception ex)
            {
                return new ApiResModel()
                {
                    StatusCode = 400,
                    MessageText = "Some thing went wrong! Please try again!",
                    ErrorText = ex.Message
                };
            }
        }

        public async Task<ApiResModel> GetAll()
        {            
            try
            {
                var listcategory = _dataContext.CategoryEntitys.Select(l => new CategoryReqModel()
                {
                    Name = l.Name,
                });
                               
                if (listcategory == null)
                {
                    return new ApiResModel()
                    {
                        StatusCode = 400,
                        MessageText = "List Category is empty!",                      
                    };
                }
                var list = _dataContext.CategoryEntitys.ToList();
                return new ApiResModel()
                {
                    StatusCode = 200,
                    Data = list
                };
            }
            catch (Exception ex)
            {
                return new ApiResModel()
                {
                    StatusCode = 400,
                    MessageText = "Some thing went wrong! Please try again!",
                    ErrorText = ex.Message
                };
            }
        }

        public async Task<ApiResModel> GetSelect(string id)
        {
            try
            {
                var category = _dataContext.CategoryEntitys.FirstOrDefault(p => p.ID == id);
                if (category == null)
                {
                    return new ApiResModel()
                    {
                        StatusCode = 400,
                        MessageText = "This Category is not exits!"
                    };
                }
                return new ApiResModel()
                {
                    StatusCode = 200,
                    Data = category
                };
            }
            catch (Exception ex)
            {
                return new ApiResModel()
                {
                    StatusCode = 400,
                    MessageText = "Some thing went wrong! Please try again!",
                    ErrorText = ex.Message
                };
            }
        }

        public async Task<ApiResModel> Update(CategoryReqModel req, string id)
        {
            try
            {
                var category = _dataContext.CategoryEntitys.FirstOrDefault(p => p.ID == id);
                
                if (category == null)
                {
                    return new ApiResModel()
                    { StatusCode = 400, MessageText = "This Category is not exits!"};
                }
                var categoryName = _dataContext.CategoryEntitys.FirstOrDefault(p => p.Name == req.Name);
                if (categoryName != null)
                {
                    return new ApiResModel()
                    { StatusCode = 400, MessageText = "This Name of Category is exits! Please try again!"};
                }
                //category.ID = req.ID;
                category.Name = req.Name is not null ? req.Name : category.Name;
                _dataContext.CategoryEntitys.Update(category);
                await _dataContext.SaveChangesAsync();
                return new ApiResModel()
                {
                    StatusCode = 200,
                    MessageText = "Updated successfully!",
                    Data = category
                };
            }
            catch (Exception ex)
            {
                return new ApiResModel()
                {
                    StatusCode = 400,
                    MessageText = "Some thing went wrong! Please try again!",
                    ErrorText = ex.Message
                };
            }
        }
    }
}
