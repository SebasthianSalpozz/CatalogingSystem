namespace CatalogingSystem.Core.Enums;
using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TipoDocumentoOrigen
{
    PorDecomiso,
    Compra,
    Dacion,
    Excavacion, 
    Legado,
    Donacion,
    OrdenacionYReordenacion,
    Permuta,
    Premios,
    Usucapion,
    AltaPorReintegracion,
    CambioDeAdscripcion,
    Ofrenda,
    ProduccionPropia,
    Recoleccion,
    DepositoDeTitularidadPublica,
    DepositoDeTerceros,
    Estudio,
    Exposicion,
    Conservacion,
    DepositoJudicial,
    DepositoPrevioAAdquisicion
}
