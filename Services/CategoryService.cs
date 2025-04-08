using AutoMapper;
using my_cosmetic_store.Dtos.Request;
using my_cosmetic_store.Models;
using my_cosmetic_store.Repository;
using my_cosmetic_store.Utility;
using System.Globalization;
using System.IO;
using System.IO.Pipes;

namespace my_cosmetic_store.Services
{
    public class CategoryService 
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly ApiOptions _apiOptions;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CategoryService(ApiOptions apiOptions, DatabaseContext context,IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _categoryRepository = new CategoryRepository(apiOptions, context, mapper);
            _apiOptions = apiOptions;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        public object CreateNewCategory(CreateNewCategoryRequest request)
        {
            var checkNameCategory = _categoryRepository.FindByCondition(x => x.Name == request.Name).FirstOrDefault();
            if (checkNameCategory != null)
            {
                return null;
            }
            var newCategory = _mapper.Map<Category>(request);
            if (request.ThumbNail != null)
            {
                var fileName = $"{Guid.NewGuid()}-{request.ThumbNail.FileName}";
                using (var filestream = File.Create(Path.Combine(_webHostEnvironment.WebRootPath + "\\images\\brands\\" + fileName)))
                {
                    request.ThumbNail.CopyTo(filestream);
                    filestream.Flush();
                }
                newCategory.thumbNail = "\\images\\brands\\" + fileName;
            }
            _categoryRepository.Create(newCategory);
            _categoryRepository.SaveChange();
            return newCategory;
        }

        public object GetAllCategory()
        {
            try
            {
                return _categoryRepository.FindAll();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public object GetAllCategoryAdmin()
        {
            return _categoryRepository.FindAll().Select(x => new
            {
                id = x.CategoryID,
                name = x.Name,
                description = x.Description,
                thumbnail = x.thumbNail,
            });
        }

        public object GetCategoryByID(int id)
        {
            try
            {
                return _categoryRepository.FindByCondition(x => x.CategoryID == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

       
        public object DeleteCategory(int CategoryID)
        {
            var findCartegory = _categoryRepository.FindByCondition(x => x.CategoryID == CategoryID).FirstOrDefault();
            if (findCartegory != null)
            {
                _categoryRepository.DeleteByEntity(findCartegory);
                _categoryRepository.SaveChange();
                return findCartegory;
            }
            return null;
        }

        public object GetTopCategory(int number)
        {
            var findtopcategory = _categoryRepository.GetTopItems(number).ToList();
            return findtopcategory;
        }

        public object UpdateCategory(UpdateCategoryRequest request)
        {
            var findCategory = _categoryRepository.FindByCondition(x => x.CategoryID == request.CategoryId).FirstOrDefault();
            if (findCategory != null)
            {
                if(request.Description != null)
                {
                    findCategory.Description = request.Description;
                }
                if(request.thumbNail != null)
                {
                    var fileName = request.thumbNail.FileName;
                    Guid guid = Guid.NewGuid();
                    using(var file = File.Create(Path.Combine(_webHostEnvironment.WebRootPath + "/images/" + guid.ToString()+ fileName)))
                    {
                        request.thumbNail.CopyTo(file);
                        file.Flush();
                    }
                    findCategory.thumbNail = "\\images\\" + guid.ToString() + fileName;
                }
                _categoryRepository.UpdateByEntity(findCategory);
                return findCategory;
            }
            return null;
        }
    }
}
