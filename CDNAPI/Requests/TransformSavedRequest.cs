using System;

namespace CDNAPI.Requests
{
    public class TransformSavedRequest
    {
        public Guid Id { get; set; }
        public string OutputFormat { get; set; }
    }
}