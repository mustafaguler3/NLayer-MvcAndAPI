using Microsoft.EntityFrameworkCore;
using NLayer.Core;
using NLayer.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(VtContext context) : base(context)
        {
        }

        public async Task<List<Product>> GetProductWithCategory()
        {
            //Eager Loading
            return await _context.Products.Include(i => i.Category).ToListAsync();
        }
    }
}
