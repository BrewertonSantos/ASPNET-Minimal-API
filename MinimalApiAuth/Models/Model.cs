namespace MinimalApiAuth.Models;

public abstract class Model
{
    public Model() => Id = Guid.NewGuid();
    
    public Guid Id { get; set; }
}