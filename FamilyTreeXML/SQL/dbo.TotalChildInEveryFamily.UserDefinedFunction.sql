USE [FamilyTreeX]
GO
/****** Object:  UserDefinedFunction [dbo].[TotalChildInEveryFamily]    Script Date: 30.06.2020 20:18:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[TotalChildInEveryFamily] ()
RETURNS TABLE
AS
RETURN
    SELECT
		id,
		p.value('count(Son)', 'int') + p.value('count(Daughter)', 'int') AS total
	FROM [FamilyTreeX].[dbo].[FamilyTrees]
		CROSS APPLY tree.nodes('/Tree//Family') t(p)
GO
