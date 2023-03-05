namespace EmpManager.Core.Domain.Models
{
    public class ModelBase
    {
        public ModelBase()
        {
            Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Id of the model (GUID).
        /// </summary>
        public string Id { get; set; }
    }
}