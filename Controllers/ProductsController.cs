using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Many_To_Many_RawSqL.Data;
using Many_To_Many_RawSqL.Models;

namespace Many_To_Many_RawSqL.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public IActionResult Index()
        {
            try
            {
                // Use raw SQL query to fetch products
                var products = _context.Products.FromSqlRaw("SELECT * FROM Products").ToList();
                return View(products);
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                return Problem("An error occurred while fetching products: " + ex.Message);
            }
        }

        // GET: Products/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                // Use raw SQL query with parameter binding to fetch a specific product
                var product = _context.Products.FromSqlInterpolated($"SELECT * FROM Products WHERE P_ID = {id}").FirstOrDefault();

                if (product == null)
                {
                    return NotFound();
                }

                return View(product);
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                return Problem("An error occurred while fetching product details: " + ex.Message);
            }
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("P_ID,P_Name,P_Price,P_Detail,P_ImgUrl")] Products products)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Use raw SQL query to insert a new product
                    _context.Database.ExecuteSqlInterpolated($"INSERT INTO Products (P_Name, P_Price, P_Detail, P_ImgUrl) VALUES ({products.P_Name}, {products.P_Price}, {products.P_Detail}, {products.P_ImgUrl})");
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Handle any exceptions here
                    return Problem("An error occurred while creating the product: " + ex.Message);
                }
            }
            return View(products);
        }

        // GET: Products/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                // Use raw SQL query with parameter binding to fetch a specific product for editing
                var product = _context.Products.FromSqlInterpolated($"SELECT * FROM Products WHERE P_ID = {id}").FirstOrDefault();

                if (product == null)
                {
                    return NotFound();
                }

                return View(product);
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                return Problem("An error occurred while fetching product details for editing: " + ex.Message);
            }
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("P_ID,P_Name,P_Price,P_Detail,P_ImgUrl")] Products products)
        {
            if (id != products.P_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Use raw SQL query with parameter binding to update the product
                    _context.Database.ExecuteSqlInterpolated($"UPDATE Products SET P_Name = {products.P_Name}, P_Price = {products.P_Price}, P_Detail = {products.P_Detail}, P_ImgUrl = {products.P_ImgUrl} WHERE P_ID = {id}");
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    // Handle any exceptions here
                    return Problem("An error occurred while updating the product: " + ex.Message);
                }

                return RedirectToAction(nameof(Index));
            }
            return View(products);
        }

        // GET: Products/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                // Use raw SQL query with parameter binding to fetch a specific product for deletion
                var product = _context.Products.FromSqlInterpolated($"SELECT * FROM Products WHERE P_ID = {id}").FirstOrDefault();

                if (product == null)
                {
                    return NotFound();
                }

                return View(product);
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                return Problem("An error occurred while fetching product details for deletion: " + ex.Message);
            }
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                // Use raw SQL query with parameter binding to delete the product
                _context.Database.ExecuteSqlInterpolated($"DELETE FROM Products WHERE P_ID = {id}");
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                return Problem("An error occurred while deleting the product: " + ex.Message);
            }
        }

        private bool ProductsExists(int id)
        {
            try
            {
                // Use raw SQL query with parameter binding to check if a product with the given ID exists
                var exists = _context.Products.FromSqlInterpolated($"SELECT 1 FROM Products WHERE P_ID = {id}").Any();
                return exists;
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                throw new DataException("An error occurred while checking for product existence: " + ex.Message);
            }
        }
    }
}
