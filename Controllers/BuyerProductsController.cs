
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Many_To_Many_RawSqL.Data;
using Many_To_Many_RawSqL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Many_To_Many_RawSqL.Controllers
{
    public class BuyerProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BuyerProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BuyerProducts1
        public IActionResult Index()
        {
            try
            {
                // Use raw SQL query to fetch buyer products
                var buyerProducts = _context.BuyerProduct.FromSqlRaw("SELECT * FROM BuyerProduct").Include(b => b.Products).ToList();
                return View(buyerProducts);
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                return Problem("An error occurred while fetching buyer products: " + ex.Message);
            }
        }

        // GET: BuyerProducts1/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                // Use raw SQL query with parameter binding to fetch a specific buyer product
                var buyerProduct = _context.BuyerProduct.FromSqlInterpolated($"SELECT * FROM BuyerProduct WHERE BuyerProductId = {id}").FirstOrDefault();

                if (buyerProduct == null)
                {
                    return NotFound();
                }

                return View(buyerProduct);
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                return Problem("An error occurred while fetching buyer product details: " + ex.Message);
            }
        }

        // GET: BuyerProducts1/Create
        public IActionResult Create()
        {
            ViewData["BuyerId"] = new SelectList(_context.buyers, "BuyerId", "BuyerId");
            ViewData["ProductP_ID"] = new SelectList(_context.Products, "P_ID", "P_ID");
            return View();
        }

        // POST: BuyerProducts1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BuyerProductId,BuyerId,ProductP_ID")] BuyerProducts buyerProducts)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Use raw SQL query with parameter binding to insert a new buyer product
                    _context.Database.ExecuteSqlInterpolated($"INSERT INTO BuyerProduct (BuyerId, ProductP_ID) VALUES ({buyerProducts.BuyerId}, {buyerProducts.ProductP_ID})");
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Handle any exceptions here
                    return Problem("An error occurred while creating the buyer product: " + ex.Message);
                }
            }
            ViewData["BuyerId"] = new SelectList(_context.buyers, "BuyerId", "BuyerId", buyerProducts.BuyerId);
            ViewData["ProductP_ID"] = new SelectList(_context.Products, "P_ID", "P_ID", buyerProducts.ProductP_ID);
            return View(buyerProducts);
        }

        // GET: BuyerProducts1/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                // Use raw SQL query with parameter binding to fetch a specific buyer product for editing
                var buyerProduct = _context.BuyerProduct.FromSqlInterpolated($"SELECT * FROM BuyerProduct WHERE BuyerProductId = {id}").FirstOrDefault();

                if (buyerProduct == null)
                {
                    return NotFound();
                }

                ViewData["BuyerId"] = new SelectList(_context.buyers, "BuyerId", "BuyerId", buyerProduct.BuyerId);
                ViewData["ProductP_ID"] = new SelectList(_context.Products, "P_ID", "P_ID", buyerProduct.ProductP_ID);
                return View(buyerProduct);
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                return Problem("An error occurred while fetching buyer product details for editing: " + ex.Message);
            }
        }

        // POST: BuyerProducts1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BuyerProductId,BuyerId,ProductP_ID")] BuyerProducts buyerProducts)
        {
            if (id != buyerProducts.BuyerProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Use raw SQL query with parameter binding to update the buyer product
                    _context.Database.ExecuteSqlInterpolated($"UPDATE BuyerProduct SET BuyerId = {buyerProducts.BuyerId}, ProductP_ID = {buyerProducts.ProductP_ID} WHERE BuyerProductId = {id}");
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    // Handle any exceptions here
                    return Problem("An error occurred while updating the buyer product: " + ex.Message);
                }

                return RedirectToAction(nameof(Index));
            }
            ViewData["BuyerId"] = new SelectList(_context.buyers, "BuyerId", "BuyerId", buyerProducts.BuyerId);
            ViewData["ProductP_ID"] = new SelectList(_context.Products, "P_ID", "P_ID", buyerProducts.ProductP_ID);
            return View(buyerProducts);
        }

        // GET: BuyerProducts1/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                // Use raw SQL query with parameter binding to fetch a specific buyer product for deletion
                var buyerProduct = _context.BuyerProduct.FromSqlInterpolated($"SELECT * FROM BuyerProduct WHERE BuyerProductId = {id}").FirstOrDefault();

                if (buyerProduct == null)
                {
                    return NotFound();
                }

                return View(buyerProduct);
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                return Problem("An error occurred while fetching buyer product details for deletion: " + ex.Message);
            }
        }

        // POST: BuyerProducts1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                // Use raw SQL query with parameter binding to delete the buyer product
                _context.Database.ExecuteSqlInterpolated($"DELETE FROM BuyerProduct WHERE BuyerProductId = {id}");
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                return Problem("An error occurred while deleting the buyer product: " + ex.Message);
            }
        }

        private bool BuyerProductsExists(int id)
        {
            try
            {
                // Use raw SQL query with parameter binding to check if a buyer product with the given ID exists
                var exists = _context.BuyerProduct.FromSqlInterpolated($"SELECT 1 FROM BuyerProduct WHERE BuyerProductId = {id}").Any();
                return exists;
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                throw new DataException("An error occurred while checking for buyer product existence: " + ex.Message);
            }
        }
    }
}

