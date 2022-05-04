namespace MinimalApiAuth.Models;

public abstract class Model
{
    protected Model() => Id = Guid.NewGuid();
    
    public Guid Id { get; set; }
}