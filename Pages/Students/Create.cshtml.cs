using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ASPNET_ContosoUniversity.Models;
using ContosoUniversity.Models;

namespace ASPNET_ContosoUniversity.Pages.Students
{
    public class CreateModel : PageModel
    {
        private readonly ContosoUniversity.Models.SchoolContext _context;

        public CreateModel(ContosoUniversity.Models.SchoolContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Student Student { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var emptyStudent = new Student();

            if (await TryUpdateModelAsync<Student>(
                emptyStudent,
                "student",   // Prefix for form value. ("student", // Prefix) es el prefijo que se usa para buscar valores. No distingue mayúsculas de minúsculas.
                s => s.FirstMidName, s => s.LastName, s => s.EnrollmentDate))
                //TryUpdateModelAsync<Student> intenta actualizar el objeto emptyStudent mediante los valores de 
                //formulario enviados desde la propiedad PageContext del PageModel. TryUpdateModelAsync solo 
                //actualiza las propiedades enumeradas (s => s.FirstMidName, s => s.LastName, s => s.EnrollmentDate).
            {
                _context.Student.Add(emptyStudent);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            return null;
        }
    }
}