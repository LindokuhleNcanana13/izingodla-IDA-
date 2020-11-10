create procedure [dbo].[AddToAllProjects]
(
	@AllProjId int,
	@Name varchar(50),
	@Date date,
	@Location varchar(50),
	@ProjectId int
)
as
begin
	insert into AllProjects values(@Name,@Date,0,@Location)
end