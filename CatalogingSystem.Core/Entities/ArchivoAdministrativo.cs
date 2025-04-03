namespace CatalogingSystem.Core.Entities;
using CatalogingSystem.Core.Enums;

public class ArchivoAdministrativo
{
    public Guid Id { get; set; }
    public required TipoInstitucion institucion { get; set; }
    public required string unidad { get; set; }
    public required long expediente { get; set; }
    public string? serie { get; set; }
    public required TipoDocumentoOrigen documentoOrigen { get; set; }
    public DateTime fechaInicial { get; set; }
    public DateTime? fechaFinal { get; set; }
    public string? expedienteAnterior { get; set; }
    public string? asunto { get; set; }
    public bool? peticionTransferencia { get; set; }
    public string? historial { get; set; }
    public string? archivoDocumental { get; set; }
    public string? observaciones { get; set; }
}
