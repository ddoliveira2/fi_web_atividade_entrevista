CREATE PROC FI_SP_VerificaClienteBeneficios	
	@IdBeneficio  BIGINT,
	@IdCliente  BIGINT,
	@CPF VARCHAR(14)
AS
BEGIN
	SELECT 1 FROM BENEFICIARIOS WHERE CPF = @CPF and IDCLIENTE = @IdCliente and ID <> @IdBeneficio
END