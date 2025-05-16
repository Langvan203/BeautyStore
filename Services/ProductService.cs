using AutoMapper;
using AutoMapper.Internal;
using Microsoft.EntityFrameworkCore;
using my_cosmetic_store.Dtos.Request;
using my_cosmetic_store.Dtos.Response;
using my_cosmetic_store.Models;
using my_cosmetic_store.Repository;
using my_cosmetic_store.Utility;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        private readonly VariantTypeRepository _variantTypeRepository;
        private readonly ProductVariantRepository _productVariantRepository;
        private readonly ProductColorRepository _productColorRepository;
        private readonly ColorRepository _colorRepository;


        public ProductService(ApiOptions apiOptions, DatabaseContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _productRepository = new ProductRepository(apiOptions, context, mapper);
            _brandRepository = new BrandRepository(apiOptions, context, mapper);
            _categoryRepository = new CategoryRepository(apiOptions, context, mapper);
            _productImageRepository = new ProductImageRepository(apiOptions, context, mapper);
            _productVariantRepository = new ProductVariantRepository(apiOptions, context, mapper);
            _colorRepository = new ColorRepository(apiOptions,context, mapper);
            _variantTypeRepository = new VariantTypeRepository(apiOptions, context, mapper);
            _productColorRepository = new ProductColorRepository(apiOptions, context, mapper);
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
                List<VariantTypeDto> variantTypes = new List<VariantTypeDto>();
                List<ColorTypeDto> colors = new List<ColorTypeDto>();
                if (!string.IsNullOrEmpty(request.VariantTypesJson))
                {
                    variantTypes = System.Text.Json.JsonSerializer.Deserialize<List<VariantTypeDto>>(request.VariantTypesJson);
                }
                else
                {
                    variantTypes = new List<VariantTypeDto>(); // Gán mặc định nếu không có dữ liệu
                }
                if(!string.IsNullOrEmpty(request.ColorJson))
                {
                    colors = System.Text.Json.JsonSerializer.Deserialize<List<ColorTypeDto>>(request.ColorJson);
                }
                else
                {
                    colors = new List<ColorTypeDto>(); // Gán mặc định nếu không có dữ liệu
                }
                var requestVariant = variantTypes.Select(x => new
                {
                    variantid = x.VariantID,
                    price = x.VariantPrice,
                    stock = x.VariantStock
                }).ToList();

                var requestColor = colors.Select(x => new
                {
                    colorId = x.ColorID,
                }).ToList();

                var listVariant = _variantTypeRepository.FindAll().Select(x => x.VariantId).ToList();
                var listColor = _colorRepository.FindAll().Select(x => x.ColorID).ToList();

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
                    ProductPrice = request.ProductPrice,
                    ProductDiscount = request.ProductDiscount,
                    ProductIngredient = request.ProductIngredient,
                    ProductUserManual = request.ProductUserManual,
                    ProductImages = new List<Product_Images>(),
                    ProductVariants = new List<ProductVariant>(),
                    ProductColors = new List<ProductColor>()
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
                var listVariantProducts = new List<ProductVariant>();
                var listColorProducts = new List<ProductColor>();


                var listVariantAdd = requestVariant.Select(x => x.variantid).ToList();
                var listColorAdd = requestColor.Select(x => x.colorId).ToList();


                var listPriceVariant = requestVariant.Select(x => x.price).ToList();

                var listStockVariant = requestVariant.Select(x => x.stock).ToList();

                for(int i = 0; i < variantTypes.Count; i++)
                {
                    if (listVariant.Contains(listVariantAdd[i]))
                    {
                        var newVariant = new ProductVariant
                        {
                            ProductID = newProduct.ProductID,
                            VariantId = listVariantAdd[i],
                            PriceOfVariant = listPriceVariant[i],
                            StockOfVariant = listStockVariant[i]
                        };
                        listVariantProducts.Add(newVariant);
                    }    
                }    

                for(int i = 0; i < colors.Count; i++)
                {
                    if (listColor.Contains(listColorAdd[i]))
                    {
                        var newColor = new ProductColor
                        {
                            ProductID = newProduct.ProductID,
                            ColorID = listColorAdd[i],
                        };
                        listColorProducts.Add(newColor);
                    }    
                }    

                _productImageRepository.AddRangeAsync(imageRecord);

                _productVariantRepository.AddRangeAsync(listVariantProducts);
                _productColorRepository.AddRangeAsync(listColorProducts);

                newProduct.ProductImages = imageRecord;
                newProduct.ProductVariants = listVariantProducts;
                newProduct.ProductColors = listColorProducts;
                var product = _productRepository.FindByCondition(x => x.ProductID == newProduct.ProductID).Select(x => new
                {
                    productID = x.ProductID,
                    productName = x.ProductName,
                    categoryName = x.Category.Name,
                    brandName = x.Brand.Name,
                    productDescription = x.ProductDescription,
                    productPrice = x.ProductPrice,
                    productStock = x.ProductStock,
                    productDiscount = x.ProductDiscount,
                    createdDate = x.CreatedDate.ToString(),
                    updatedDate = x.UpdatedDate.ToString(),
                }).FirstOrDefault();
                return product;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object GetAllProduct()
        {
            var products = _productRepository.FindAll().Include(x => x.ProductImages).Include(x => x.ProductVariants).ThenInclude(x => x.Variant).Include(x => x.ProductColors).ThenInclude(x => x.Color).Select(x => new
            {
                productID = x.ProductID,
                productName = x.ProductName,
                categoryName = x.Category.Name,
                brandName = x.Brand.Name,
                productDescription = x.ProductDescription,
                productPrice = x.ProductPrice,
                productStock = x.ProductStock,
                productDiscount = x.ProductDiscount,
                productImages = x.ProductImages.Where(x => x.Is_primary == 1).Select(x => x.ImageUrl).FirstOrDefault(),
                createdDate = x.CreatedDate.ToString(),
                updatedDate = x.UpdatedDate.ToString(),
                variants = x.ProductVariants.Select(findProduct => new
                {
                    variantId = findProduct.Variant.VariantId,
                    variantName = findProduct.Variant.VariantName,
                    price = findProduct.PriceOfVariant,
                    stock = findProduct.StockOfVariant
                }).ToList(),
                colors = x.ProductColors.Select(findProduct => new
                {
                    colorId = findProduct.Color.ColorID,
                    colorName = findProduct.Color.ColorName,
                    colorValue = findProduct.Color.ColorHexaValue,
                    stock = findProduct.StockOfColor
                })
            }).ToList();
            return products;
        }
        public object GetAllProductAdmin()
        {
            var products = _productRepository.FindAll().Include(x => x.Brand).Include(x => x.Category).Select(x => new
            {
                id = x.ProductID,
                name = x.ProductName,
                category = x.Category.Name,
                brand = x.Brand.Name,
                price = x.ProductPrice,
                stock = x.ProductStock,
                discount = x.ProductDiscount,
            }).ToList();
            return products;
        }

        public object GetProductUpdate(int productId)
        {
            try
            {
                var findProduct = _productRepository.FindByCondition(x => x.ProductID == productId)
                    .Include(x => x.ProductImages)
                    .Include(x => x.ProductVariants)
                        .ThenInclude(x => x.Variant)
                    .Include(x => x.ProductColors)
                        .ThenInclude(x => x.Color)
                        .FirstOrDefault();
                if(findProduct != null)
                {
                    var findProductUpdate = new
                    {
                        id = findProduct.ProductID,
                        name = findProduct.ProductName,
                        price = findProduct.ProductPrice,
                        stock = findProduct.ProductStock,
                        discount = findProduct.ProductDiscount,
                        categoryId = findProduct.CategoryID,
                        brandId = findProduct.BrandID,
                        productDescription = findProduct.ProductDescription,
                        ingredient = findProduct.ProductIngredient,
                        userManual = findProduct.ProductUserManual,
                        variants = findProduct.ProductVariants.Select(findProduct => new
                        {
                            id = findProduct.VariantId,
                            variantName = findProduct.Variant.VariantName,
                            variantPrice = findProduct.PriceOfVariant,
                            variantStock = findProduct.StockOfVariant
                        }).ToList(),
                        existingImages = findProduct.ProductImages.Select(x => new
                        {
                            id = x.ImageID,
                            url = x.ImageUrl,
                            isMain = x.Is_primary == 1 ? true : false
                        }).ToList(),
                        colors = findProduct.ProductColors.Select(findProduct => new
                        {
                            colorId = findProduct.Color.ColorID,
                            colorName = findProduct.Color.ColorName,
                            colorCode = findProduct.Color.ColorHexaValue,
                            stock = findProduct.StockOfColor
                        }).ToList(),
                    };
                    return findProductUpdate;
                }
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public object DeleteProductImage(int ProductID, int ImagesID)
        {
            var findProduct = _productImageRepository.FindByCondition(x => x.ProductID == ProductID && x.ImageID == ImagesID).FirstOrDefault();
            if (findProduct != null)
            {
                _productImageRepository.DeleteByEntity(findProduct);
                return findProduct;
            }
            return null;
        }
        public object GetProductById(int id)
        {
            var product = _productRepository.FindByCondition(x => x.ProductID == id).Include(x => x.ProductImages).Include(x => x.ProductVariants).ThenInclude(x => x.Variant).Include(x => x.ProductColors).ThenInclude(x => x.Color).Select(x => new
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
                ingredient = x.ProductIngredient,
                userManual = x.ProductUserManual,
                variants = x.ProductVariants.Select(findProduct => new
                {
                    variantId = findProduct.Variant.VariantId,
                    variantName = findProduct.Variant.VariantName,
                    price = findProduct.PriceOfVariant,
                    stock = findProduct.StockOfVariant
                }).ToList(),
                colors = x.ProductColors.Select(findProduct => new
                {
                    colorId = findProduct.Color.ColorID,
                    colorName = findProduct.Color.ColorName,
                    stock = findProduct.StockOfColor,
                    colorCode = findProduct.Color.ColorHexaValue
                })
            }).FirstOrDefault();
            return product;
        }

        public object GetNewsetProductTop(int number)
        {
            var products = _productRepository.FindAll().OrderBy(x => x.CreatedDate).Take(number).Include(x => x.ProductImages).Include(x => x.ProductVariants).ThenInclude(x => x.Variant).ToList();
            var productDTOs = products.Select(x => new
            {
                productID = x.ProductID,
                productName = x.ProductName,
                productDescription = x.ProductDescription,
                productPrice = x.ProductPrice,
                productStock = x.ProductStock,
                productDiscount = x.ProductDiscount,
                productImages = x.ProductImages.Where(x => x.Is_primary == 1).Select(x => x.ImageUrl).FirstOrDefault(),
                variants = x.ProductVariants.Select(findProduct => new
                {
                    variantId = findProduct.Variant.VariantId,
                    variantName = findProduct.Variant.VariantName,
                    price = findProduct.PriceOfVariant,
                    stock = findProduct.StockOfVariant

                }).ToList(),
            }).ToList();
            return productDTOs;
        }

        public object GetProductByCategory(int categoryid)
        {
            var products = _productRepository.FindByCondition(x => x.CategoryID == categoryid).Include(x => x.ProductImages).Include(x => x.ProductVariants).Select(x => new
            {
                productID = x.ProductID,
                productName = x.ProductName,
                categoryName = x.Category.Name,
                brandName = x.Brand.Name,
                productDescription = x.ProductDescription,
                categoryId = x.CategoryID,
                brandId = x.BrandID,
                productPrice = x.ProductPrice,
                productStock = x.ProductStock,
                productDiscount = x.ProductDiscount,
                productImages = x.ProductImages,
                createdDate = x.CreatedDate.ToString(),
                updatedDate = x.UpdatedDate.ToString(),
            }).ToList();
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
            var findProduct = _productRepository.FindByCondition(x => x.ProductID == request.ProductID).Include(x => x.ProductImages).Include(x => x.ProductVariants).ThenInclude(x => x.Variant).Include(x => x.ProductColors).ThenInclude(x => x.Color).FirstOrDefault();
            #region old version
            //if (findProduct != null)
            //{
            //    var requestVariant = request.VariantID.ToString().Split(',').Select(int.Parse).ToList();
            //    var listVariant = _variantTypeRepository.FindAll().Select(x => x.VariantId).ToList();
            //    findProduct.ProductName = request.ProductName;
            //    findProduct.ProductPrice = request.ProductPrice;
            //    findProduct.ProductDescription = request.ProductDescription;
            //    findProduct.ProductDiscount = request.ProductDiscount;
            //    findProduct.ProductStock = request.ProductStock;
            //    findProduct.UpdatedDate = DateTime.UtcNow;
            //    findProduct.ProductUserManual = request.ProductUserManual;
            //    findProduct.ProductIngredient = request.ProductIngredient;
            //    findProduct.UpdatedDate = DateTime.UtcNow;
            //    var listNewImageProducts = new List<Product_Images>();
            //    if (findProduct.ProductImages != null)
            //    {
            //        listNewImageProducts.AddRange(findProduct.ProductImages);
            //    }
            //    if (request.Files != null && request.Files.Count > 0)
            //    {
            //        var imagesFolder = Path.Combine(_webHostEnvironment.WebRootPath + "\\images\\products");
            //        for (int i = 0; i < request.Files.Count; i++)
            //        {
            //            var file = request.Files[i];
            //            var fileExt = Path.GetExtension(file.FileName);
            //            var fileName = $"{Guid.NewGuid()}{fileExt}";
            //            var filePath = Path.Combine(imagesFolder, fileName);
            //            using (var stream = new FileStream(filePath, FileMode.Create))
            //            {
            //                file.CopyTo(stream);
            //                stream.Flush();
            //            }
            //            var productImage = new Product_Images
            //            {
            //                ProductID = findProduct.ProductID,
            //                ImageUrl = Path.Combine("images\\products", fileName),
            //                Is_primary = 0
            //            };
            //            listNewImageProducts.Add(productImage);
            //            _productImageRepository.Create(productImage);
            //        }
            //    }
            //    for (int i = 0; i < listNewImageProducts.Count; i++)
            //    {
            //        listNewImageProducts[i].Is_primary = (i == request.MainImageIndex) ? 1 : 0;
            //    }
            //    var listVariantProducts = new List<ProductVariant>();
            //    for (int i = 0; i < requestVariant.Count; i++)
            //    {
            //        if (listVariant.Contains(requestVariant[i]))
            //        {
            //            var newVariant = new ProductVariant
            //            {
            //                ProductID = findProduct.ProductID,
            //                VariantId = requestVariant[i],
            //            };
            //            listVariantProducts.Add(newVariant);
            //        }
            //    }
            //    findProduct.ProductImages = listNewImageProducts;
            //    _productImageRepository.UpdateRange(listNewImageProducts);
            //    _productRepository.UpdateByEntity(findProduct);
            //    return findProduct;
            //}
            #endregion
            if (findProduct == null) return null;

            // Cập nhật thông tin sản phẩm
            findProduct.ProductName = request.ProductName;
            findProduct.ProductPrice = request.ProductPrice;
            findProduct.ProductDescription = request.ProductDescription;
            findProduct.ProductDiscount = request.ProductDiscount;
            findProduct.ProductStock = request.ProductStock;
            findProduct.UpdatedDate = DateTime.UtcNow;
            findProduct.ProductUserManual = request.ProductUserManual;
            findProduct.ProductIngredient = request.ProductIngredient;
            var existingImageIdsToKeep = new List<int>();
            if (!string.IsNullOrEmpty(request.ExistingImageIdsToKeep))
            {
                existingImageIdsToKeep = request.ExistingImageIdsToKeep.Split(',')
                    .Select(int.Parse)
                    .ToList();
            }
            var existingImagesToKeep = findProduct.ProductImages
            .Where(img => existingImageIdsToKeep.Contains(img.ImageID))
            .ToList();
            var imagesToDelete = findProduct.ProductImages
            .Where(img => !existingImageIdsToKeep.Contains(img.ImageID))
            .ToList();
            foreach (var img in imagesToDelete)
            {
                _productImageRepository.DeleteByEntity(img);
                //// Xóa file vật lý nếu cần
                //var filePath = Path.Combine(_webHostEnvironment.WebRootPath, img.ImageUrl.TrimStart('/'));
                //if (File.Exists(filePath))
                //{
                //    File.Delete(filePath);
                //}
            }
            var listNewImageProducts = new List<Product_Images>();
            if (request.Files != null && request.Files.Count > 0)
            {
                var imagesFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");
                Directory.CreateDirectory(imagesFolder); // Đảm bảo thư mục tồn tại
                for (int i = 0; i < request.Files.Count; i++)
                {
                    var file = request.Files[i];
                    var fileExt = Path.GetExtension(file.FileName);
                    var fileName = $"{Guid.NewGuid()}{fileExt}";
                    var filePath = Path.Combine(imagesFolder, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    var productImage = new Product_Images
                    {
                        ProductID = findProduct.ProductID,
                        ImageUrl = Path.Combine("images", "products", fileName),
                        Is_primary = 0
                    };
                    listNewImageProducts.Add(productImage);
                    _productImageRepository.Create(productImage);
                }
            }
            var allImages = existingImagesToKeep.Concat(listNewImageProducts).ToList();

            // 5. Cập nhật Is_primary dựa trên MainImageIndex
            for (int i = 0; i < allImages.Count; i++)
            {
                allImages[i].Is_primary = (i == request.MainImageIndex) ? 1 : 0;
            }
            findProduct.ProductImages = allImages;
            _productImageRepository.UpdateRange(allImages);

            // ### Xử lý variants ###


            List<VariantTypeDto> variantTypes = new List<VariantTypeDto>();
            List<ColorTypeDto> colorTypes = new List<ColorTypeDto>();
            if (!string.IsNullOrEmpty(request.VariantID))
            {
                variantTypes = System.Text.Json.JsonSerializer.Deserialize<List<VariantTypeDto>>(request.VariantID);
            }
            else
            {
                variantTypes = new List<VariantTypeDto>(); // Gán mặc định nếu không có dữ liệu
            }
            if (!string.IsNullOrEmpty(request.ColorID))
            {
                colorTypes = System.Text.Json.JsonSerializer.Deserialize<List<ColorTypeDto>>(request.ColorID);
            }
            else
            {
                colorTypes = new List<ColorTypeDto>(); // Gán mặc định nếu không có dữ liệu
            }

            var requestVariant = variantTypes.Select(x => new
            {
                variantid = x.VariantID,
                price = x.VariantPrice,
                stock = x.VariantStock
            }).ToList();
            var requestColor = colorTypes.Select(x => new
            {
                colorId = x.ColorID,
            }).ToList();


            var listKeepvariantId = requestVariant.Select(x => x.variantid).ToList();
            var listKeepColorId = requestColor.Select(x => x.colorId).ToList();


            var listPriceNewAdd = requestVariant.Select(x => x.price).ToList();

            var listStockNewAdd = requestVariant.Select(x => x.stock).ToList();
            var currentVariants = findProduct.ProductVariants.Select(x => x.VariantId).ToList();
            var currentColors = findProduct.ProductColors.Select(x => x.ColorID).ToList();

            // 1. Xóa variants không còn được chọn
            var variantsToRemove = currentVariants.Except(listKeepvariantId).ToList();
            var colorsToRemove = currentColors.Except(listKeepColorId).ToList();

            if (variantsToRemove.Count > 0)
            {
                foreach (var variantId in variantsToRemove)
                {
                    var pv = findProduct.ProductVariants.FirstOrDefault(pv => pv.VariantId == variantId);
                    if (pv != null)
                    {
                        _productVariantRepository.DeleteByEntity(pv);
                    }
                }
            }
            if (colorsToRemove.Count > 0)
            {
                foreach (var colorId in colorsToRemove)
                {
                    var pc = findProduct.ProductColors.FirstOrDefault(pc => pc.ColorID == colorId);
                    if (pc != null)
                    {
                        _productColorRepository.DeleteByEntity(pc);
                    }
                }
            }



            // 2. Thêm variants mới
            var variantsToAdd = listKeepvariantId.Except(currentVariants).ToList();
            if (variantsToAdd.Count > 0)
            {
                for (int i = 0; i < variantsToAdd.Count; i++)
                {
                    var newVariant = new ProductVariant
                    {
                        ProductID = findProduct.ProductID,
                        VariantId = variantsToAdd[i],
                        PriceOfVariant = listPriceNewAdd[i],
                        StockOfVariant = listStockNewAdd[i]
                    };
                    _productVariantRepository.Create(newVariant);
                }
            }
            // 2. Thêm colors mới
            var colorsToAdd = listKeepColorId.Except(currentColors).ToList();
            if (colorsToAdd.Count > 0)
            {
                for (int i = 0; i < colorsToAdd.Count; i++)
                {
                    var newColor = new ProductColor
                    {
                        ProductID = findProduct.ProductID,
                        ColorID = colorsToAdd[i],
                    };
                    _productColorRepository.Create(newColor);
                }
            }




            //3. cập nhật lại giá và stock nếu không có variant mới được thêm hoặc bớt đi
            var currentProductVariant = findProduct.ProductVariants;

            var currentProductColor = findProduct.ProductColors;

            for (int i = 0; i < listKeepvariantId.Count; i++)
            {
                var findUpdateVariant = currentProductVariant.Where(x => x.VariantId == listKeepvariantId[i]).FirstOrDefault();
                if(findUpdateVariant != null)
                {
                   findUpdateVariant.PriceOfVariant = listPriceNewAdd[i];
                    findUpdateVariant.StockOfVariant = listStockNewAdd[i];
                    _productVariantRepository.UpdateByEntity(findUpdateVariant);
                }    
            }


            for (int i = 0; i < listKeepColorId.Count; i++)
            {
                var findUpdateColor = currentProductColor.Where(x => x.ColorID == listKeepColorId[i]).FirstOrDefault();
                if (findUpdateColor != null)
                {
                    _productColorRepository.UpdateByEntity(findUpdateColor);
                }
            }
            // Cập nhật sản phẩm
            _productRepository.UpdateByEntity(findProduct);
            return new
            {
                productID = findProduct.ProductID,
                productName = findProduct.ProductName,
            };
        }

        public object pagnationProductByCondition(int? categoryid,decimal? minPrice, decimal? maxPrice ,int page = 1, int pagesize = 12, string categories = null, string brands = null)
        {
            var products = _productRepository.FindAll().Include(x => x.ProductVariants).ThenInclude(x => x.Variant).AsQueryable();
            if(categoryid != null)
            {
                products = products.Where(p => p.CategoryID == categoryid);
            }
            if (categoryid == null)
            {
                products = _productRepository.FindAll().AsQueryable();
            }
            if (!string.IsNullOrEmpty(categories))
            {
                var categoryList = categories.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
                products = products.Where(p => categories.Contains(p.CategoryID.ToString()));
            }
            if (!string.IsNullOrEmpty(brands))
            {
                var brandList = brands.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
                products = products.Where(p => brandList.Contains(p.BrandID.ToString()));
            }
            if(minPrice != null)
            {
                products = products.Where(p => (p.ProductPrice - (p.ProductPrice * p.ProductDiscount/100)) >= minPrice);
            }
            if (maxPrice != null)
            {
                products = products.Where(p => (p.ProductPrice - (p.ProductPrice * p.ProductDiscount/100)) <= maxPrice);
            }
            var totalItems = products.Count();
            var productsReponse = products.Select(x => new
            {
                productID = x.ProductID,
                productName = x.ProductName,
                categoryName = x.Category.Name,
                brandName = x.Brand.Name,
                productDescription = x.ProductDescription,
                productPrice = x.ProductPrice,
                productStock = x.ProductStock,
                productDiscount = x.ProductDiscount,
                productImages = x.ProductImages.Where(x => x.Is_primary == 1).Select(x => x.ImageUrl).FirstOrDefault(),
                createdDate = x.CreatedDate.ToString(),
                updatedDate = x.UpdatedDate.ToString(),
                variants = x.ProductVariants.Select(findProduct => new
                {
                    variantId = findProduct.Variant.VariantId,
                    variantName = findProduct.Variant.VariantName,
                    price = findProduct.PriceOfVariant,
                    stock = findProduct.StockOfVariant

                }).ToList(),
            }).Skip((page -1) * pagesize).Take(pagesize).ToList();
            return new
            {
                ListProduct = productsReponse,
                TotalItem = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pagesize),
                Page = page,
                PageSize = pagesize,
            };
        }

    }
}
