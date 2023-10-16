using review.Common.Entities;
using review.Common.ReqModels;
using review.Common.ResModels;
using review.Data;

namespace review.Services
{
    public interface IProvinceService
    {
        Task<ApiResModel> Add(ProvinceReqModel req);

        Task<ApiResModel> Update(ProvinceReqModel req, string id);

        Task<ApiResModel> Delete(string id);

        Task<ApiResModel> GetSelect(string id);

        Task<ApiResModel> GetAll();

    }
    public class ProvinceService : IProvinceService
    {
        private readonly DataContext _dataContext;

        public ProvinceService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<ApiResModel> Add(ProvinceReqModel req)
        {
            try
            {            
                var province = _dataContext.ProvinceEntitys.FirstOrDefault(p => p.Name == req.Name);
                if (province != null)
                {
                    return new ApiResModel()
                    {
                        StatusCode = 400,
                        MessageText = "This Province is exits! Please try again other Province!"
                    };
                }
                ProvinceEntity provinceEntity = new ProvinceEntity()
                {
                    ID = Guid.NewGuid().ToString(),
                    Name = req.Name
                };
                _dataContext.ProvinceEntitys.Add(provinceEntity);
                await _dataContext.SaveChangesAsync();
                return new ApiResModel()
                {
                    StatusCode = 200,
                    MessageText = "Province added successfully!",
                    Data = provinceEntity
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
                var province = _dataContext.ProvinceEntitys.FirstOrDefault(p => p.ID == id);
                if (province == null)
                {
                    return new ApiResModel()
                    {
                        StatusCode = 400,
                        MessageText = "This Province is not exits!"
                    };
                }              
                _dataContext.ProvinceEntitys.Remove(province);
                await _dataContext.SaveChangesAsync();
                return new ApiResModel()
                {
                    StatusCode = 200,
                    MessageText = "Deleted successfully!",
                    Data = province
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
                var province = _dataContext.ProvinceEntitys.FirstOrDefault(p => p.ID == id);
                if (province == null)
                {
                    return new ApiResModel()
                    {
                        StatusCode = 400,
                        MessageText = "This Province is not exits!"
                    };
                }
                return new ApiResModel()
                {
                    StatusCode = 200,
                    Data = province
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

        public async Task<ApiResModel> Update(ProvinceReqModel req, string id)
        {
            try
            {
                var province = _dataContext.ProvinceEntitys.FirstOrDefault(p => p.ID == id);
                if (province == null)
                {
                    return new ApiResModel()
                    {
                        StatusCode = 400,
                        MessageText = "This Province is not exits!"
                    };
                }
                var provinceName = _dataContext.ProfileEntitys.FirstOrDefault(p => p.Name == req.Name);
                if (provinceName != null)
                {
                    return new ApiResModel()
                    { StatusCode = 400, MessageText = "This Name of Category is exits! Please try again!" };
                }
                province.Name = req.Name is not null ? req.Name : province.Name;
                _dataContext.ProvinceEntitys.Update(province);
                await _dataContext.SaveChangesAsync();
                return new ApiResModel()
                {
                    StatusCode = 200,
                    MessageText = "Updated successfully"!,
                    Data = province
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
                var listprovince = _dataContext.ProvinceEntitys.Select(l => new ProvinceReqModel()
                {
                    Name = l.Name,
                });

                if (listprovince == null)
                {
                    return new ApiResModel()
                    {
                        StatusCode = 400,
                        MessageText = "List Province is empty!",
                    };
                }
                var list = _dataContext.ProvinceEntitys.ToList();
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
    }
}
