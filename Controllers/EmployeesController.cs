using EmployeeCRUD2.Data;
using EmployeeCRUD2.Models;
using EmployeeCRUD2.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeCRUD2.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly EmployeeDbContext employeeDb;

        public EmployeesController(EmployeeDbContext employeeDb)
        {
            this.employeeDb = employeeDb;
        }
        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employee = await employeeDb.Employees.ToListAsync();
            return View(employee);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest)
        {
            var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = addEmployeeRequest.Name,
                Email = addEmployeeRequest.Email,
                Salary = addEmployeeRequest.Salary,
                DateofBirth = addEmployeeRequest.DateofBirth,
                Department = addEmployeeRequest.Department,
            };

            await employeeDb.Employees.AddAsync(employee);
            await employeeDb.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var employee = await employeeDb.Employees.FirstOrDefaultAsync(x => x.Id == id);

            if (employee != null)
            {
                var viewModel = new UpdateEmployeeViewModel()
                {
                    Id = Guid.NewGuid(),
                    Name = employee.Name,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    DateofBirth = employee.DateofBirth,
                    Department = employee.Department,
                };
                return await Task.Run(() => View("View", viewModel));
            }
            return RedirectToAction("Index");
        }

        [HttpPost]

        public async Task<IActionResult> View(UpdateEmployeeViewModel model)
        {
            var employee = await employeeDb.Employees.FindAsync(model.Id);

            if (employee != null)
            {
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Salary = model.Salary;
                employee.DateofBirth = model.DateofBirth;
                employee.Department = model.Department;

                await employeeDb.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]

        public async Task<IActionResult> Delete(UpdateEmployeeViewModel model)
        {
            var employee = employeeDb.Employees.FindAsync(model.Id);

            if (employee != null)
            {
                employeeDb.Employees.Remove(await employee);

                await employeeDb.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }

    

}

