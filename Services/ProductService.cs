using AutoMapper;
using Microsoft.EntityFrameworkCore;
using my_cosmetic_store.Dtos.Request;
using my_cosmetic_store.Models;
using my_cosmetic_store.Repository;
using my_cosmetic_store.Utility;

namespace my_cosmetic_store.Services
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository;
        private readonly ApiOptions _apiOptions;
        private readonly CategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly BrandRepository _brandRepository;
        private readonly ProductImageRepository _productImageRepository;


        public ProductService(ApiOptions apiOptions, DatabaseContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _productRepository = new ProductRepository(apiOptions, context, mapper);
            _brandRepository = new BrandRepository(apiOptions, context, mapper);
            _categoryRepository = new CategoryRepository(apiOptions, context, mapper);
            _productImageRepository = new ProductImageRepository(apiOptions, context, mapper);
            _apiOptions = apiOptions;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        public object CreateNewProduct(CreateNewProductRequest request)
        {
            try
            {
                var checkBrand = _brandRepository.FindByCondition(x => x.BrandID == request.BrandID).FirstOrDefault();
                var checkCategory = _categoryRepository.FindByCondition(x => x.CategoryID == request.CategoryID).FirstOrDefault();
                if (checkBrand == null || checkCategory == null)
                {
                    throw new Exception("Vui lòng kiểm tra lại giá trị brandId và categoryId");
                }
                var newProduct = new Product
                {
                    ProductName = request.ProductName,
                    BrandID = request.BrandID,
                    CategoryID = request.CategoryID,
                    ProductDescription = request.ProductDescription,
                    ProductStock = request.ProductStock,
                    ProductPrice = request.ProductPrice,
                    ProductDiscount = request.ProductDiscount,
                    ProductImages = new List<Product_Images>()
                };
                _productRepository.Create(newProduct);
                var imageRecord = new List<Product_Images>();
                var imagesFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");
                if (!Directory.Exists(imagesFolder))
                {
                    Directory.CreateDirectory(imagesFolder);
                }
                for (int i = 0; i < request.Files.Count; i++)
                {
                    var file = request.Files[i];
                    var fileExt = Path.GetExtension(file.FileName);
                    var fileName = $"{Guid.NewGuid()}{fileExt}";
                    var filePath = Path.Combine(imagesFolder, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        stream.Flush();
                    }
                    var productImage = new Product_Images
                    {
                        ProductID = newProduct.ProductID,
                        ImageUrl = Path.Combine("images\\products\\", fileName),
                        Is_primary = (i == request.MainImageIndex) ? 1 : 0,
                    };
                    imageRecord.Add(productImage);
                }
                _productImageRepository.AddRangeAsync(imageRecord);
                newProduct.ProductImages = imageRecord;
                return newProduct;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object GetAllProduct()
        {
            var products = _productRepository.FindAll().Include(x => x.ProductImages).Select(x => new
            {
                productID = x.ProductID,
                productName = x.ProductName,
                categoryName = x.Category.Name,
                brandName = x.Brand.Name,
                productDescription = x.ProductDescription,
                productPrice = x.ProductPrice,
                productStock = x.ProductStock,
                productDiscount = x.ProductDiscount,
                productImages = x.ProductImages,
                createdDate = x.CreatedDate.ToString(),
                updatedDate = x.UpdatedDate.ToString(),
            }).ToList();
            return products;
        }
        public object GetProductById(int id)
        {
            var product = _productRepository.FindByCondition(x => x.ProductID == id).Include(x => x.ProductImages).FirstOrDefault();
            return product;
        }

        public object GetNewsetProductTop(int number)
        {
            var products = _productRepository.FindAll().OrderBy(x => x.CreatedDate).Take(number).Include(x => x.ProductImages).ToList();
            return products;
        }

        public object GetProductByCategory(int categoryid)
        {
            var products = _productRepository.FindByCondition(x => x.CategoryID == categoryid).Include(x => x.ProductImages).ToList();
            return products;
        }

        public object GetProductByBrand(int brandID)
        {
            var products = _productRepository.FindByCondition(x => x.BrandID == brandID);
            return products;
        }

        public object SearchProductName(string keyword)
        {
            var products = _productRepository.FindByCondition(x => x.ProductName.Contains(keyword));
            return products;
        }

        public object SearchProductPrice(FindProductPrice findProductPrice)
        {
            if (findProductPrice.BeginPrice == null && findProductPrice.EndPrice == null)
            {
                return _productRepository.FindAll();
            }
            if (findProductPrice.BeginPrice < 0 && findProductPrice.EndPrice < 0)
            {
                return null;
            }
            if (findProductPrice.BeginPrice == null && findProductPrice.EndPrice > 0)
            {
                return _productRepository.FindByCondition(x => x.ProductPrice <= findProductPrice.EndPrice);
            }
            if (findProductPrice.BeginPrice > 0 && findProductPrice.EndPrice == null)
            {
                return _productRepository.FindByCondition(x => x.ProductPrice >= findProductPrice.BeginPrice);
            }
            if (findProductPrice.BeginPrice > 0 && findProductPrice.EndPrice > 0)
            {
                return _productRepository.FindByCondition(x => x.ProductPrice >= findProductPrice.BeginPrice && x.ProductPrice <= findProductPrice.EndPrice);
            }
            return _productRepository.FindAll();

            // phát triển tìm kiếm nâng cao sau: Tìm kiếm kết hợp điều kiện, tìm kiếm theo đánh giá,..
        }

        public object DeleteProduct(int productId)
        {
            var findProduct = _productRepository.FindByCondition(x => x.ProductID == productId).FirstOrDefault();
            if (findProduct != null)
            {
                var productImages = _productImageRepository.FindByCondition(x => x.ProductID == productId);
                foreach (var image in productImages)
                {
                    var filePathDelete = Path.Combine(_webHostEnvironment.WebRootPath, image.ImageUrl);
                    File.Delete(filePathDelete);
                }
                _productRepository.DeleteByEntity(findProduct);
                _productImageRepository.DeleteRange(productImages);
                return findProduct;
            }
            return null;
        }

        public object UpdateProduct(UpdateProductRequest request)
        {
            var findProduct = _productRepository.FindByCondition(x => x.ProductID == request.ProductID).Include(x => x.ProductImages).FirstOrDefault();
            if (findProduct != null)
            {
                findProduct.ProductName = request.ProductName;
                findProduct.ProductPrice = request.ProductPrice;
                findProduct.ProductDescription = request.ProductDescription;
                findProduct.ProductDiscount = request.ProductDiscount;
                findProduct.ProductStock = request.ProductStock;
                findProduct.UpdatedDate = DateTime.UtcNow;
                var listNewImageProducts = new List<Product_Images>();
                if (findProduct.ProductImages != null)
                {
                    listNewImageProducts.AddRange(findProduct.ProductImages);
                }
                if (request.Files != null && request.Files.Count > 0)
                {
                    var imagesFolder = Path.Combine(_webHostEnvironment.WebRootPath + "\\images\\products");
                    for (int i = 0; i < request.Files.Count; i++)
                    {
                        var file = request.Files[i];
                        var fileExt = Path.GetExtension(file.FileName);
                        var fileName = $"{Guid.NewGuid()}{fileExt}";
                        var filePath = Path.Combine(imagesFolder, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                            stream.Flush();
                        }
                        var productImage = new Product_Images
                        {
                            ProductID = findProduct.ProductID,
                            ImageUrl = Path.Combine("images\\products", fileName),
                            Is_primary = 0
                        };
                        listNewImageProducts.Add(productImage);
                        _productImageRepository.Create(productImage);
                    }
                }
                for (int i = 0; i < listNewImageProducts.Count; i++)
                {
                    listNewImageProducts[i].Is_primary = (i == request.MainImageIndex) ? 1 : 0;
                }
                findProduct.ProductImages = listNewImageProducts;
                _productImageRepository.UpdateRange(listNewImageProducts);
                _productRepository.UpdateByEntity(findProduct);
                return findProduct;
            }
            return null;
        }

    }
}
