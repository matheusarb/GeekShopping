using AutoMapper;
using GeekShopping.ProductAPI.Config;
using GeekShopping.ProductAPI.Data.ValueObjects;
using GeekShopping.ProductAPI.Models;
using GeekShopping.ProductAPI.Models.Context;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.ProductAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly MySQLContext _context;
        private IMapper _mapper;

        public ProductRepository(MySQLContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductVO>> FindAll()
        {
            var products = await _context.Products.ToListAsync();
            return _mapper.Map<List<ProductVO>>(products);
        }
        public async Task<ProductVO> FindById(long id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<ProductVO>(product);
        }
        public async Task<ProductVO> Create(ProductVO vo)
        {            
            if (vo is null)
                return null;
            
            var product = _mapper.Map<Product>(vo);
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProductVO>(product);            
        }
                     
        public async Task<ProductVO> Update(long id, ProductVO productVO)
        {
            var productDb = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (productVO is null)
                return null;

            productDb!.Update(productVO);
            _context.Products.Update(productDb);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProductVO>(productDb);
        }

        public async Task<bool> Delete(long id)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
                if (product is null)
                    return false;

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
