using SoftUni.Data;
using SoftUni.Models;
using System.Globalization;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniContext dbContext = new SoftUniContext();
            string result = RemoveTown(dbContext);

            Console.WriteLine(result);
        }


        //03. Employees Full Information
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .OrderBy(e => e.EmployeeId)
                .Select(e => new
                {
                    e.FirstName,
                    e.MiddleName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary
                })
                .ToArray();

            foreach (var employee in employees)
            {
                sb
                    .AppendLine($"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        //04. Employees with Salary Over 50 000
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .OrderBy(e => e.FirstName)
                .Where(e => e.Salary > 50000)
                .Select(e => new
                {
                    e.FirstName,
                    e.Salary
                })
                .ToArray();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} - {employee.Salary:f2}");
            }
            return sb.ToString().TrimEnd();
        }

        //05. Employees from Research and Development
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Where(e => e.Department.Name == "Research and Development")
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    DepartmentName = e.Department,
                    e.Salary
                })
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .ToArray();

            foreach (var employee in employees)
            {
                sb
                    .AppendLine(
                        $"{employee.FirstName} {employee.LastName} from {employee.DepartmentName.Name} - ${employee.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        //06. Adding a New Address and Updating Employee
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            Address newAddress = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            Employee? employee = context.Employees
                .FirstOrDefault(e => e.LastName == "Nakov");
            employee.Address = newAddress;

            context.SaveChanges();

            var employeeAddresses = context.Employees
                .OrderByDescending(e => e.AddressId)
                .Take(10)
                .Select(e => e.Address.AddressText)
                .ToArray();

            foreach (var employeeAddress in employeeAddresses)
            {
                sb.AppendLine(employeeAddress);
            }

            return sb.ToString().TrimEnd();
        }

        //07. Employees and Projects
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employeesProjects = context.Employees
                //.Where(ep => ep.EmployeesProjects
                //    .Any(ep => ep.Project.StartDate.Year >= 2001 &&
                //               ep.Project.StartDate.Year <= 2003))
                .Take(10)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    ManagerFirstName = e.Manager!.FirstName,
                    ManagerLastName = e.Manager!.LastName,
                    Pojects = e.EmployeesProjects
                        .Where(e => e.Project.StartDate.Year >= 2001 && e.Project.StartDate.Year <= 2003)
                        .Select(ep => new
                        {
                            ProjectName = ep.Project.Name,
                            StartDate = ep.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                            EndDate = ep.Project.EndDate.HasValue ?
                                ep.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) : "not finished"
                        })
                    .ToArray()
                })
                .ToArray();

            foreach (var e in employeesProjects)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - Manager: {e.ManagerFirstName} {e.ManagerLastName}");
                foreach (var p in e.Pojects)
                {
                    sb.AppendLine($"--{p.ProjectName} - {p.StartDate} - {p.EndDate}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        //08. Addresses by Town
        public static string GetAddressesByTown(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var addressEmployees = context.Addresses
                .OrderByDescending(ae => ae.Employees.Count)
                .ThenBy(ae => ae.Town.Name)
                .ThenBy(ae => ae.AddressText)
                .Select(ae => new
                {
                    Address = ae.AddressText,
                    Town = ae.Town.Name,
                    EmployeeCount = ae.Employees.Count
                })
                .Take(10)
                .ToArray();

            foreach (var ae in addressEmployees)
            {
                sb.AppendLine($"{ae.Address}, {ae.Town} - {ae.EmployeeCount} employees");
            }

            return sb.ToString().TrimEnd();
        }

        //09. Employee 147
        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employee = context.Employees
                .Where(e => e.EmployeeId == 147)
                .OrderBy(e => $"{e.FirstName} {e.LastName}")
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    ProjectNames = e.EmployeesProjects
                        .OrderBy(p => p.Project.Name)
                        .Select(p => new
                        {
                            p.Project.Name
                        })
                        .ToArray()
                })
                .FirstOrDefault();

            sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
            foreach (var projectName in employee.ProjectNames)
            {
                sb.AppendLine($"{projectName.Name}");
            }

            return sb.ToString().TrimEnd();
        }

        //10. Departments with More Than 5 Employees
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var departments = context.Departments
                .Where(d => d.Employees.Count > 5)
                .OrderBy(d => d.Employees.Count)
                .ThenBy(d => d.Name)
                .Select(d => new
                {
                    d.Name,
                    ManagerFirstName = d.Manager.FirstName,
                    ManagerLastName = d.Manager.LastName,
                    EmployeesTitles = d.Employees
                        .OrderBy(e => e.FirstName)
                        .ThenBy(e => e.LastName)
                        .Select(e => new
                        {
                            e.FirstName,
                            e.LastName,
                            e.JobTitle
                        })
                })
                .ToArray();

            foreach (var d in departments)
            {
                sb.AppendLine($"{d.Name} - {d.ManagerFirstName} {d.ManagerLastName}");
                foreach (var e in d.EmployeesTitles)
                {
                    sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");
                }
            }
            return sb.ToString().TrimEnd();
        }

        //11. Find Latest 10 Projects
        public static string GetLatestProjects(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var projects = context.Projects
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .Select(p => new
                {
                    p.Name,
                    p.Description,
                    StartDate = p.StartDate.ToString("M/d/yyyy h:mm:ss tt"),
                })
                .ToArray();

            var orderByName = projects.OrderBy(p => p.Name);

            foreach (var p in orderByName)
            {
                sb.AppendLine(p.Name);
                sb.AppendLine(p.Description);
                sb.AppendLine(p.StartDate);
            }
            return sb.ToString().TrimEnd();
        }

        //12. Increase Salaries
        public static string IncreaseSalaries(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Where(e => e.Department.Name == "Engineering" ||
                            e.Department.Name == "Tool Design" ||
                            e.Department.Name == "Marketing" ||
                            e.Department.Name == "Information Services")
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToArray();

            foreach (var e in employees)
            {
                e.Salary *= 1.12m;
                sb.AppendLine($"{e.FirstName} {e.LastName} (${e.Salary:f2})");
            }

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        //13. Find Employees by First Name Starting With Sa
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employee = context.Employees
                .Where(e => e.FirstName.StartsWith("Sa"))
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary
                })
                .ToArray();

            foreach (var e in employee)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }

        //14. Delete Project by Id
        public static string DeleteProjectById(SoftUniContext context)
        {
            var epToDel = context.EmployeesProjects
                .Where(ep => ep.ProjectId == 2);
            context.EmployeesProjects.RemoveRange(epToDel);

            Project projectToDel = context.Projects.Find(2);
            context.Projects.RemoveRange(projectToDel);
            context.SaveChanges();

            string[] projects = context.Projects
                .Take(10)
                .Select(p => p.Name)
                .ToArray();

            return String.Join(Environment.NewLine, projects);
        }

        //15. Remove Town
        public static string RemoveTown(SoftUniContext context)
        {
            var townToDel = context.Towns
                .FirstOrDefault(t => t.Name == "Seattle");
            var addressesToDel = context.Addresses
                .Where(a => a.TownId == townToDel.TownId);
            var employeeAddresses = context.Employees
                .Where(e => addressesToDel.Any(a => a.AddressId == e.AddressId))
                .ToArray();

            int count = addressesToDel.Count();

            foreach (var e in employeeAddresses)
            {
                e.AddressId = null;
            }

            context.Addresses.RemoveRange(addressesToDel);
            context.Towns.Remove(townToDel);
            context.SaveChanges();

            return $"{count} addresses in Seattle were deleted";
        }
    }
}