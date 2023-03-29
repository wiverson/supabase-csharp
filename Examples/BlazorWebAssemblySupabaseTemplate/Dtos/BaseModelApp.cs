using Postgrest.Attributes;
using Postgrest.Models;

public class BaseModelApp : BaseModel
{
    [PrimaryKey("id", false)] // Key is Autogenerated
    public int Id { get; set; }

    // There is a rule in the definition of this table to set to false as default value to this field.
    // So, It's necessary to set default value in the class creation bacause the insert method would send this value as null.
    // This way, the sent row wouldn't be set to false automaticaly by Postgre default value
    // because the field was sent with null value.
    // To fix this, the null fields should not be serialized in the request.
    [Column("SoftDeleted")]
    public bool? SoftDeleted { get; set; } = false;
    
    [Column("SoftDeletedAt")]
    public DateTime? SoftDeletedAt { get; set; }

}