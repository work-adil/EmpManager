﻿using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json.Serialization;

namespace EmpManager.Core.Services.CQRS.Responses
{
    public class BaseResult
    {
        public static bool IsInDebuggingMode { get; set; }
        public bool IsSuccess
         => (ResponseStatusCode == HttpStatusCode.OK || ResponseStatusCode == HttpStatusCode.Created || ResponseStatusCode == HttpStatusCode.Accepted);

        /// <summary>
        /// Message if any.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Errors if any.
        /// </summary>
        public IList<string>? Errors { get; set; }

        /// <summary>
        /// Errorcode.
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// HTTP status code.
        /// </summary>
        [JsonIgnore]
        public HttpStatusCode ResponseStatusCode { get; set; } = HttpStatusCode.OK;

        /// <summary>
        /// Adds exception to errors list in the response in debug mode.
        /// </summary>
        /// <param name="ex">Exception to add in error list.</param>
        public void AddExceptionLog(Exception ex)
        {
            if (ResponseStatusCode == HttpStatusCode.OK)
                ResponseStatusCode = HttpStatusCode.BadRequest;

            // It is really bad idea to show exceptions in production.
            if (IsInDebuggingMode || ex is ValidationException || ex is ArgumentNullException || ex is InvalidOperationException)
            {
                if (Errors == null) // Initialize if needed.
                    Errors = new List<string>();

                Errors.Add(ex.Message);
                if (ex.InnerException != null)
                    AddExceptionLog(ex.InnerException);
            }
        }
    }
}
