namespace CatalogingSystem.Core.Enums;
using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TipoInstitucion
{
    UMRPSFXCH, 
    IglesiaDeChuquisaca,
    InstitucionPublica,
    InstitucionPrivada,
    ColeccionPublica,
    ColeccionPrivada,
    PersonaJuridica,
    PersonaNatural
}
