using System.Collections.Generic;

namespace FaceGateway.Services
{
    public class Face
    {
        public string Name { get; set; }
        public IEnumerable<string> TrainingImageFiles { get; set; }
    }
}