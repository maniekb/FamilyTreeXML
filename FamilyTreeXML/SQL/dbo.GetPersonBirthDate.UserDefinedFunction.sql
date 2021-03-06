USE [FamilyTreeX]
GO
/****** Object:  UserDefinedFunction [dbo].[GetPersonBirthDate]    Script Date: 30.06.2020 20:18:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GetPersonBirthDate] (
	@firstname VARCHAR(50),
	@lastname VARCHAR(50)
)
RETURNS table
AS
RETURN
    SELECT TOP 1
		p.value('(./BirthDate)[1]', 'VARCHAR(8000)') AS birthDate
	FROM [FamilyTreeX].[dbo].[FamilyTrees] 
		CROSS APPLY tree.nodes('/Tree//Family//*') t(p)
	WHERE p.value('(./Firstname)[1]', 'VARCHAR(8000)') = @firstname AND p.value('(./Lastname)[1]', 'VARCHAR(8000)') = @lastname
GO
