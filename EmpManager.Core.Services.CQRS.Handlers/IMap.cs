namespace EmpManager.Core.Services.CQRS.Handlers
{
    public interface IMap
    {
        /// <summary>
        /// Maps to destination.
        /// </summary>
        /// <typeparam name="TDest">Destination type.</typeparam>
        /// <param name="source">Source object to convert.</param>
        /// <returns>Converted object of type <typeparamref name="TDest"/>.</returns>
        TDest Map<TDest>(object source);
    }
}