namespace Domain.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset? DateUpdated { get; set; }
        public DateTimeOffset? DateDeleted { get; set; }

        // Construtor para inicializar propriedades
        public BaseEntity()
        {
            Id = Guid.NewGuid(); // Gera um novo GUID para a entidade
            DateCreated = DateTimeOffset.UtcNow; // Define a data de criação para o momento atual
        }

        // Atualiza a data de atualização
        public void UpdateDate()
        {
            DateUpdated = DateTimeOffset.UtcNow;
        }

        // Marca a entidade como excluída
        public void MarkAsDeleted()
        {
            DateDeleted = DateTimeOffset.UtcNow;
        }
    }
}
