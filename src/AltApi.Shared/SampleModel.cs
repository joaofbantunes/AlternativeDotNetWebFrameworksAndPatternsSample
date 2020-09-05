using System;

namespace AltApi.Shared
{
    public class SampleInputModel
    {
        public Guid SomeId { get; set; }

        public string SomeDescription { get; set; }

        public DateTime SomeDate { get; set; }

        public SampleInnerModel SomeInnerModel { get; set; }
    }
    
    public class SampleOutputModel
    {
        public Guid SomeId { get; set; }

        public string SomeDescription { get; set; }

        public DateTime SomeDate { get; set; }

        public SampleInnerModel SomeInnerModel { get; set; }
    }

    public class SampleInnerModel
    {
        public Guid SomeInnerId { get; set; }
        
        public string SomeInnerDescription { get; set; }

        public DateTime SomeInnerDate { get; set; }
    }
}