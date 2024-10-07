namespace DotnetAPI.DTOs;
public partial class AddUseSalaryDtos
{
    public decimal? Salary {get;set;}
    public decimal? AvgSalary {get;set;}
    public AddUseSalaryDtos()
    {
        if(Salary == null)
         Salary = 0;
        if(AvgSalary == null)
         AvgSalary = 0;
    }
}