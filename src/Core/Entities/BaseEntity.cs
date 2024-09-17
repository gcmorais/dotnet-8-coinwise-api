namespace Domain.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; private set; }
        public DateTimeOffset DateCreated { get; private set; }
        public DateTimeOffset? DateUpdated { get; private set; }
        public DateTimeOffset? DateDeleted { get; private set; }

        // Construtor para inicializar propriedades
        protected BaseEntity()
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
