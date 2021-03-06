USE [FamilyTreeX]
GO
/****** Object:  UserDefinedFunction [dbo].[GetPersonBirthDate]    Script Date: 30.06.2020 20:18:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GetFamilyIdByParentName] (
	@firstname VARCHAR(50),
	@lastname VARCHAR(50)
)
RETURNS table
AS
RETURN
    SELECT
		p.value('(/Tree//Family/@Id)[1]', 'INT') AS Id
	FROM [FamilyTreeX].[dbo].[FamilyTrees] 
		CROSS APPLY tree.nodes('/Tree//Family') t(p)
	WHERE (p.value('(./Father//Firstname)[1]', 'VARCHAR(8000)') = @firstname AND p.value('(./Father//Lastname)[1]', 'VARCHAR(8000)') = @lastname) OR 
			(p.value('(./Mother//Firstname)[1]', 'VARCHAR(8000)') =