Create procedure [dbo].[AddNewClientRecord]  
(  
   @Name varchar (50),  
   @Surname varchar (50),
   @Email varchar(50),
   @Password varchar (50),  
   @Company varchar(50),
   @PhoneNo nvarchar (50),
   @ClientId int 
)  
as  
begin  
   Insert into Client values(@Name,@Surname,@Email,@PhoneNo,@Company,@Password)  
   Set @ClientId = (Select  ClientId from Client where Email = @Email)
    Insert into [dbo].[User] values(@Email,@Password,0,@ClientId)   
End