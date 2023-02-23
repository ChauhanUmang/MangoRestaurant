using AutoMapper;
using Mango.Services.ProductAPI.DbContexts;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ProductAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public ProductRepository(ApplicationDbContext db,IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<ProductDTO> CreateUpdateProduct(ProductDTO productDTO)
        {
            //Convert DTO to Domain object
            Product product = _mapper.Map<Product>(productDTO);
            if(product.ProductId > 0)
            {
                _db.Products.Update(product);
            }
            else
            {
                _db.Products.Add(product);
            }

            await _db.SaveChangesAsync();

            //return change domain object to DTO
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            try
            {
                var product = await _db.Products.FirstOrDefaultAsync(x => x.ProductId == productId);
                if (product == null)
                {
                    return false;
                }
                _db.Products.Remove(product);
                await _db.SaveChangesAsync();
                return true; 
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<ProductDTO> GetProductById(int productId)
        {
            var product = await _db.Products.FirstOrDefaultAsync(x => x.ProductId == productId);
            if(product is null)
            {
                return null;
            }

            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<IEnumerable<ProductDTO>> GetProducts()
        {
            //Get list of all products
            var productList = await _db.Products.ToListAsync();

            //return by convert product domain list to productDTO list
            return _mapper.Map<List<ProductDTO>>(productList);
        }
    }
}
