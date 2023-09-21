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
    public class BuyersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BuyersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Buyers
        public IActionResult Index()
        {
            try
            {
                // Use raw SQL query to fetch buyers
                var buyers = _context.buyers.FromSqlRaw("SELECT * FROM Buyers").ToList();
                return View(buyers);
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                return Problem("An error occurred while fetching buyers: " + ex.Message);
            }
        }

        // GET: Buyers/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                // Use raw SQL query with parameter binding to fetch a specific buyer
                var buyer = _context.buyers.FromSqlInterpolated($"SELECT * FROM Buyers WHERE BuyerId = {id}").FirstOrDefault();

                if (buyer == null)
                {
                    return NotFound();
                }

                return View(buyer);
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                return Problem("An error occurred while fetching buyer details: " + ex.Message);
            }
        }

        // GET: Buyers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Buyers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BuyerId,Name")] Buyer buyer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Use raw SQL query to insert a new buyer
                    _context.Database.ExecuteSqlInterpolated($"INSERT INTO Buyers (Name) VALUES ({buyer.Name})");
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Handle any exceptions here
                    return Problem("An error occurred while creating the buyer: " + ex.Message);
                }
            }
            return View(buyer);
        }

        // GET: Buyers/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                // Use raw SQL query with parameter binding to fetch a specific buyer for editing
                var buyer = _context.buyers.FromSqlInterpolated($"SELECT * FROM Buyers WHERE BuyerId = {id}").FirstOrDefault();

                if (buyer == null)
                {
                    return NotFound();
                }

                return View(buyer);
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                return Problem("An error occurred while fetching buyer details for editing: " + ex.Message);
            }
        }

        // POST: Buyers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BuyerId,Name")] Buyer buyer)
        {
            if (id != buyer.BuyerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Use raw SQL query with parameter binding to update the buyer
                    _context.Database.ExecuteSqlInterpolated($"UPDATE Buyers SET Name = {buyer.Name} WHERE BuyerId = {id}");
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    // Handle any exceptions here
                    return Problem("An error occurred while updating the buyer: " + ex.Message);
                }

                return RedirectToAction(nameof(Index));
            }
            return View(buyer);
        }

        // GET: Buyers/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                // Use raw SQL query with parameter binding to fetch a specific buyer for deletion
                var buyer = _context.buyers.FromSqlInterpolated($"SELECT * FROM Buyers WHERE BuyerId = {id}").FirstOrDefault();

                if (buyer == null)
                {
                    return NotFound();
                }

                return View(buyer);
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                return Problem("An error occurred while fetching buyer details for deletion: " + ex.Message);
            }
        }

        // POST: Buyers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                // Use raw SQL query with parameter binding to delete the buyer
                _context.Database.ExecuteSqlInterpolated($"DELETE FROM Buyers WHERE BuyerId = {id}");
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                return Problem("An error occurred while deleting the buyer: " + ex.Message);
            }
        }

        private bool BuyerExists(int id)
        {
            try
            {
                // Use raw SQL query with parameter binding to check if a buyer with the given ID exists
                var exists = _context.buyers.FromSqlInterpolated($"SELECT 1 FROM Buyers WHERE BuyerId = {id}").Any();
                return exists;
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                throw new DataException("An error occurred while checking for buyer existence: " + ex.Message);
            }
        }
    }
}
