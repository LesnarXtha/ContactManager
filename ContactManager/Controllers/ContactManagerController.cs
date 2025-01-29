using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContactManager.Models;
using System.Text;

namespace ContactManager.Controllers
{
    public class ContactManagerController : Controller
    {
        private readonly ContactManagerDbContext _context;

        public ContactManagerController(ContactManagerDbContext context)
        {
            _context = context;
        }

        // GET: ContactManager
        public async Task<IActionResult> Index(string searchQuery)
        {
            // Filter contacts based on the search query
            var contacts = from c in _context.Contacts select c;

            if (!string.IsNullOrEmpty(searchQuery))
            {
                contacts = contacts.Where(c => c.FirstName.Contains(searchQuery)
                                            || c.LastName.Contains(searchQuery)
                                            || c.Category.Contains(searchQuery)
                                            || c.Tags.Contains(searchQuery));
            }

            ViewData["SearchQuery"] = searchQuery; // Pass search query back to the view
            return View(await contacts.ToListAsync());
        }


        // GET: ContactManager/AddOrEdit
        public IActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return View(new Contact());
            }
            else
            {
                return View(_context.Contacts.Find(id));
            }

        }

        // POST: ContactManager/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("Id,FirstName,LastName,Email,PhoneNumber,Category,Tags,Address,Date")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                if (contact.Id == 0)
                {
                    contact.Date = DateTime.Now;
                    _context.Add(contact);
                }
                else {
                    _context.Update(contact);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contact);
        }

        // POST: ContactManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contactManager = await _context.Contacts.FindAsync(id);
            if (contactManager != null)
            {
                _context.Contacts.Remove(contactManager);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> ExportToCsv(string searchQuery)
        {
            // Filter contacts based on the search query
            var contacts = from c in _context.Contacts select c;

            if (!string.IsNullOrEmpty(searchQuery))
            {
                contacts = contacts.Where(c => c.FirstName.Contains(searchQuery)
                                            || c.LastName.Contains(searchQuery)
                                            || c.Category.Contains(searchQuery)
                                            || c.Tags.Contains(searchQuery));
            }

            var filteredContacts = await contacts.ToListAsync();

            // Build CSV content
            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("Id,FirstName,LastName,Email,PhoneNumber,Category,Tags,Address,Date");

            foreach (var contact in filteredContacts)
            {
                csvBuilder.AppendLine(
                    $"{contact.Id}," +
                    $"{contact.FirstName}," +
                    $"{contact.LastName}," +
                    $"{contact.Email}," +
                    $"{contact.PhoneNumber}," +
                    $"{contact.Category}," +
                    $"{contact.Tags}," +
                    $"{contact.Address}," +
                    $"{contact.Date:yyyy-MM-dd}"
                );
            }

            // Return the CSV file
            return File(Encoding.UTF8.GetBytes(csvBuilder.ToString()), "text/csv", "FilteredContacts.csv");
        }

        // Import Contacts from CSV
        [HttpPost]
        public async Task<IActionResult> ImportFromCsv(IFormFile csvFile)
        {
            if (csvFile == null || csvFile.Length == 0)
            {
                ModelState.AddModelError("", "Please select a valid CSV file.");
                return RedirectToAction(nameof(Index));
            }

            using (var stream = new StreamReader(csvFile.OpenReadStream()))
            {
                var lineNumber = 0;
                while (!stream.EndOfStream)
                {
                    var line = await stream.ReadLineAsync();
                    if (lineNumber == 0) // Skip header row
                    {
                        lineNumber++;
                        continue;
                    }

                    var values = line.Split(',');
                    var contact = new Contact
                    {
                        FirstName = values[1],
                        LastName = values[2],
                        Email = values[3],
                        PhoneNumber = values[4],
                        Category = values[5],
                        Tags = values[6],
                        Address = values[7],
                        Date = DateTime.Parse(values[8])
                    };

                    _context.Contacts.Add(contact);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
       
  