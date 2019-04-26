using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ASPNET_ContosoUniversity.Models;
using ContosoUniversity.Models;

namespace ASPNET_ContosoUniversity.Pages.Students
{
    public class IndexModel : PageModel
    {
        private readonly ContosoUniversity.Models.SchoolContext _context;

        public IndexModel(ContosoUniversity.Models.SchoolContext context)
        {
            _context = context;
        }

        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public PaginatedList<Student> Student { get; set; }

        #region Get normal
        //public async Task OnGetAsync()
        //{
        //    Student = await _context.Student.ToListAsync();
        //}
        #endregion

        #region Get con ordenamiento de apellido o fecha de anotación
        //public async Task OnGetAsync(string sortOrder)
        //{
        //    NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        //    DateSort = sortOrder == "Date" ? "date_desc" : "Date";

        //    IQueryable<Student> studentIQ = from s in _context.Student
        //                                    select s;

        //    switch (sortOrder)
        //    {
        //        case "name_desc":
        //            studentIQ = studentIQ.OrderByDescending(s => s.LastName);
        //            break;
        //        case "Date":
        //            studentIQ = studentIQ.OrderBy(s => s.EnrollmentDate);
        //            break;
        //        case "date_desc":
        //            studentIQ = studentIQ.OrderByDescending(s => s.EnrollmentDate);
        //            break;
        //        default:
        //            studentIQ = studentIQ.OrderBy(s => s.LastName);
        //            break;
        //    }

        //    //Cuando se crea o se modifica un IQueryable, no se envía ninguna consulta a la base de datos. 
        //    //La consulta no se ejecuta hasta que el objeto IQueryable se convierte en una colección. 
        //    //IQueryable se convierte en una colección mediante una llamada a un método como ToListAsync. 
        //    //Por lo tanto, el código IQueryable produce una única consulta que no se ejecuta hasta la siguiente instrucción:

        //    Student = await studentIQ.AsNoTracking().ToListAsync();
        //}
        #endregion

        #region Get con busqueda filtrada
        //public async Task OnGetAsync(string sortOrder, string searchString)
        //{
        //    NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        //    DateSort = sortOrder == "Date" ? "date_desc" : "Date";
        //    CurrentFilter = searchString;

        //    IQueryable<Student> studentIQ = from s in _context.Student
        //                                    select s;
        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        studentIQ = studentIQ.Where(s => s.LastName.Contains(searchString)
        //                               || s.FirstMidName.Contains(searchString));
        //    }

        //    switch (sortOrder)
        //    {
        //        case "name_desc":
        //            studentIQ = studentIQ.OrderByDescending(s => s.LastName);
        //            break;
        //        case "Date":
        //            studentIQ = studentIQ.OrderBy(s => s.EnrollmentDate);
        //            break;
        //        case "date_desc":
        //            studentIQ = studentIQ.OrderByDescending(s => s.EnrollmentDate);
        //            break;
        //        default:
        //            studentIQ = studentIQ.OrderBy(s => s.LastName);
        //            break;
        //    }

        //    //Agrega el parámetro searchString al método OnGetAsync.
        //    //El valor de la cadena de búsqueda se recibe desde un cuadro de texto que se agrega en la siguiente sección.
        //    //Se agregó una cláusula Where a la instrucción LINQ.
        //    //La cláusula Where selecciona solo los alumnos cuyo nombre o apellido contienen la cadena de búsqueda. 
        //    //La instrucción LINQ se ejecuta solo si hay un valor para buscar.

        //    Student = await studentIQ.AsNoTracking().ToListAsync();
        //}
        #endregion

        #region Get con paginación
        public async Task OnGetAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex)
            //Todos los parámetros son NULL cuando:
            //Se llama a la página desde el vínculo Students.
            //El usuario no ha seleccionado un vínculo de ordenación o paginación.
        {
            CurrentSort = sortOrder;
            NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            DateSort = sortOrder == "Date" ? "date_desc" : "Date";
            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            CurrentFilter = searchString;

            IQueryable<Student> studentIQ = from s in _context.Student
                                            select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                studentIQ = studentIQ.Where(s => s.LastName.Contains(searchString)
                                       || s.FirstMidName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    studentIQ = studentIQ.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    studentIQ = studentIQ.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    studentIQ = studentIQ.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default:
                    studentIQ = studentIQ.OrderBy(s => s.LastName);
                    break;
            }

            int pageSize = 3;
            Student = await PaginatedList<Student>.CreateAsync(
                studentIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
        #endregion
    }
}
