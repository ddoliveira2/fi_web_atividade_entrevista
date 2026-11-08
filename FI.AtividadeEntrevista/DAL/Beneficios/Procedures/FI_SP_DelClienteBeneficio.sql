CREATE PROC FI_SP_DelClienteBeneficio
	@ID bigint
AS
BEGIN
	Delete from BENEFICIARIOS where ID = @ID	
END