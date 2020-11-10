Create procedure [dbo].[AddNewEmployeeRecord]  
(  
   @Name varchar (50),  
   @Surname varchar (50),  
   @Gender nvarchar (50),
   @Email varchar(50),
   @Position varchar (50),  
   @Address varchar (50),  
   @HireDate nvarchar (50),
   @Salary varchar(50),
   @IDNumber varchar (50),  
   @PhoneNo nvarchar (50),
   @EmpId int 
)  
as  
begin  
   Insert into Employee values(@Name,@Surname,@Email,@PhoneNo,@Address,@Gender,@IDNumber,@Salary,@Position,@HireDate)  
   Set @EmpId = (Select  EmpId from Employee where Email = @Email)
    Insert into [dbo].[User] values(@Email,@IDNumber,@EmpId,0)   
End