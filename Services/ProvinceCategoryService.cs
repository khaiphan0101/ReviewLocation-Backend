using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;
using review.Common.Entities;
using review.Common.ReqModels;
using review.Common.ResModels;
using review.Data;

namespace review.Services
{
    public interface IProvinceCategoryService
    {
        Task<ApiResModel> Add(ProvinceCategoryReqModel req);

        Task<ApiResModel> Update(ProvinceCategoryReqModel req, string id);

        Task<ApiResModel> Delete(string id);

        Task<ApiResModel> GetSelect(string id);

        Task<ApiResModel> GetAll();

        Task<ApiResModel> GetByProvince(string provineId);
    }
    public class ProvinceCategoryService : IProvinceCategoryService
    {
        private readonly DataContext _dataContext;
        private readonly ICloudinaryService _cloudinaryService;

        public ProvinceCategoryService(DataContext dataContext, ICloudinaryService cloudinaryService)
        {
            _dataContext = dataContext;
            _cloudinaryService = cloudinaryService;
        }
        public async Task<ApiResModel> Add(ProvinceCategoryReqModel req)
        {
            try
            {
                //B1: Kiểm tra xem provinceID này có bên Province chưa
                var province = _dataContext.ProvinceEntitys.FirstOrDefault(p => p.ID == req.ProvinceID);
                if(province == null)
                {
                    return new ApiResModel()
                    { StatusCode = 400, MessageText = "This Province is not exits! Please try again!"};
                }
                //B2: nếu có r xét tới tới Name của ProvinceCategory xem có chưa
                var prCateName = _dataContext.ProvinceCategoryEntitys.FirstOrDefault(p => p.Name == req.Name);
                if (prCateName != null)
                {
                    return new ApiResModel()
                    {
                        StatusCode = 400,
                        MessageText = "This ProvinceCategory is exits! Please try again!"
                    };
                }
                
                ImageUploadResult image = new ImageUploadResult();
                image = await _cloudinaryService.UploadImage(req.Thumb);

                ProvinceCategoryEntity data = new ProvinceCategoryEntity()
                {
                    ID = Guid.NewGuid().ToString(),
                    Name = req.Name,
                    Thumb = image.PublicId,
                    Province = province,
                };
                _dataContext.ProvinceCategoryEntitys.Add(data);
                await _dataContext.SaveChangesAsync();
                return new ApiResModel()
                {
                    StatusCode = 200,
                    MessageText = "ProvinceCategory added successfully!",
                    Data = new ProvinceCategoryEntity()
                    {
                        ID = data.ID,
                        Name = data.Name,
                        Thumb = data.Thumb,
                        ProvinceID = data.ProvinceID,
                    }
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
                var provinceCategory = _dataContext.ProvinceCategoryEntitys.FirstOrDefault(p => p.ID == id);
                if (provinceCategory == null)
                {
                    return new ApiResModel()
                    {
                        StatusCode = 400,
                        MessageText = "This ProvinceCategory is not exits!"
                    };
                }
                _dataContext.ProvinceCategoryEntitys.Remove(provinceCategory);
                await _dataContext.SaveChangesAsync();
                return new ApiResModel()
                {
                    StatusCode = 200,
                    MessageText = "Deleted successfully!",
                    Data = provinceCategory
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
                var listProvinceCategory = _dataContext.ProvinceCategoryEntitys.Include(s => s.Province).Where(s => true);

                var list = _dataContext.ProvinceCategoryEntitys.ToList();
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

        public async Task<ApiResModel> GetByProvince(string provineId)
        {
            try
            {
                var provinceCategory = _dataContext.ProvinceCategoryEntitys.Where(p => p.ProvinceID == provineId);
               
                return new ApiResModel()
                {
                    StatusCode = 200,
                    Data = provinceCategory
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
                var provinceCategory = _dataContext.ProvinceCategoryEntitys.FirstOrDefault(p => p.ID == id);
                if (provinceCategory == null)
                {
                    return new ApiResModel()
                    {
                        StatusCode = 400,
                        MessageText = "This ProvinceCategory is not exits!"
                    };
                }
                return new ApiResModel()
                {
                    StatusCode = 200,
                    Data = new ProvinceCategoryEntity()
                    {
                        ID = provinceCategory.ID,
                        Name = provinceCategory.Name,
                        Thumb = provinceCategory.Thumb,
                        ProvinceID = provinceCategory.ProvinceID,
                    }
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

        public async Task<ApiResModel> Update(ProvinceCategoryReqModel req, string id)
        {
            try
            {
                //B1: trước tiên bạn cần nhập provinceID muốn update
                var province = _dataContext.ProvinceEntitys.FirstOrDefault(p => p.ID == req.ProvinceID);
                if (province == null)
                {
                    return new ApiResModel()
                    { StatusCode = 400, MessageText = "Province is not exits! Please try again!" };
                }
                //B2: Có provinceID đúng r thì kiểm tra coi provinceCategoryID có chưa
                var provinceCategory = _dataContext.ProvinceCategoryEntitys.FirstOrDefault(p => p.ID == id);
                if (provinceCategory == null)
                {
                    return new ApiResModel()
                    {
                        StatusCode = 400,
                        MessageText = "This ProvinceCategory is not exits! Please try again!"
                    };
                }
                //B3: nếu cũng có r thì kiểm tra cais province nayf ddax cos category nayf chuaw
                var provinceCategoryName = _dataContext.ProvinceCategoryEntitys.FirstOrDefault(p => p.Name == req.Name && p.ProvinceID == req.ProvinceID);
                if (provinceCategoryName != null)
                {
                    return new ApiResModel()
                    { StatusCode = 400, MessageText = "The Name of Category is exits! Please try again!" };
                }

                ImageUploadResult image = new ImageUploadResult();
                if (req.Thumb is not null)
                {
                    image = await _cloudinaryService.UploadImage(req.Thumb);
                    if (provinceCategory.Thumb != null)
                    {
                        await _cloudinaryService.DeleteImage(provinceCategory.Thumb);
                    }
                }
                provinceCategory.Name = req.Name ;
                provinceCategory.Thumb = image.PublicId;
                provinceCategory.ProvinceID = req.ProvinceID;
                _dataContext.ProvinceCategoryEntitys.Update(provinceCategory);
                await _dataContext.SaveChangesAsync();
                return new ApiResModel()
                {
                    StatusCode = 200,
                    MessageText = "Updated successfully"!,
                    Data = new ProvinceCategoryEntity()
                    {
                        ID = provinceCategory.ID,
                        Name = provinceCategory.Name,
                        Thumb = provinceCategory.Thumb,
                        ProvinceID = provinceCategory.ProvinceID,
                    }
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
