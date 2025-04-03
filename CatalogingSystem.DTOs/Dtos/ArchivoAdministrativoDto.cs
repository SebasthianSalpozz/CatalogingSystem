namespace CatalogingSystem.DTOs.Dtos;
using CatalogingSystem.Core.Enums; 

public class ArchivoAdministrativoDto
{
    public required TipoInstitucion Institucion { get; set; }
    public required string Unidad { get; set; }
    public required long Expediente { get; set; }
    public string? Serie { get; set; }
    public required TipoDocumentoOrigen DocumentoOrigen { get; set; }
    public DateTime FechaInicial { get; set; }
    public DateTime? FechaFinal { get; set; }
    public string? ExpedienteAnterior { get; set; }
    public string? Asunto { get; set; }
    public bool? PeticionTransferencia { get; set; }
    public string? Historial { get; set; }
    public string? ArchivoDocumental { get; set; }
    public string? Observaciones { get; set; }
}
