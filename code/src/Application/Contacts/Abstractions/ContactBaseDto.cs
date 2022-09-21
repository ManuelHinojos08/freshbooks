using System.ComponentModel;
namespace Application.Contacts.Abstractions;

public abstract class ContactBaseDto
{
    public int Id { get; set; }
    [DisplayName("Nombre")]
    public string Name { get; set; }
    [DisplayName("Telefono")]
    public string Phone { get; set; }
    [DisplayName("Email")]
    public string Email { get; set; }
}
