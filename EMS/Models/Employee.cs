namespace EmployeeManagement
{
    public class Employee
    {
        public string EmployeeNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Dob { get; set; }
        public string EmailId { get; set; }
        public long MobileNumber { get; set; }
        public string JoiningDate { get; set; }
        public string Location { get; set; }
        public int LocationID {get; set; }
        public string JobTitle { get; set; }
        public int JobID { get; set; }
        public string Department { get; set; }
        public int DeptID { get; set; }
        public string Manager { get; set; }
        public string Project { get; set; }
    }
}