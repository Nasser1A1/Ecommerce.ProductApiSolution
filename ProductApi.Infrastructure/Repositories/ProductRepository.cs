using eCommerce.SharedLib.Logs;
using eCommerce.SharedLib.ResponseT;
using Microsoft.EntityFrameworkCore;
using ProductApi.Application.Interfaces;
using ProductApi.Domain.Entities;
using ProductApi.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Infrastructure.Repositories
{
    public class ProductRepository(ProductDbContext context) : IProduct
    {
        public async Task<Response> CreateAsync(Product entity)
        {
            try
            {
                var getProduct = await GetByAsync(_=> EF.Functions.Like(_.Name, entity.Name));
                if (getProduct is not null && !string.IsNullOrEmpty(getProduct.Name))
                {
                    return new Response(false, $"{entity.Name} with the same name already exists.");
                }
                var currentEntity = await context.Products.AddAsync(entity);
                await context.SaveChangesAsync();
                if (currentEntity.Entity is not null && currentEntity.Entity.Id > 0)
                {
                    return new Response(true, $"{entity.Name} created successfully.");
                }
                else
                {
                    return new Response(false, $"{entity.Name} creation failed. Please try again.");
                }
            }
            catch (Exception ex)
            {
                // log original Exception
                LogExceptions.LogException(ex);
                // return a Message That is not scary to the client
                return new Response(false, "An error occurred while creating the product. Please try again later.");
            }
        }

        public async Task<Response> DeleteAsync(Product entity)
        {
             try
            {
                var product = await FindByIdAsync(entity.Id);
                if (product is null)
                {
                    return new Response(false, $"{entity.Name} is Not Found");
                }
                context.Products.Remove(entity);
                await context.SaveChangesAsync();
                return new Response(true, $"{entity.Name} deleted successfully");
            }
            catch (Exception ex)
            {
                // log original Exception
                LogExceptions.LogException(ex);
                // return a Message That is not scary
                return new Response(false, "An error occurred while deleting the product. Please try again later.");
            }
        }

        public async Task<Product> FindByIdAsync(int id)
        {
            try
            {
                var product = await context.Products.FindAsync(id);
                return product is not null ? product : null!;
            }
            catch (Exception ex)
            {
                // log original Exception
                LogExceptions.LogException(ex);
                // return a Message That is not scary
                throw new Exception("An error occurred while retrieving the product. Please try again later.");
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            try
            {
                var products = await context.Products.AsNoTracking().ToListAsync();
                return products is not null ? products : null!;
            }
            catch (Exception ex)
            {
                // log original Exception
                LogExceptions.LogException(ex);
                // return a Message That is not scary
                throw new Exception("An error occurred while retrieving the products. Please try again later.");
            }
        }

        public async Task<Product> GetByAsync(Expression<Func<Product, bool>> predicate)
        {
            try
            {
                var products = await context.Products.Where(predicate).FirstOrDefaultAsync();
                return products is not null ? products : null!;
            }
            catch (Exception ex)
            {
                // log original Exception
                LogExceptions.LogException(ex);
                // return a Message That is not scary
                throw new InvalidOperationException("An error occurred while retrieving the product. Please try again later.");
            }
            
        }

        public async Task<Response> UpdateAsync(Product entity)
        {
            try
            {
                var oldProduct = context.Products.Find(entity.Id);
                if (oldProduct is null)
                    return new Response(false, $"Product {entity.Name} Not Found"); ;
                
                // Set the state of the old product to Detached to avoid tracking issues
                context.Entry(oldProduct).State = EntityState.Detached;
                context.Products.Update(entity);
                await context.SaveChangesAsync();
                return new Response(true, $"{entity.Name} updated successfully.");
            }
            catch (Exception ex)
            {
                // log original Exception
                LogExceptions.LogException(ex);
                // return a Message That is not scary
                return new Response(false,"An error occurred while updating the product. Please try again later.");
            }
        }
    }
}
