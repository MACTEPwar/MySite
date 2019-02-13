using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySite.Models;
using MySite.Data;
using Microsoft.EntityFrameworkCore;
using System.Web.Http;
using System.Net;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace MySite.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private DbProductsAndGroupsContext _productContext;
        private IHostingEnvironment _env;

        public ProductsController(DbProductsAndGroupsContext productContext, IHostingEnvironment env)
        {
            _productContext = productContext;
            _env = env;
        }

        //получить url картинки по ее id
        public string GetUrlImage(int id)
        {
            var url = _productContext.Images.FirstOrDefault(x => x.Id == id);
            if (url != null) return $"https://{HttpContext.Request.Host.Value}/ProductImages/{url.ImageName}.{url.ImageType}";
            return null;

        }

        //получить все категории
        [Route("Category")]
        [HttpGet]
        public Object GetCategory()
        {
            return _productContext.categories.Join(_productContext.Images, c => c.ImageId, i => i.Id, (c, i) => new
            {
                Id = c.Id,
                Title = c.Title,
                Discription = c.Discription,
                Image = $"https://{HttpContext.Request.Host.Value}/ProductImages/{i.ImageName}.{i.ImageType}",
                ImageId = i.Id
                //Image = $"<img style='max-width:200;max-height:200px;' class='imgCat img-rounded' value='{i.Id}' src='https://{HttpContext.Request.Host.Value}/ProductImages/{i.ImageName}.{i.ImageType}' />"
            });
        }

        //получить категорию по id
        [Route("Category/{id}")]
        [HttpGet]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            return await _productContext.categories.FirstAsync(x => x.Id == id);
        }

        //получить все подкатегорию
        [Route("Group")]
        [HttpGet]
        public object GetGroups()
        {
            return _productContext.groups.Join(_productContext.categories, g => g.CategoryId, c => c.Id, (g, c) => new {
                Id = g.Id,
                Title = g.Title,
                Discription = g.Discription,
                CategoryId = g.CategoryId,
                CategoryTitle = c.Title,
                CategoryList = this.GetCategory()
            });
        }

        //получить подкатегории по id
        [Route("Group/{id}")]
        [HttpGet]
        public async Task<ActionResult<Groups>> GetGroups(int id)
        {
            return await _productContext.groups.FirstAsync(x => x.Id == id);
        }

        /// <summary>
        /// Создает новую категорию
        /// 
        /// api/Products/Create/Category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [Route("Category/Create")]
        [HttpPost]
        public async Task<ActionResult> Post(Category category)
        {
            if (!ModelState.IsValid) return BadRequest("Not valid category model");
            try
            {
                _productContext.Add(category);
                await _productContext.SaveChangesAsync();
            }
            catch
            {
                return BadRequest("Error");
            }
            return Ok();
        }


        /// <summary>
        /// Создает новую группу
        /// 
        /// api/Products/Create/Group
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        [Route("Group/Create")]
        [HttpPost]
        public async Task<ActionResult> Post(Groups group)
        {
            if (!ModelState.IsValid) return BadRequest("Not valid group model");
            try
            {
                _productContext.Add(group);
                await _productContext.SaveChangesAsync();
            }
            catch
            {
                return BadRequest("Error");
            }
            return Ok();
        }

        /// <summary>
        /// Добавляет один продукт
        /// api/Products/Create/Product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        //[Route("Product/Create")]
        //[HttpPost]
        //public async Task<ActionResult> Post(Products product)
        //{
        //    if (!ModelState.IsValid) return BadRequest("Not valid product model");
        //    try
        //    {
        //        _productContext.Add(product);
        //        await _productContext.SaveChangesAsync();
        //    }
        //    catch
        //    {
        //        return BadRequest("Error");
        //    }
        //    return Ok();
        //}


        [Route("Product/Create")]
        [HttpPost]
        public async Task<ActionResult> Post(Products product)
        {
            return Ok();
        }

        /// <summary>
        /// Добавляет несколько продуктов
        /// 
        /// api/Products/Create/Product
        /// 
        /// [
        ///    {
        ///    	"Title":"1",
        ///		"Unit":"1",
        ///		"Count":1,
        ///		"Price":1,
        ///		"Discount":1
        ///    },
        ///	   {
        ///	   	"Title":"2",
        ///	   	"Unit":"2",
        ///	   	"Count":2,
        ///	   	"Price":2,
        ///	   	"Discount":2
        ///	   }
        /// ]
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        [Route("Products/Create")]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody]Products[] products)
        {
            try
            {
                foreach (Products p in products)
                {
                    _productContext.Add(p);
                }

                await _productContext.SaveChangesAsync();
            }
            catch
            {
                return BadRequest("Error");
            }
            return Ok();
        }

        /// <summary>
        /// Вытягивает информацию о продукте по фильтру
        /// </summary>
        /// <param name="title"></param>
        /// <param name="group"></param>
        /// <param name="category"></param>
        /// <param name="priceMin"></param>
        /// <param name="priceMax"></param>
        /// <param name="countMin"></param>
        /// <param name="countMax"></param>
        /// <param name="discountMin"></param>
        /// <param name="discountMax"></param>
        /// <returns></returns>
        [Route("GetWithFilter")]
        [HttpGet]
        public Object Get(string title, string group = null, string category = null, int? priceMin = null, int? priceMax = null, int? countMin = null, int? countMax = null, int? discountMin = null, int? discountMax = null)
        {
            return _productContext.gropsAndProducts
           .Join(_productContext.groups, gap => gap.GroupId, g => g.Id, (gap, g) =>
           new
           {
               Id = gap.Id,
               ProductId = gap.ProductId,
               GroupId = g.Id,
               GroupTitle = g.Title,
               GroupDiscription = g.Discription,
               CategoryId = g.CategoryId
           })
           .Where(x => x.GroupTitle == group || group == null)
           .Join(_productContext.categories, g => g.CategoryId, c => c.Id, (g, c) =>
           new
           {
               Id = g.Id,
               ProductId = g.ProductId,
               GroupId = g.GroupId,
               GroupTitle = g.GroupTitle,
               GroupDiscription = g.GroupDiscription,
               CategoryId = c.Id,
               CategoryTitle = c.Title,
               CategoryDiscription = c.Discription
           })
           .Where(x => x.CategoryTitle == category || category == null)
           .Join(_productContext.products, c => c.ProductId, p => p.Id, (c, p) =>
           new
           {
               Id = c.Id,
               ProductTitle = p.Title,
               ProductDiscription = p.Discription,
               Count = p.Count,
               Price = p.Price,
               Unit = p.Unit,
               Discount = p.Discount,
               GroupId = c.GroupId,
               GroupTitle = c.GroupTitle,
               GroupDiscription = c.GroupDiscription,
               CategoryId = c.CategoryId,
               CategoryTitle = c.CategoryTitle,
               CategoryDiscription = c.CategoryDiscription,
               ImageId = p.ImageId
           })
           .Where(x => x.ProductTitle.Contains(title) || title == null)
           .Where(x => x.Price >= priceMin || priceMin == null)
           .Where(x => x.Price <= priceMax || priceMax == null)
           .Where(x => x.Count >= countMin || countMin == null)
           .Where(x => x.Count <= countMax || countMax == null)
           .Join(_productContext.Images, p => p.ImageId, i => i.Id, (p, i) => new
           {
               Id = p.Id,
               ProductTitle = p.ProductTitle,
               ProductDiscription = p.ProductDiscription,
               Count = p.Count,
               Price = p.Price,
               Unit = p.Unit,
               Discount = p.Discount,
               GroupId = p.GroupId,
               GroupTitle = p.GroupTitle,
               GroupDiscription = p.GroupDiscription,
               CategoryId = p.CategoryId,
               CategoryTitle = p.CategoryTitle,
               CategoryDiscription = p.CategoryDiscription,
               ImageId = p.ImageId,
               ImagePath = $"https://{HttpContext.Request.Host.Value}/ProductImages/{i.ImageName}.{i.ImageType}"
           })
           ;
        }

        //Получить товар по id
        [HttpGet("Product/{id}")]
        public async Task<ActionResult<Products>> Get(int id)
        {
            try
            {
                return await _productContext.products.FirstAsync(s => s.Id == id);
            }
            catch
            {
                return BadRequest("Not valid product id");
            }
        }

        //получить весь товар
        [HttpGet("Products")]
        public async Task<ActionResult<List<Products>>> GetAllProducts()
        {
            return await _productContext.products.ToListAsync();
        }

        //Изменить товар по id
        [HttpPut("Product/{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Products product)
        {
            var searchProduct = _productContext.products.First(s => s.Id == id);
            if (searchProduct != null)
            {
                searchProduct.Title = product.Title;
                searchProduct.Unit = product.Unit;
                searchProduct.Count = product.Count;
                searchProduct.Price = product.Price;
                searchProduct.Discount = product.Discount;
                searchProduct.Discription = product.Discription;
                //searchProduct.Images = product.Images;
                try
                {
                    await _productContext.SaveChangesAsync();
                    return Ok();
                }
                catch
                {
                    return BadRequest("BadRequest");
                }
            }
            else return BadRequest("BadRequest");
        }

        //Изменить группу по id
        [HttpPut("Group/Update/{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Groups group)
        {
            var searchGroup = _productContext.groups.First(s => s.Id == id);
            if (searchGroup != null)
            {
                searchGroup.Title = group.Title;
                searchGroup.Discription = group.Discription;
                searchGroup.CategoryId = group.CategoryId;
                try
                {
                    await _productContext.SaveChangesAsync();
                    return Ok();
                }
                catch
                {
                    return BadRequest("BadRequest");
                }
            }
            else return BadRequest("BadRequest");
        }

        //Изменить категорию по id
        [HttpPut("Category/Update/{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Category category)
        {
            var searchCategory = _productContext.categories.First(s => s.Id == id);
            if (searchCategory != null)
            {
                searchCategory.Title = category.Title;
                searchCategory.Discription = category.Discription;
                


                if(searchCategory.ImageId != category.ImageId)
                {
                    var img = _productContext.Images.First(x => x.Id == (_productContext.categories.First(c => c.Id == id)).ImageId);
                    System.IO.File.Delete($"{_env.WebRootPath}/ProductImages/{img.ImageName}.{img.ImageType}");
                    _productContext.Entry(img).State = EntityState.Deleted;
                }


                searchCategory.ImageId = category.ImageId;
                try
                {
                    await _productContext.SaveChangesAsync();
                    return Ok();
                }
                catch
                {
                    return BadRequest("BadRequest");
                }
            }
            else return BadRequest("BadRequest");
        }

        //Удалить товар
        [HttpDelete("Product/Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (id < 0) return BadRequest("Not valid product id");
            try
            {
                _productContext.Entry(await _productContext.products.Where(x => x.Id == id).FirstOrDefaultAsync()).State = EntityState.Deleted;
                await _productContext.SaveChangesAsync();
            }
            catch
            {
                return BadRequest("Not valid product id");
            }
            return Ok();
        }

        //Удалить картинку по id
        [HttpDelete("Image/Delete/{id}")]
        public async Task<ActionResult> DeleteImage(int id)
        {
            try
            {
                var img = _productContext.Images.First(x => x.Id == id);
                System.IO.File.Delete($"{_env.WebRootPath}/ProductImages/{img.ImageName}.{img.ImageType}");
                _productContext.Entry(img).State = EntityState.Deleted;
                await _productContext.SaveChangesAsync();
                return Ok();
            }
            catch
            {
                return BadRequest("Все плохо");
            }
        }

        //Удалить категорию
        [HttpDelete("Category/Delete/{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            //object test;
            if (id < 0) return BadRequest("Not valid product id");
            try
            {
                //await DeleteImageBiCategoryId(id);
                var img = _productContext.Images.First(x => x.Id == (_productContext.categories.First(c => c.Id == id)).ImageId);
                System.IO.File.Delete($"{_env.WebRootPath}/ProductImages/{img.ImageName}.{img.ImageType}");
                _productContext.Entry(img).State = EntityState.Deleted;
                //await _productContext.SaveChangesAsync();
                //return Ok($@"{_env.WebRootPath}\ProductImages\{img.ImageName}.{img.ImageType}");
                _productContext.Entry(await _productContext.categories.Where(x => x.Id == id).FirstOrDefaultAsync()).State = EntityState.Deleted;
                await _productContext.SaveChangesAsync();
                //test = $"{_env.WebRootPath}/ProductImages/{img.ImageName}.{img.ImageType}";
            }
            catch
            {
                return BadRequest("Not valid product id");
            }
            return Ok();
        }

        //Удалить группу по id
        [HttpDelete("Group/Delete/{id}")]
        public async Task<ActionResult> DeleteGroup(int id)
        {
            if (id < 0) return BadRequest("Not valid product id");
            try
            {
                _productContext.Entry(await _productContext.groups.Where(x => x.Id == id).FirstOrDefaultAsync()).State = EntityState.Deleted;
                await _productContext.SaveChangesAsync();
            }
            catch
            {
                return BadRequest("Not valid product id");
            }
            return Ok();
        }

        [Route("Image/Add")]
        [HttpPost]
        public async Task<ActionResult> PostTest(IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    Images img = new Images
                    {
                        //ImageName = (_productContext.Images.Count() > 0 ? (Convert.ToInt32(await _productContext.Images.MaxAsync(x => x.Id)) + 1).ToString() : "0"),
                        ImageName = $"{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Second}",
                        ImageSize = Convert.ToInt32(file.Length),
                        ImageType = file.FileName.Substring(file.FileName.LastIndexOf(".") + 1, file.FileName.Length - file.FileName.LastIndexOf(".") - 1)
                    };
                    await _productContext.Images.AddAsync(img);
                    await _productContext.SaveChangesAsync();
                    using (var fileStream = new FileStream($"{_env.WebRootPath}/ProductImages/{img.ImageName}{file.FileName.Substring(file.FileName.LastIndexOf("."), file.FileName.Length - file.FileName.LastIndexOf("."))}", FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    return Ok(img.Id);
                }
                else return BadRequest("file not found");
            }
            else return BadRequest("Model is not valid");
        }

        [Route("Test/Get")]
        //public async Task<ActionResult<List<Images>>> GetTest()
        public async Task<ActionResult> GetTest(Category c)
        {
            //return _productContext.Images.ToList();
            return Ok($"{c.Title} {c.Discription}");
            

            //Category c = new Category();
            //c.Title = t;c.Discription = d; c.ImageId = i;
            //_productContext.categories.Add();
            //_productContext.categories.Add(c);
            //_productContext.SaveChanges();
            //return Content(d);
        }

        [Route("Test/Post")]
        public async Task<ActionResult> PostTest(IFormFile[] files)
        {
            if (ModelState.IsValid)
            {
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        Images img = new Images
                        {
                            ImageName = (_productContext.Images.Count() > 0 ? (Convert.ToInt32(await _productContext.Images.MaxAsync(x => x.Id)) + 1).ToString() : "0"),
                            ImageSize = Convert.ToInt32(file.Length),
                            ImageType = file.FileName.Substring(file.FileName.LastIndexOf(".") + 1, file.FileName.Length - file.FileName.LastIndexOf(".") - 1)
                            //ProductId = Convert.ToInt32(HttpContext.Request.Form.ToList().First(x => x.Key == "productId").Value)
                        };
                        await _productContext.Images.AddAsync(img);
                        await _productContext.SaveChangesAsync();
                        using (var fileStream = new FileStream($"{_env.WebRootPath}/ProductImages/{img.Id}{file.FileName.Substring(file.FileName.LastIndexOf("."), file.FileName.Length - file.FileName.LastIndexOf("."))}", FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                    }
                    return Ok();
                }
                else return BadRequest("Model is not valid");
            }
            else return BadRequest("Model is not valid");
        }
        [Route("Test/Put")]
        public async Task<ActionResult> PutTest()
        {
            return Ok();
        }
        [Route("Test/Delete")]
        public async Task<ActionResult> DeleteTest()
        {
            return Ok();
        }
    }
}