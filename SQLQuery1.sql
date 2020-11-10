Create procedure [dbo].[GetEmployeeById]  
(  
   @EmpId int  
)  
as   
begin  
   Select * from Employee where EmpId=@EmpId 
End