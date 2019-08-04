using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using System.Data.Entity;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomersController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Customers
        public ViewResult Index()
        {
            var customers = _context.Customers.Include(c => c.MembershipType).ToList();
            return View(customers);
        }

        public ActionResult Edit(int id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);

            if (customer == null)
                return HttpNotFound();

            var membershipTypes = _context.MembershipTypes.ToList();
            var editCustomerViewModel = new CustomerFormViewModel()
            {
                MembershipTypes = membershipTypes,
                Customer = customer               
            };

            return View("CustomerForm", editCustomerViewModel);
        }
        public ActionResult New()
        {
            var membershipTypes = _context.MembershipTypes.ToList();
            var newCustomerViewModel = new CustomerFormViewModel()
                {
                    MembershipTypes = membershipTypes
                };
            return View("CustomerForm", newCustomerViewModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(CustomerFormViewModel customerForm)
        {
            if (!ModelState.IsValid)
            {
                var customerFormViewModel = new CustomerFormViewModel()
                {
                    MembershipTypes = _context.MembershipTypes.ToList(),
                    Customer = customerForm.Customer
                };
                return View("CustomerForm", customerFormViewModel);
            }

            if(customerForm.Customer.Id == 0)
            {
                _context.Customers.Add(customerForm.Customer);
            }
            else
            {
                var customerInDb = _context.Customers.SingleOrDefault(c=> c.Id == customerForm.Customer.Id);

                customerInDb.Name = customerForm.Customer.Name;
                customerInDb.Birthday = customerForm.Customer.Birthday;
                customerInDb.IsSubscribedToNewsletter = customerForm.Customer.IsSubscribedToNewsletter;
                customerInDb.MembershipTypeId = customerForm.Customer.MembershipTypeId;
            }   

            _context.SaveChanges();

            return RedirectToAction("Index", "Customers");
        }
    }
}