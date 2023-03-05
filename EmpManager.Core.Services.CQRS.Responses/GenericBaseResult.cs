namespace EmpManager.Core.Services.CQRS.Responses
{
    public class GenericBaseResult<TModel> : BaseResult
    {
        public GenericBaseResult(TModel? model)
        {
            Result = model;
        }

        /// <summary>
        /// Result object.
        /// </summary>
        public TModel? Result { get; set; }
    }
}
